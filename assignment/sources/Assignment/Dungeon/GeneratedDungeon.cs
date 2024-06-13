using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;

internal class GeneratedDungeon : Dungeon
{
    public readonly List<Room> roomsQueue = new List<Room>();
    public readonly List<Room> roomsDone = new List<Room>();
    public GeneratedDungeon(Size size) : base(size) { }


    protected override void Generate()
    {
        // clear all lists (just in case)
        roomsQueue.Clear();
        roomsDone.Clear();

        //create the first room
        roomsQueue.Add(new Room(new Rectangle(0, 0, size.Width, size.Height),this));

        // if the roomsTODO list has rooms and you have not exceeded maxloops check if the rooms in roomsTODO can be split
        GenerateRooms();
        AddLeftoverRooms();

        // this wil generate a door between all possible doors id generateAllDoors is true
        if (AlgorithmsAssignment.generateAllDoors)
        {
            FindConnectedRooms();
            AddDoorToConnections();
        }
        foreach (Door door in doors)
        {
            door.FindConnectedRooms(this);
            Console.WriteLine(door);
        }

        ShrinkRooms();
    }


    /// <summary>
    /// loops through all queued rooms untill none are left or the max amount of loops is reached
    /// </summary>
    void GenerateRooms()
    {
        //save the amount of loops so i dont go over the max
        int loop = 0;

        while (roomsQueue.Count > 0 && loop < AlgorithmsAssignment.maxGenerationLoops)
        {
            //get room at 0
            Room currentRoom = roomsQueue[0];

            // rooms can be split if the size is bigger that pMinimumRoomSize * 2
            // also a random chanse to finalize the room called finalizeRoomChanse
            if ((currentRoom.area.Width > AlgorithmsAssignment.minRoomSize * 2 || currentRoom.area.Height > AlgorithmsAssignment.minRoomSize * 2) && random.NextDouble() > AlgorithmsAssignment.finalizeRoomChanse)
            {
                bool splitsHorizontal = currentRoom.area.Width >= currentRoom.area.Height;
                SplitRoom(currentRoom, AlgorithmsAssignment.minRoomSize, splitsHorizontal);
                roomsQueue.Remove(currentRoom);
            }
            // if the rest fails the room should not be split and will be moved to the roomsDone list
            else
            {
                rooms.Add(currentRoom);
                roomsQueue.Remove(currentRoom);
                continue;
            }

            Console.WriteLine($"loop {loop} finished");
            loop++;
        }
    }

    /// <summary>
    /// will split a room
    /// </summary>
    void SplitRoom(Room room, int minRoomSize, bool splitsHorizontal)
    {
        //splitsHorizontal = true;
        float randF = (float)random.NextDouble();

        Rectangle room1Rect;
        Rectangle room2Rect;

        if (splitsHorizontal)
        { // split over width
            int randWallDist = Mathf.Round(minRoomSize + (room.area.Width - minRoomSize * 2) * randF);
            room1Rect = new Rectangle(room.area.X, room.area.Y, randWallDist + 1, room.area.Height);
            room2Rect = new Rectangle(room.area.X + randWallDist, room.area.Y, room.area.Width - randWallDist, room.area.Height);
            roomsQueue.Add(new Room(room1Rect, this));
            roomsQueue.Add(new Room(room2Rect, this));
        }
        else
        { // split over height
            int randWallDist = Mathf.Round(minRoomSize + (room.area.Height - minRoomSize * 2) * randF);
            room1Rect = new Rectangle(room.area.X, room.area.Y, room.area.Width, randWallDist + 1);
            room2Rect = new Rectangle(room.area.X, room.area.Y + randWallDist, room.area.Width, room.area.Height - randWallDist);
            roomsQueue.Add(new Room(room1Rect, this));
            roomsQueue.Add(new Room(room2Rect, this));
        }
        if (!AlgorithmsAssignment.generateAllDoors) GenerateDoorOnSplit(splitsHorizontal, room2Rect, minRoomSize);
    }

