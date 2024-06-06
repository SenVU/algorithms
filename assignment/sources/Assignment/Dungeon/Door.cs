using System;
using System.Drawing;
using System.Dynamic;

/**
 * This class represents (the data for) a Door, at this moment only a position in the dungeon.
 * Changes to this class might be required based on your specific implementation of the algorithm.
 */
class Door
{
	public readonly Rectangle area;

	Dungeon dungeon = null;

	//Keeping tracks of the Rooms that this door connects to
	Room roomA = null;
	Room roomB = null;

	public bool horizontal = false;

	public Door(Rectangle area, Dungeon dungeon)
	{
		this.area = area;
		this.dungeon = dungeon;
	}

	public Door(Point location, Dungeon dungeon, bool horizontal)
	{
		int maxShrink = AlgorithmsAssignment.maxShrink;
		this.dungeon = dungeon;
		if (horizontal) this.area = new Rectangle(location.X - maxShrink - 1, location.Y, (maxShrink) * 2 + 3, 1);
		else this.area = new Rectangle(location.X, location.Y - maxShrink - 1, 1, (maxShrink) * 2 + 3);
	}

	public Door(Point location)
	{
		this.area = new Rectangle(location.X, location.Y, 1, 1);
	}

	public override string ToString()
	{
		return $"Door: (X:{area.X}Y:{area.Y}) Width:{area.Width}, Height:{area.Height}, roomA found:{roomA != null}, roomB found:{roomB != null}";
	}

	public void SetConnectedRooms(Room roomA, Room roomB)
	{
		this.roomA = roomA;
		this.roomB = roomB;
	}

	/// <summary>
	/// finds the room at the entrance and exit of the door
	/// </summary>
	public void FindConnectedRooms(Dungeon dungeon)
	{
		roomA = dungeon.GetRoomAt(area.X, area.Y);
		roomB = dungeon.GetRoomAt(area.X + area.Width - 1, area.Y + area.Height - 1);
	}

	public Point GetCenterPoint()
	{
		return new Point(area.X + area.Width / 2, area.Y + area.Height / 2);
	}

	public void GenerateNodes(NodeGraph nodeGraph)
	{
		Node nodeA = new Node(new Point((int)(dungeon.scale * area.X) + ((int)dungeon.scale / 2), (int)(dungeon.scale * area.Y) + ((int)dungeon.scale / 2)), nodeGraph);
		Node nodeB = new Node(new Point((int)(dungeon.scale * (area.X + area.Width - 1)) + ((int)dungeon.scale / 2), (int)(dungeon.scale * (area.Y + area.Height - 1)) + ((int)dungeon.scale / 2)), nodeGraph);

		nodeGraph.AddConnection(nodeA, nodeB);
		Node roomANode = nodeGraph.GetNodeAt(roomA.GetCenterPoint());
		Node roomBNode = nodeGraph.GetNodeAt(roomB.GetCenterPoint());

		if (roomANode != null) nodeGraph.AddConnection(nodeA, roomANode);
		if (roomBNode != null) nodeGraph.AddConnection(nodeB, roomBNode);
	}
}

