using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceGame
{
    public class RoomRow
    {

        public RoomColumn[] columns;
        public RoomRow()
        {
            columns = new RoomColumn[10];
            for (int x = 0; x <= columns.Length - 1; x++)
            {
                columns[x] = new RoomColumn();

            }
        }

        public RoomRow(int sizeX)
        {
            columns = new RoomColumn[sizeX];
            for (int x = 0; x <= columns.Length - 1; x++)
            {
                columns[x] = new RoomColumn();
            }
        }


    }
}
