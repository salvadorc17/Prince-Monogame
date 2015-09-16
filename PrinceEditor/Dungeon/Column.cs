using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PrinceGame
{
    public class Column
{
	public Enumeration.TileType tileType = Enumeration.TileType.block;
	public Enumeration.SpriteType spriteType;
	public Enumeration.StateTile state = Enumeration.StateTile.normal;
	public int switchButton = 0;
	public float timeOpen = 0;

	public Enumeration.Items item = Enumeration.Items.nothing;

	public Column()
	{
	}
}
}
