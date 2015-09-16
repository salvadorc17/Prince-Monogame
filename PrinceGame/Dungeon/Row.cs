using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceGame
{
    public class Row
    {

        public Column[] columns;
        public Row()
        {
            columns = new Column[10];
            for (int x = 0; x <= columns.Length - 1; x++)
            {
                columns[x] = new Column();

            }
        }

        public Row(int sizeX)
        {
            columns = new Column[sizeX];
            for (int x = 0; x <= columns.Length - 1; x++)
            {
                columns[x] = new Column();
            }
        }

    }

}
