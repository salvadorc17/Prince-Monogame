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
       public bool Flip, Enabled;
       public int X, Y, Width, Height;
       public Rectangle Bounds;

       public Sprite(int id, Enumeration.SpriteType type, int x, int y)
       {
           Id = id;
           Type = type;
           Name = type.ToString();
           X = x;
           Y = y;
           Width = 64;
           Height = 74;
           Bounds = new Rectangle(x, y, Width, Height);
           Flip = false;
           Enabled = false;

       }
    }
}