    void GenerateDoorOnSplit(bool splitsHorizontal, Rectangle room2Rect, int minRoomSize)
    {
        int dist = random.Next(AlgorithmsAssignment.maxShrink + 1, minRoomSize - (AlgorithmsAssignment.maxShrink + 1));
        if (splitsHorizontal)
        {
            if (random.NextDouble() >= .5) doors.Add(new Door(new Point(room2Rect.X, room2Rect.Y + dist),this,splitsHorizontal));
            else doors.Add(new Door(new Point(room2Rect.X, room2Rect.Y + room2Rect.Height -1 - dist),this,splitsHorizontal));
        }
        else
        {
            if (random.NextDouble() >= .5) doors.Add(new Door(new Point(room2Rect.X + dist, room2Rect.Y),this,splitsHorizontal));
            else doors.Add(new Door(new Point(room2Rect.X + room2Rect.Width -1 - dist, room2Rect.Y),this, splitsHorizontal));
        }
    }

    /// <summary>
    /// will request a room to check for ajacent rooms to the rigth and at the bottom
    /// </summary>
    void FindConnectedRooms()
    {
        foreach (Room room in rooms)
        {
            room.rightConnections = GetRoomsIn(new Rectangle(room.area.X + room.area.Width + 1, room.area.Y + 3, 1, room.area.Height - 6));
            room.bottomConnections = GetRoomsIn(new Rectangle(room.area.X + 3, room.area.Y + room.area.Height + 1, room.area.Width - 6, 1));
        }
    }

    /// <summary>
    /// will add a door at every connection between all rooms
    /// </summary>
    void AddDoorToConnections()
    {
        foreach (Room room in rooms)
        {
            foreach (Room connection in room.rightConnections) { addDoorRight(room, connection); }
            foreach (Room connection in room.bottomConnections) { AddDoorBottom(room, connection); }
        }
    }

    /// <summary>
    /// will add a room between a room and an ajacent room on the right
    /// </summary>
    void addDoorRight(Room room, Room connection)
    {
        int minHeight = Math.Max(room.area.Y + AlgorithmsAssignment.maxShrink + 1, connection.area.Y + AlgorithmsAssignment.maxShrink + 1);
        int maxHeight = Math.Min(room.area.Y + room.area.Height - 1 - AlgorithmsAssignment.maxShrink, connection.area.Y + connection.area.Height - 1 - AlgorithmsAssignment.maxShrink);
        if (minHeight <= 0 || maxHeight <= 0 || maxHeight - minHeight <= 0) return;
        Door door = new Door(new Point(connection.area.X, minHeight + random.Next(maxHeight - minHeight)),this, true);
        door.SetConnectedRooms(room, connection);
        doors.Add(door);
    }

    /// <summary>
    /// will add a room between a room and an ajacent room on the bottom
    /// </summary>
    void AddDoorBottom(Room room, Room connection)
    {
        int minWidth = Math.Max(room.area.X + AlgorithmsAssignment.maxShrink + 1, connection.area.X + AlgorithmsAssignment.maxShrink + 1);
        int maxWidth = Math.Min(room.area.X + room.area.Width - 1-AlgorithmsAssignment.maxShrink, connection.area.X + connection.area.Width - 1-AlgorithmsAssignment.maxShrink);
        if (minWidth <= 0 || maxWidth <= 0 || maxWidth - minWidth <= 0) return;
        Door door = new Door(new Point(minWidth + random.Next(maxWidth - minWidth), connection.area.Y),this,false);
        door.SetConnectedRooms(room, connection);
        doors.Add(door);
    }

    /// <summary>
    /// adds the remaining TODO rooms to the main list
    /// </summary>
    void AddLeftoverRooms()
    {
        foreach (Room room in roomsQueue)
        {
            Console.WriteLine(room);
            if (!rooms.Contains(room))
            {
                rooms.Add(room);
            }
        }
    }

    /// <summary>
    /// will shrink all rooms randomly
    /// </summary>
    void ShrinkRooms()
    {
        foreach (Room room in rooms)
        {
            int randShrink = random.Next(AlgorithmsAssignment.minShrink, AlgorithmsAssignment.maxShrink +1);
            room.Shrink(randShrink);
        }
    }
}