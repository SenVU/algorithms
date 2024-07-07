using GXPEngine;
using GXPEngine.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;


abstract class Dungeon : Canvas
{
    public Random random = null;
    //the (unscaled) dimensions of the dungeon (basically how 'tiles' wide and high)
    public readonly Size size;

	//base implementation assumes dungeon consists of rooms and doors, adapt in subclass if needed
	public readonly List<Room> rooms = new List<Room>();
	public readonly List<Door> doors = new List<Door>();

	//Set this to false if you want to do all drawing yourself from the generate method.
	//This might be handy while debugging your own algorithm.
	protected bool autoDrawAfterGenerate = true;


	//The colors for the walls and doors
	//TODO:try changing 255 to 128 to see where the room boundaries are...
	private Brush roomFillBrush = Brushes.White;
    //private Pen roomFillBrush = Pens.White;
    private Pen wallPen = Pens.Black;
	private Pen doorPen = Pens.White;

	/**
	 * Create empty dungeon instance of the specified size.
	 * It's empty because it doesn't contain any rooms yet.
	 */
	public Dungeon(Size size) : base(size.Width, size.Height)
	{
		this.size = size;

		/**/
		//ignore lines below, this is for rendering scaled canvasses without blurring 
		//Comment it out if you feel lucky or just want to see what this is doing.
		_texture.Bind();
		GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
		GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
		_texture.Unbind();
		/**/

		System.Console.WriteLine(this.GetType().Name + " created.");
	}

	/**
	 * Clears all rooms and doors, calls generate (note the lower case),
	 * and visualizes the result by drawing on the canvas.
	 * 
	 * @param pMinimumRoomSize the minimum size that a room should have
	 */
	public void StartGeneration(int seed)
	{
        random = new Random(seed);
        Console.WriteLine(this.GetType().Name + ".Generate:Generating dungeon...");

		rooms.Clear();
		doors.Clear();

		Generate();

		Console.WriteLine($"Total Rooms {rooms.Count}");
		Console.WriteLine($"Total Doors {doors.Count}");
		Console.WriteLine(this.GetType().Name + ".Generate:Dungeon generated.");

		if (autoDrawAfterGenerate) Draw();
	}

    public void StartGeneration()
    {
        StartGeneration(Environment.TickCount);
    }

    protected abstract void Generate();

    /////////////////////////////////////////////////////////////////////////////////////////
    ///	This section contains helper methods to draw all or specific doors/rooms
    ///	You can call them from your own methods to actually draw the dungeon during/after generation
    ///	These methods do not have to be changed.

    protected virtual void Draw()
	{
		graphics.Clear(Color.Transparent);
		DrawRooms(rooms, wallPen, roomFillBrush);    
		DrawDoors(doors, doorPen);
	}

    /**
	 * Draw all rooms in the given list with the given color, eg drawRooms (_rooms, Pen.Black)
	 * @param pRooms		the list of rooms to draw
	 * @param pWallColor	the color of the walls
	 * @param pFillColor	if not null, the color of the inside of the room, if null insides will be transparent
	 */
    protected virtual void DrawRooms(IEnumerable<Room> pRooms, Pen wallColor, Brush fillColor = null)
	{
		foreach (Room room in pRooms)
		{
			DrawRoom(room, wallColor, fillColor);
		}
	}

    /**
	 * Draws a single room in the given color.
	 * @param pRoom			the room to draw
	 * @param pWallColor	the color of the walls
	 * @param pFillColor	if not null, the color of the inside of the room, if null insides will be transparent
	 */
    protected virtual void DrawRoom(Room room, Pen wallColor, Brush fillColor = null)
	{
		//the -0.5 has two reasons:
		//- Doing it this way actually makes sure that an area of 0,0,4,4 (x,y,width,height) is draw as an area of 0,0,4,4
		//- Doing it this way makes sure that an area of 0,0,1,1 is ALSO drawn (which it wouldn't if you used -1 instead 0.5f)
		if (fillColor != null) graphics.FillRectangle(fillColor, room.area.Left, room.area.Top, room.area.Width - 0.5f, room.area.Height - 0.5f);
		graphics.DrawRectangle(wallColor, room.area.Left, room.area.Top, room.area.Width - 0.5f, room.area.Height - 0.5f);
	}

    protected virtual void DrawDoors(IEnumerable<Door> doors, Pen color)
	{
		foreach (Door door in doors)
		{
			DrawDoor(door, color);
		}
	}

    protected virtual void DrawDoor(Door door, Pen color)
	{
		//note the 0.5, 0.5, this forces the drawing api to draw at least 1 pixel ;)
		graphics.DrawRectangle(color, door.area.X, door.area.Y, door.area.Width-.5f, door.area.Height-.5f);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////
	///	This section contains helper methods to print information about the dungeon to the console

	public override string ToString()
	{
		return "Dungeon: implement/override this method to print info about all rooms and doors";
	}

	/// <summary>
	/// gets door at X,Y
	/// </summary>
	public Room GetRoomAt(int x, int y)
	{
		foreach (Room room in rooms)
		{
			Rectangle area = room.area;
			if (
				area.X <= x &&
				area.Y <= y &&
                area.X + area.Width >= x &&
                area.Y + area.Height >= y
				)
			{
				return room;
			}
		}
		return null;
	}

	/// <summary>
	/// gets all rooms in the rectengle
	/// </summary>
    public List<Room> GetRoomsIn(Rectangle areaToCheck)
    {
		List<Room> toReturn = new List<Room>();
        foreach (Room room in rooms)
        {
            Rectangle roomArea = room.area;
            if (
				areaToCheck.X + areaToCheck.Width >= roomArea.X &&
                areaToCheck.X <= roomArea.X + roomArea.Width &&
                areaToCheck.Y + areaToCheck.Height >= roomArea.Y &&
                areaToCheck.Y <= roomArea.Y + roomArea.Height)
            {
                toReturn.Add(room);
            }
        }
        return toReturn;
    }

	public void SetScale(float scale) { this.scale = scale; }
}