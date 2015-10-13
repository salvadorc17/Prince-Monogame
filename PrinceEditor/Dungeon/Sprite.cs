using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PrinceGame
{
   public class Sprite
    {

       public int Id;
       public string Name;
       public Enumeration.SpriteType Type;

       public int X, Y;
       public Rectangle Bounds;

       public Sprite(int id, Enumeration.SpriteType type, int x, int y)
       {
           Id = id;
           Type = type;
           Name = type.ToString();
           X = x;
           Y = y;
           Bounds = new Rectangle(x, y, 64, 74);

       }
    }
}
