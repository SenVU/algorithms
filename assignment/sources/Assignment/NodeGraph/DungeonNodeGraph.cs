using System;
using System.Diagnostics;
using System.Drawing;

 class DungeonNodeGraph : NodeGraph
{
    protected Dungeon dungeon;

    public DungeonNodeGraph(Dungeon dungeon) : base((int)(dungeon.size.Width * dungeon.scale), (int)(dungeon.size.Height * dungeon.scale), Math.Max((int)dungeon.scale/5,1))
	{
		Debug.Assert(dungeon != null, "Please pass in a dungeon.");
		this.dungeon = dungeon;
	}


	protected override void Generate ()
	{
        foreach (Room room in dungeon.rooms)
        {
            TryPlaceNode(room.GetCenterPoint());
        }
        foreach (Door door in dungeon.doors)
        {
            GenerateDoorNodes(door,this);
        }
	}

    /// <summary>
	/// Generates a node at each end of the door
	/// </summary>
	protected void GenerateDoorNodes(Door door, NodeGraph nodeGraph)
    {
        Node nodeA = nodeGraph.TryPlaceNode(new Point((int)(dungeon.scale * door.area.X) + ((int)dungeon.scale / 2), (int)(dungeon.scale * door.area.Y) + ((int)dungeon.scale / 2)));
        Node nodeB = nodeGraph.TryPlaceNode(new Point((int)(dungeon.scale * (door.area.X + door.area.Width - 1)) + ((int)dungeon.scale / 2), (int)(dungeon.scale * (door.area.Y + door.area.Height - 1)) + ((int)dungeon.scale / 2)));

        nodeGraph.AddConnection(nodeA, nodeB);
        Node roomANode = nodeGraph.GetNodeAt(door.GetRoomA().GetCenterPoint());
        Node roomBNode = nodeGraph.GetNodeAt(door.GetRoomB().GetCenterPoint());

        if (roomANode != null) nodeGraph.AddConnection(nodeA, roomANode);
        if (roomBNode != null) nodeGraph.AddConnection(nodeB, roomBNode);
    }
}
