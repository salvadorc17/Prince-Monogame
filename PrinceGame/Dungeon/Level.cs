using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PrinceGame
{
    public class Level
{
	public RoomRow[] rows;
	public string levelName;
	public int levelIndex;

	public int startroom;
	public Level()
	{
		rows = new RoomRow[10];

		for (int x = 0; x <= rows.Count() - 1; x++) {
			rows[x] = new RoomRow(10);
		}




	}

	public Level(int sizeRow, int sizeColumn)
	{
		rows = new RoomRow[sizeRow];

		for (int x = 0; x <= rows.Count() - 1; x++) {
			rows[x] = new RoomRow(sizeColumn);

		}
	}


	public void Serialize()
	{
		System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
		TextWriter writer = new StreamWriter("C:\\temp\\LEVEL_" + levelName.ToString() + ".xml");
		ax.Serialize(writer, this);
	}

}
}
