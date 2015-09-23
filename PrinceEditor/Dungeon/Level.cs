using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PrinceEditor;

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

    public Room DownRoom(List<Room> rooms, Room current, Level level)
    {
        int x = current.roomX;
        int y = current.roomY;
        int z = current.roomZ;

        if (y != level.rows.Count() - 1)
        {
            y = System.Threading.Interlocked.Increment(ref y);
        }


        foreach (Room r in rooms)
        {
            if (r.roomX == x & r.roomY == y & r.roomZ == z)
            {
                return r;
            }
        }
        return current;

    }

    public Room UpRoom(List<Room> rooms, Room current, Level level)
    {
        int x = current.roomX;
        int y = current.roomY;
        int z = current.roomZ;

        if (y != level.rows.Count() - 1)
        {
            y = System.Threading.Interlocked.Decrement(ref y);
        }


        foreach (Room r in rooms)
        {
            if (r.roomX == x & r.roomY == y & r.roomZ == z)
            {
                return r;
            }
        }
        return current;

    }

    public Room RightRoom(List<Room> rooms, Room current, Level level)
    {
        int x = current.roomX;
        int y = current.roomY;
        int z = current.roomZ;

        if (x != level.rows[y].columns.Count() - 1)
        {
            x = System.Threading.Interlocked.Increment(ref x);
        }


        foreach (Room r in rooms)
        {
            if (r.roomX == x & r.roomY == y & r.roomZ == z)
            {
                return r;
            }
        }
        return current;

    }

    public Room LeftRoom(List<Room> rooms, Room current, Level level)
    {
        int x = current.roomX;
        int y = current.roomY;
        int z = current.roomZ;

        if (x != level.rows[y].columns.Count() - 1)
        {
            x = System.Threading.Interlocked.Decrement(ref x);
        }


        foreach (Room r in rooms)
        {
            if (r.roomX == x & r.roomY == y & r.roomZ == z)
            {
                return r;
            }
        }
        return current;

    }

    public Room StartRoom(List<Room> rooms, Room current, Level level)
    {

        foreach (Room r in rooms)
        {
            if (r.startRoom == true)
            {
                return r;
            }
        }

        return current;

    }

	public void Serialize()
	{
		System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
		TextWriter writer = new StreamWriter("C:\\temp\\LEVEL_" + levelName.ToString() + ".xml");
		ax.Serialize(writer, this);
	}

}
}
