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
            for (int y = room.area.Y + 1; y < room.area.Y + room.area.Height - 1; y++)
                for (int x = room.area.X + 1; x < room.area.X + room.area.Width - 1; x++)
                    SetTileType(x, y, TileType.GROUND);

        foreach (Door door in dungeon.doors)
            for (int y = door.area.Y; y < door.area.Y + door.area.Height - 0; y++)
                for (int x = door.area.X; x < door.area.X + door.area.Width - 0; x++)
                    SetTileType(x, y, TileType.GROUND);

    }
}

