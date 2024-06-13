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
            door.GenerateNodes(this);
        }
	}
}
