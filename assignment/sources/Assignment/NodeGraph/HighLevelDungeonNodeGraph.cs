using System;
using System.Diagnostics;
using System.Drawing;

/**
 * An example of a dungeon nodegraph implementation.
 * 
 * This implementation places only three nodes and only works with the SampleDungeon.
 * Your implementation has to do better :).
 * 
 * It is recommended to subclass this class instead of NodeGraph so that you already 
 * have access to the helper methods such as getRoomCenter etc.
 * 
 * TODO:
 * - Create a subclass of this class, and override the generate method, see the generate method below for an example.
 */
 class HighLevelDungeonNodeGraph : NodeGraph
{
    protected Dungeon dungeon;

    public HighLevelDungeonNodeGraph(Dungeon dungeon) : base((int)(dungeon.size.Width * dungeon.scale), (int)(dungeon.size.Height * dungeon.scale), Math.Max((int)dungeon.scale/5,1))
	{
		Debug.Assert(dungeon != null, "Please pass in a dungeon.");

		this.dungeon = dungeon;
	}

	protected override void Generate ()
	{
		if (AlgorithmsAssignment.nodeGraphHighQuality) { HighQualityGenerate(); }
		else { LowQualityGenerate (); }
		
	}

	void LowQualityGenerate()
	{
		foreach (Room room in dungeon.rooms)
		{
			TryPlaceNode(room.GetCenterPoint());
		}
		foreach (Door door in dungeon.doors)
		{
			door.GenerateNodes(this);
		}
	}

	void HighQualityGenerate()
	{
        foreach (Room room in dungeon.rooms)
        {
            for (int i = 0; i < (room.area.Width - 2) * (room.area.Height - 2); i++)
            {
                TryPlaceNode(new Point((room.area.X + 1 + i % (room.area.Width - 2))*(int)dungeon.scale + ((int)dungeon.scale / 2), 
					(room.area.Y + 1 + i / (room.area.Width - 2)) * (int)dungeon.scale + ((int)dungeon.scale / 2)));
            }
        }

        foreach (Door door in dungeon.doors)
        {
            for (int i = 0; i < door.area.Width * door.area.Height; i++)
            {
                TryPlaceNode(new Point((door.area.X  + i % (door.area.Width)) * (int)dungeon.scale + ((int)dungeon.scale / 2), 
					(door.area.Y + i / (door.area.Width)) * (int)dungeon.scale + ((int)dungeon.scale / 2)));
            }
        }

		foreach (Node node in GetNodes())
		{
			ConnectNodeToNeighbours(node);
		}
    }

    protected void ConnectNodeToNeighbours(Node node)
    {
        Node left = GetNodeAt(node.location.X - (int)dungeon.scale, node.location.Y);
        Node right = GetNodeAt(node.location.X + (int)dungeon.scale, node.location.Y);
        Node up = GetNodeAt(node.location.X, node.location.Y - (int)dungeon.scale);
        Node down = GetNodeAt(node.location.X, node.location.Y + (int)dungeon.scale);

		if (left != null && !node.IsConnectedTo(left)) { AddConnection(node, left); }
		if (right != null && !node.IsConnectedTo(right)) { AddConnection(node, right); }
		if (up != null && !node.IsConnectedTo(up)) { AddConnection (node, up); }
		if (down != null && !node.IsConnectedTo(down)) { AddConnection (node, down); }

        Node leftUp = GetNodeAt(node.location.X - (int)dungeon.scale, node.location.Y - (int)dungeon.scale);
        Node leftDown = GetNodeAt(node.location.X - (int)dungeon.scale, node.location.Y + (int)dungeon.scale);
        Node rightUp = GetNodeAt(node.location.X + (int)dungeon.scale, node.location.Y - (int)dungeon.scale);
        Node rightDown = GetNodeAt(node.location.X + (int)dungeon.scale, node.location.Y + (int)dungeon.scale);

		if (left == null) { leftUp = null; leftDown = null; }
		if (right == null) { rightUp = null; rightDown = null; }
		if (up == null) { leftUp = null; rightUp = null; }
		if (down == null) { leftDown = null; rightDown = null; }

        if (leftUp != null && !node.IsConnectedTo(leftUp)) { AddConnection(node, leftUp); }
        if (leftDown != null && !node.IsConnectedTo(leftDown)) { AddConnection(node, leftDown); }
        if (rightUp != null && !node.IsConnectedTo(rightUp)) { AddConnection(node, rightUp); }
        if (rightDown != null && !node.IsConnectedTo(rightDown)) { AddConnection(node, rightDown); }
    }
}
