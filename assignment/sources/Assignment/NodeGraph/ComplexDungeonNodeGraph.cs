using System;
using System.Diagnostics;
using System.Drawing;


 class ComplexDungeonNodeGraph : NodeGraph
{
    protected Dungeon dungeon;

    public ComplexDungeonNodeGraph(Dungeon dungeon) : base((int)(dungeon.size.Width * dungeon.scale), (int)(dungeon.size.Height * dungeon.scale), Math.Max((int)dungeon.scale/5,1))
	{
		Debug.Assert(dungeon != null, "Please pass in a dungeon.");

		this.dungeon = dungeon;
	}

    protected override void Generate()
    {
        foreach (Room room in dungeon.rooms)
        {
            for (int i = 0; i < (room.area.Width - 2) * (room.area.Height - 2); i++)
            {
                TryPlaceNode(new Point((room.area.X + 1 + i % (room.area.Width - 2)) * (int)dungeon.scale + ((int)dungeon.scale / 2),
                    (room.area.Y + 1 + i / (room.area.Width - 2)) * (int)dungeon.scale + ((int)dungeon.scale / 2)));
            }
        }

        foreach (Door door in dungeon.doors)
        {
            for (int i = 0; i < door.area.Width * door.area.Height; i++)
            {
                TryPlaceNode(new Point((door.area.X + i % (door.area.Width)) * (int)dungeon.scale + ((int)dungeon.scale / 2),
                    (door.area.Y + i / (door.area.Width)) * (int)dungeon.scale + ((int)dungeon.scale / 2)));
            }
        }
        Console.WriteLine(GetNodes().Length);
        foreach (Node node in GetNodes())
        {
            ConnectNodeToNeighbours(node);
            // gets stuck looping here
        }
    }

    /*private void AddNodeConnection(Node nodeA, Node nodeB)
    {
        nodeA.AddConnection(nodeB);
        nodeB.AddConnection(nodeA);
    }*/

    /// <summary>
    /// finds and connects a node to all nodes directly next to it.
    /// </summary>
    protected void ConnectNodeToNeighbours(Node node)
    {
        if (node == null) return;
        Console.WriteLine(node);
        
        Node right = GetNodeAt(node.location.X + (int)dungeon.scale, node.location.Y);
        Node down = GetNodeAt(node.location.X, node.location.Y + (int)dungeon.scale);

        if (right != null) { AddConnection(node, right); }
		if (down != null) { AddConnection (node, down); }

        if (AlgorithmsAssignment.nodeGraphHighQualityDiagonals)
        {
            Node rightUp = GetNodeAt(node.location.X + (int)dungeon.scale, node.location.Y - (int)dungeon.scale);
            Node rightDown = GetNodeAt(node.location.X + (int)dungeon.scale, node.location.Y + (int)dungeon.scale);

            bool noNodeUp = GetNodeAt(node.location.X, node.location.Y - (int)dungeon.scale) == null;

            if (noNodeUp || right == null) { rightUp = null; }
            if (down == null || right == null) { rightDown = null; }

            if (rightUp != null && !node.IsConnectedTo(rightUp)) { AddConnection(node, rightUp); }
            if (rightDown != null && !node.IsConnectedTo(rightDown)) { AddConnection(node, rightDown); }
        }
    }
}
