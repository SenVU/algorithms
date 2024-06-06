using GXPEngine;

/**
 * This is an example subclass of the TiledView that just generates random tiles.
 */
class TiledDungeonView : TiledView
{
	Dungeon dungeon = null;
	public TiledDungeonView(Dungeon dungeon) : base(dungeon.size.Width, dungeon.size.Height, (int)dungeon.scale, TileType.WALL)
	{
		this.dungeon = dungeon;
	}

	/**
	 * Fill the tileview with random data instead.
	 * In your subclass, you should set the tiletype correctly based on the provided dungeon contents.
	 */
	protected override void generate()
	{
		foreach (Room room in dungeon.rooms)
		{
            for (int i = 0; i < (room.area.Width - 2)*(room.area.Height-2); i++)
            {
                SetTileType(room.area.X + 1 + i % (room.area.Width-2), room.area.Y + 1 + i / (room.area.Width-2), TileType.GROUND);
            }
        }

        foreach (Door door in dungeon.doors)
        {
            for (int i = 0; i < door.area.Width * door.area.Height; i++)
            {
                SetTileType(door.area.X + i % door.area.Width, door.area.Y + i / door.area.Width, TileType.GROUND);
            }
        }
    }
}

