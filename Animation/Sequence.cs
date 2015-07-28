using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PrinceGame
{
    public class Sequence : ICloneable
    {
        private const int FRAME_WIDTH = 114;
        private const int FRAME_HEIGHT = 114;
        //public const float FRAME_TIME = 0.1f;


        public string config_type;
        public List<Frame> frames = new List<Frame>();
        public string name, path;
        public bool raised = false;
        public Enumeration.TileType tileType;

        public Enumeration.TileCollision collision;
        /// <summary>
        /// Get the Total xOffset and yOffset
        /// 
        /// </summary>
        /// <retur>
        /// Vector2
        /// </retur>
        public Vector2 CountOffSet
        {
            get
            {
                if (this == null)
                {
                    return Vector2.Zero;
                }
                int x = 0;
                int y = 0;
                foreach (Frame f in frames)
                {
                    x += f.xOffSet;
                    y += f.yOffSet;
                }
                return new Vector2(Convert.ToInt32(x), Convert.ToInt32(y));
            }
        }



        public void Initialize(ContentManager Content)
        {
            foreach (Frame f in frames)
            {
                try
                {
                    //loading texture
                    if (f.value != null)
                    {
                         
                       path = System.Configuration.ConfigurationManager.AppSettings[config_type].ToString().ToUpper();
                       Texture2D t = (Texture2D)Maze.dContentRes[System.Configuration.ConfigurationManager.AppSettings[config_type].ToString().ToUpper() + f.value.ToUpper()];


                       if (t == null)
                       {
                           f.SetTexture(Content.Load<Texture2D>(path + f.value));
                       }
                        else
                        {
                            f.SetTexture(t);
                        }
                    }
                    //loading sound
                    if (f.sound != null)
                    {
                        SoundEffect s = (SoundEffect)Maze.dContentRes[PrinceOfPersiaGame.CONFIG_SOUNDS + f.sound.ToUpper()];
                        f.SetSound(s);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("ERROR:Content.Load<dContentRes>" + ex.ToString() + config_type.ToUpper() + f.value);
                }
            }
        }

        // Deep clone
        public Sequence DeepClone()
        {
            Sequence newSequence = new Sequence();
            newSequence.name = this.name;
            newSequence.raised = this.raised;
            newSequence.collision = this.collision;
            newSequence.tileType = this.tileType;
            newSequence.config_type = this.config_type;

            //newSequence.frameTime = this.frameTime;
            foreach (Frame f in this.frames)
            {
                newSequence.frames.Add(f.DeepCopy());
            }
            return newSequence;

        }


        private object ICloneable_Clone()
        {
            return this.Clone();
        }
        object ICloneable.Clone()
        {
            return ICloneable_Clone();
        }
        public Sequence Clone()
        {
            return (Sequence)this.MemberwiseClone();
        }



    }


}
