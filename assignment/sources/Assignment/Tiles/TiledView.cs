using GXPEngine;
using GXPEngine.Core;
using System.Diagnostics;


abstract class TiledView : GameObject
{
	//the dimensions of the tileview
	public int columns { get; private set; }
	public int rows { get; private set; }
	//stores the tiletype for each cell (and with it whether a tile is walkable)
	private TileType[,] _tileData;
	//used to reset all data to the default tiletype when requested
	private TileType defaultTileType;
	//single sprite, used for rendering all tiles
	private AnimationSprite tileSet;

	public TiledView(int columns, int rows, int tileSize, TileType defaultTileType) {
		Debug.Assert(columns > 0, "Invalid amount of columns passed in: " + columns);
		Debug.Assert(rows > 0, "Invalid amount of rows passed in: " + rows);
		Debug.Assert(defaultTileType != null, "Invalid default tile type passed in:" + defaultTileType);

		this.columns = columns;
		this.rows = rows;

		this.defaultTileType = defaultTileType;

		//we use a single sprite to render the whole tileview
		tileSet = new AnimationSprite("assets/tileset.png", 3, 1);
		tileSet.width = tileSet.height = tileSize;

		InitializeTiles();
	}

    /// <summary>
	/// initialize all tiles to default
	/// </summary>
    private void InitializeTiles ()
	{
		_tileData = new TileType[columns, rows];
		ResetAllTilesToDefault();
	}

	protected void ResetAllTilesToDefault()
	{
		//a 'trick' to do everything in one for loop instead of a nested loop
		for (int i = 0; i < columns * rows; i++)
		{
			_tileData[i % columns, i / columns] = defaultTileType;
		}
	}

	public void SetTileType(int column, int row, TileType tileType)
	{
		//an example of hardcore defensive coding;)
		Debug.Assert(column >= 0 && column < columns, "Invalid column passed in: " + column);
		Debug.Assert(row >= 0 && row < rows, "Invalid row passed in:" + row);
		Debug.Assert(tileType != null, "Invalid tile type passed in:" + tileType);

		_tileData[column, row] = tileType;
	}

	public TileType GetTileType(int pColumn, int pRow)
	{
		Debug.Assert(pColumn >= 0 && pColumn < columns, "Invalid column passed in: " + pColumn);
		Debug.Assert(pRow >= 0 && pRow < rows, "Invalid row passed in:" + pRow);

		return _tileData[pColumn, pRow];
	}

	protected override void RenderSelf(GLContext glContext)
	{
		//another way of rendering you might not be used to. Instead of adding all 
		//seperate sprites, we override the RenderSelf method, move a sprite around
		//like a stamp and 'stamp' the sprite onto the screen by calling its render method
		for (int column = 0; column < columns; column++)
		{
			for (int row = 0; row < rows; row++)
			{
				if (GetTileType(column, row) == null) continue;
                tileSet.currentFrame = GetTileType(column, row).id;
				tileSet.x = column * tileSet.width;
				tileSet.y = row * tileSet.height;
				tileSet.Render(glContext);
			}
		}
	}

	/**
	 * Trigger the tile view generation process, do not override this method, 
	 * but override generate (note the lower case) instead.
	 */
	public void StartGeneration()
	{
		ResetAllTilesToDefault();
		System.Console.WriteLine(this.GetType().Name + ".Generate: Generating tile view...");
		Generate();
		System.Console.WriteLine(this.GetType().Name + ".Generate: tile view generated.");
	}

	protected abstract void Generate();

}

