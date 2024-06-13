using GXPEngine;


class TiledDungeonView : TiledView
{
	Dungeon dungeon = null;
	public TiledDungeonView(Dungeon dungeon) : base(dungeon.size.Width, dungeon.size.Height, (int)dungeon.scale, TileType.WALL)
	{
		this.dungeon = dungeon;
    }

	protected override void Generate()
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

