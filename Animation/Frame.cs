using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PrinceGame
{
    public class Frame
    {
        public string name;
        public string sound;
        public bool soundLoop = false;
        public bool soundFirstTime = false;
        //for determine if already sounded for single loop purpose
        public string value;
        public Enumeration.TypeFrame type = Enumeration.TypeFrame.SPRITE;

        public string parameter;
        //public SoundEffect soundEffect = new SoundEffect();
        public bool stoppable = false;
        public int Width, Height;
        public int xOffSet = 0;
        public int yOffSet = 0;
        public bool raised = false;
        //for check if the frame is a jump, hang raised in air
        public float delay = 0;
        //delay animation frame

        public void PlaySound()
        {
            //Play Sound
            if (sound != null && soundeffect != null)
            {
                if (soundLoop == false)
                {
                    if (soundFirstTime == false)
                    {
                        soundeffect.Play();
                        soundFirstTime = true;
                    }
                }
                else
                {
                    soundeffect.Play();
                }
            }
        }

        public SoundEffect soundeffect
        {
            get { return osoundeffect; }
        }

        private SoundEffect osoundeffect;
        public Texture2D texture
        {
            get { return otexture; }
        }

        private Texture2D otexture;

        public void SetSound(SoundEffect value)
        {
            osoundeffect = value;
        }

        public void SetTexture(Texture2D value)
        {
            otexture = value;
        }

        public Frame()
        {
        }

        //public enum TypeFrame
        //{
        //    SPRITE,
        //    COMMAND,
        //}

        public Frame DeepCopy()
        {
            return (Frame)this.MemberwiseClone();
        }

    }

}
