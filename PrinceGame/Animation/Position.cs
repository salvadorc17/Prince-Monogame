using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    public class Position
    {
        public Vector2 _screenRealSize;
        public Vector2 _spriteRealSize;

        private Vector2 _vector2;


        public Position(Vector2 screenRealSize, Vector2 spriteRealSize)
        {
            _screenRealSize = screenRealSize;
            _spriteRealSize = spriteRealSize;
            _vector2 = Vector2.Zero;
        }

        /// <summary>
        /// The DrawValue is differtent coordinate position because the drawing routing
        /// draw sprite on the left upper corner and the real screen room screen begin to BOTTOM_BORDER
        /// 
        /// </summary>
        /// <returns></returns>

        //public Vector2 DrawValue()
        //{ return new Vector2(_vector2.X, _vector2.Y); }




        public Vector2 Value
        {
            get { return _vector2; }
            set { _vector2 = value; }
        }




        //mask X and Y values of Vector2 structure 
        public float X
        {
            get { return _vector2.X; }
            set { _vector2.X = value; }
        }


        public float Y
        {

            get { return _vector2.Y; }
            set { _vector2.Y = value; }
        }

        public bool CheckCollision(Position p)
        {
            if (p.Y == Y)
            {
                if (p.X + 10 > X & p.X - 10 < X)
                {
                    return true;
                }
            }
            return false;
        }


        public bool CheckOnRow(Position p)
        {
            if (p.Y == Y)
            {
                return true;
            }
            return false;

        }

        public float CheckOnRowDistancePixel(Position p)
        {
            if (p.Y == Y)
            {
                float distance = Math.Abs(p.X - X);
                //int ret = ((int)distance) / Tile.WIDTH;
                return distance;
            }
            return -1;

        }


        public float CheckOnRowDistance(Position p)
        {
            if (p.Y == Y)
            {
                float distance = Math.Abs(p.X - X);
                int ret = Convert.ToInt32(Math.Truncate(distance)) / Tile.WIDTH;
                return ret;
            }
            return -1;

        }

        //53 pix total
        public Rectangle Bounding
        {
            //return new Rectangle((int)_vector2.X + Player.PLAYER_STAND_BORDER_FRAME, (int)_vector2.Y, (int)Player.PLAYER_STAND_FRAME, (int)_spriteRealSize.Y);
            get { return new Rectangle(Convert.ToInt32(Math.Truncate(_vector2.X)), Convert.ToInt32(Math.Truncate(_vector2.Y)), Convert.ToInt32(Math.Truncate(_spriteRealSize.X)), Convert.ToInt32(Math.Truncate(_spriteRealSize.Y))); }
        }






        ////example to mask a method of Vector2 structure 
        public static float Dot(Vector2 value1, Vector2 value2)
        {
            return Vector2.Dot(value1, value2);
        }

        ////override the cast to Vector2
        //public static implicit operator Vector2(Position value) 
        //{
        //    return new Vector2(value.X, value.Y);
        //}

    }
}
