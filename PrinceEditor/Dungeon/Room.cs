using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceEditor
{
    public class Room
    {

        public string roomName;
        public int roomIndex;
        public int roomZ;
        public int roomX;
        public int roomY;
        public bool startRoom;

        public Room(int id, string name, bool startroom)
            {
                roomIndex = id;
                roomName = name;
                startRoom = startroom;
            }

        public void AssignRoomPosition(int x, int y, int level)
            {
                roomX = x;
                roomY = y;
                roomZ = level;

            }

    }
}
