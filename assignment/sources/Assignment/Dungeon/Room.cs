using System.Collections.Generic;
using System.Drawing;

class Room
{

	Dungeon dungeon;

	public List<Room> rightConnections = new List<Room>();
	public List<Room> bottomConnections = new List<Room>();


	public Rectangle area;

	public Room (Rectangle area, Dungeon dungeon)
	{
		this.area = area;
		this.dungeon = dungeon;
	}

	public override string ToString()
	{
		return $"room: (X:{area.X},Y:{area.Y}) Width:{area.Width} Heigt:{area.Height}";
	}

	/// <summary>
	/// moves all rooms inward by the shrink value
	/// </summary>
    public void Shrink(int shrink)
    {
		area.X += shrink;
		area.Y += shrink;
		area.Width -= shrink * 2;
		area.Height -= shrink * 2;
    }

	/// <summary>
	/// gets the center of the room
	/// </summary>
    public Point GetCenterPoint()
    {
        float centerX = ((area.Left + area.Right) / 2.0f) * dungeon.scale;
        float centerY = ((area.Top + area.Bottom) / 2.0f) * dungeon.scale;
        return new Point((int)centerX, (int)centerY);
    }

}
