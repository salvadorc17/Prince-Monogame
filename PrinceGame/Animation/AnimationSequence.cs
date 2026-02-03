using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrinceGame
{
    public struct AnimationSequence
    {
        private bool firstTime;
        public Sequence sequence;
        public List<Sequence> lsequence;
        private float TotalElapsed;
        private int ElapsedTime;
        public string StateName;


        public static float frameRate = 0.09f;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStoppable
        {
            get
            {
                if (sequence != null)
                {
                    return Frames[m_frameIndex].stoppable;
                }
                return true;
            }
        }
        //set { stoppable = value; }

        public List<Frame> Frames
        {
            get { return sequence.frames; }
        }

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {

            get { return m_frameIndex; }
            set { m_frameIndex = value; }
        }

        private int m_frameIndex;



        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                

                if (sequence.frames[m_frameIndex].texture == null)
                {
                    return Vector2.Zero;
                }
                return new Vector2(sequence.frames[m_frameIndex].texture.Width / 2f, 0);
            }
        }


        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// i count ONLY sprite command
        /// </summary>
        public int FrameSpriteCount()
        {
            int frameSprite = 0;
            foreach (Frame f in this.Frames)
            {
                if (f.type == Enumeration.TypeFrame.SPRITE)
                {
                    frameSprite += 1;
                }
            }
            return frameSprite;
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(List<Sequence> lsequence, StateElement stateElement)
        {
            StatePlayerElement statePlayerElement = default(StatePlayerElement);
            StateTileElement stateTileElement = default(StateTileElement);
            string stateName = string.Empty;

            if (object.ReferenceEquals(typeof(StatePlayerElement), stateElement.GetType()))
            {
                statePlayerElement = (StatePlayerElement)stateElement;
                stateName = statePlayerElement.state.ToString();
                StateName = stateName;
            }
            else if (object.ReferenceEquals(typeof(StateTileElement), stateElement.GetType()))
            {
                stateTileElement = (StateTileElement)stateElement;
                stateName = stateTileElement.state.ToString();
                StateName = stateName;
            }
            else
            {
                stateName = stateElement.state.ToString();
                StateName = stateName;
            }

            // Start the new animation.
            if (stateElement.Priority == Enumeration.PriorityState.Normal & this.IsStoppable == false)
            {
                ElapsedTime = 0;
                return;
            }

            //Check if the animation is already playing
            if (sequence != null && sequence.name == stateName)
            {
                return;
            }



            this.lsequence = lsequence;
            //Search in the sequence the right type
            Sequence result = lsequence.Find((Sequence s) => s.name == stateName.ToUpper());

            if (result == null)
            {
                //will be an error 
                return;
            }

            //cloning for avoid reverse pemanently...
            sequence = result;

            if (stateElement.Stoppable != null)
            {
                foreach (Frame f in sequence.frames)
                {
                    f.stoppable = Convert.ToBoolean(stateElement.Stoppable);
                }
            }

            //For increase offset depend of the state previus; for example when running and fall the x offset will be increase.
            if (stateElement.OffSet != Vector2.Zero)
            {
                foreach (Frame f in sequence.frames)
                {
                    f.xOffSet = f.xOffSet + Convert.ToInt32(Math.Truncate(stateElement.OffSet.X));
                    f.yOffSet = f.yOffSet + Convert.ToInt32(Math.Truncate(stateElement.OffSet.Y));
                }
            }

            //Check if reverse movement and reverse order and sign x,y
            if (stateElement.Reverse == Enumeration.SequenceReverse.Reverse)
            {
                List<Frame> newListFrame = new List<Frame>();
                List<Frame> newListCommand = new List<Frame>();

                foreach (Frame f in sequence.frames)
                {
                    if (f.type == Enumeration.TypeFrame.COMMAND)
                    {
                        newListCommand.Add(f);
                    }
                    else
                    {
                        f.xOffSet = -1 * f.xOffSet;
                        f.yOffSet = -1 * f.yOffSet;
                        newListFrame.Add(f);
                    }
                }
                newListFrame.Reverse();
                //add command
                foreach (Frame f in newListCommand)
                {
                    newListFrame.Add(f);
                }

                sequence.frames = newListFrame;
            }


            if (stateElement.Reverse == Enumeration.SequenceReverse.FixFrame)
            {
                this.m_frameIndex = this.FrameSpriteCount() - this.m_frameIndex;
            }
            else
            {
                this.m_frameIndex = 0;
            }

            this.firstTime = true;
        }


        public void UpdateFrame(float elapsed, ref Position position, ref SpriteEffects spriteEffects__1, ref PlayerState playerState)
        {
            //Resetting Name
            float TimePerFrame = 0;
            playerState.Value().Name = string.Empty;
            //playerState.Value().Name = sequence.frames[frameIndex].name;
            //System.Console.WriteLine(playerState.Value().Name.ToUpper());


            TimePerFrame = frameRate + sequence.frames[m_frameIndex].delay;
            //0.1
            //TimePerFrame = 0.9f + sequence.frames[frameIndex].delay; //0.1
            //TimePerFrame = 1.2f + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {
                //Play Sound
                sequence.frames[m_frameIndex].PlaySound();


                m_frameIndex = Math.Min(m_frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;

                //Taking name of the frame usefull for hit combat..
                playerState.Value().Name = sequence.frames[m_frameIndex].name;


                if (sequence.frames[m_frameIndex].type != Enumeration.TypeFrame.SPRITE)
                {
                    //COMMAND
                    string[] aCommand = sequence.frames[m_frameIndex].name.Split('|');
                    string[] aParameter = sequence.frames[m_frameIndex].parameter.Split('|');
                    for (int x = 0; x <= aCommand.Length - 1; x++)
                    {
                        if (aCommand[x] == Enumeration.TypeCommand.ABOUTFACE.ToString())
                        {
                            if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                            {
                                spriteEffects__1 = SpriteEffects.None;
                            }
                            else
                            {
                                spriteEffects__1 = SpriteEffects.FlipHorizontally;
                            }
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOFRAME.ToString())
                        {
                            string par = aParameter[x];
                            int result = sequence.frames.FindIndex((Frame f) => f.name == par);
                            m_frameIndex = result;
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOSEQUENCE.ToString())
                        {
                            string[] par = aParameter[x].Split(':');
                            Sequence result = lsequence.Find((Sequence s) => s.name == par[0]);
                            sequence = result;
                            m_frameIndex = 0;

                            if (par.Length > 1)
                            {
                                Vector2 v = new Vector2(float.Parse(par[1].Split(',')[0]), float.Parse(par[1].Split(',')[1]));
                                playerState.Add(StatePlayerElement.Parse(par[0]), Enumeration.PriorityState.Normal, v);
                            }
                            else
                            {
                                playerState.Add(StatePlayerElement.Parse(par[0]));
                            }
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.IFGOTOSEQUENCE.ToString())
                        {
                            string par = string.Empty;
                            if (playerState.Value().IfTrue == true)
                            {
                                par = aParameter[0];
                            }
                            else
                            {
                                par = aParameter[1];
                            }

                            Sequence result = lsequence.Find((Sequence s) => s.name == par);
                            sequence = result;
                            m_frameIndex = 0;
                            playerState.Add(StatePlayerElement.Parse(par));
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.DELETE.ToString())
                        {
                            playerState.Add(Enumeration.State.delete, Enumeration.PriorityState.Force);

                        }
                    }
                }

                int flip = 0;
                if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                {
                    //& TotalElapsed > TimePerFrame) skip first frame =!==!=?!?bug
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }





                position.Value = new Vector2(position.X + (sequence.frames[m_frameIndex].xOffSet * flip), position.Y + sequence.frames[m_frameIndex].yOffSet);
            }
            else if (firstTime == true)
            {
                //Play Sound
                sequence.frames[m_frameIndex].PlaySound();

                int flip = 0;
                if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }

                position.Value = new Vector2(position.X + (sequence.frames[m_frameIndex].xOffSet * flip), position.Y + sequence.frames[m_frameIndex].yOffSet);


                firstTime = false;
            }
        }

        public void UpdateFrameTile(float elapsed, ref Position position, ref SpriteEffects spriteEffects__1, ref TileState tileState)
        {
            float TimePerFrame = 0;
            TimePerFrame = frameRate + sequence.frames[m_frameIndex].delay;
            //0.1
            //TimePerFrame = 0.9f + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {
                //Play Sound
                sequence.frames[m_frameIndex].PlaySound();


                m_frameIndex = Math.Min(m_frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;




                if (sequence.frames[m_frameIndex].type != Enumeration.TypeFrame.SPRITE)
                {
                    //COMMAND
                    string[] aCommand = sequence.frames[m_frameIndex].name.Split('|');
                    string[] aParameter = sequence.frames[m_frameIndex].parameter.Split('|');
                    for (int x = 0; x <= aCommand.Length - 1; x++)
                    {
                        if (aCommand[x] == Enumeration.TypeCommand.ABOUTFACE.ToString())
                        {
                            if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                            {
                                spriteEffects__1 = SpriteEffects.None;
                            }
                            else
                            {
                                spriteEffects__1 = SpriteEffects.FlipHorizontally;
                            }
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOFRAME.ToString())
                        {
                            string par = aParameter[x];
                            int result = sequence.frames.FindIndex((Frame f) => f.name == par);
                            m_frameIndex = result;
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOSEQUENCE.ToString())
                        {
                            string par = aParameter[x];
                            Sequence result = lsequence.Find((Sequence s) => s.name == par);
                            sequence = result;
                            m_frameIndex = 0;
                            tileState.Add(StateTileElement.Parse(par));
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.IFGOTOSEQUENCE.ToString())
                        {
                            string par = string.Empty;
                            if (tileState.Value().IfTrue == true)
                            {
                                par = aParameter[0];
                            }
                            else
                            {
                                par = aParameter[1];
                            }

                            Sequence result = lsequence.Find((Sequence s) => s.name == par);
                            sequence = result;
                            m_frameIndex = 0;
                            tileState.Add(StateTileElement.Parse(par));

                        }
                    }
                }

                int flip = 0;
                if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }

                position.Value = new Vector2(position.X + (sequence.frames[m_frameIndex].xOffSet * flip), position.Y + sequence.frames[m_frameIndex].yOffSet);
            }
            else if (firstTime == true)
            {
                int flip = 0;
                if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }

                position.Value = new Vector2(position.X + (sequence.frames[m_frameIndex].xOffSet * flip), position.Y + sequence.frames[m_frameIndex].yOffSet);
                firstTime = false;

                //Play Sound

                sequence.frames[m_frameIndex].PlaySound();
            }

        }


        public void UpdateFrameItem(float elapsed, ref Position position, ref SpriteEffects spriteEffects__1, ref TileState itemState)
        {
            float TimePerFrame = 0;
            TimePerFrame = frameRate + sequence.frames[m_frameIndex].delay;
            //0.1
            //TimePerFrame = 0.9f + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;


            if (TotalElapsed > TimePerFrame)
            {
                //Play Sound
                sequence.frames[m_frameIndex].PlaySound();


                m_frameIndex = Math.Min(m_frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;




                if (sequence.frames[m_frameIndex].type != Enumeration.TypeFrame.SPRITE)
                {
                    //COMMAND
                    string[] aCommand = sequence.frames[m_frameIndex].name.Split('|');
                    string[] aParameter = sequence.frames[m_frameIndex].parameter.Split('|');
                    for (int x = 0; x <= aCommand.Length - 1; x++)
                    {
                        if (aCommand[x] == Enumeration.TypeCommand.ABOUTFACE.ToString())
                        {
                            if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                            {
                                spriteEffects__1 = SpriteEffects.None;
                            }
                            else
                            {
                                spriteEffects__1 = SpriteEffects.FlipHorizontally;
                            }
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOFRAME.ToString())
                        {
                            string par = aParameter[x];
                            int result = sequence.frames.FindIndex((Frame f) => f.name == par);
                            m_frameIndex = result;
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOSEQUENCE.ToString())
                        {
                            string par = aParameter[x];
                            Sequence result = lsequence.Find((Sequence s) => s.name == par);
                            sequence = result;
                            m_frameIndex = 0;
                            itemState.Add(StateTileElement.Parse(par));
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.IFGOTOSEQUENCE.ToString())
                        {
                            string par = string.Empty;
                            if (itemState.Value().IfTrue == true)
                            {
                                par = aParameter[0];
                            }
                            else
                            {
                                par = aParameter[1];
                            }

                            Sequence result = lsequence.Find((Sequence s) => s.name == par);
                            sequence = result;
                            m_frameIndex = 0;
                            itemState.Add(StateTileElement.Parse(par));

                        }
                    }
                }

                int flip = 0;
                if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }

                position.Value = new Vector2(position.X + (sequence.frames[m_frameIndex].xOffSet * flip), position.Y + sequence.frames[m_frameIndex].yOffSet);
            }
            else if (firstTime == true)
            {
                //Play Sound
                sequence.frames[m_frameIndex].PlaySound();


                int flip = 0;
                if (spriteEffects__1 == SpriteEffects.FlipHorizontally)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }

                position.Value = new Vector2(position.X + (sequence.frames[m_frameIndex].xOffSet * flip), position.Y + sequence.frames[m_frameIndex].yOffSet);
                firstTime = false;
            }

        }



        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Texture2D texture)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);
        }


        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, sequence.frames[m_frameIndex].texture.Width, sequence.frames[m_frameIndex].texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[m_frameIndex].texture, position, source, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);
        }



        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Rectangle rectangleMask, Texture2D texture)
        {
            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[m_frameIndex].texture, position, rectangleMask, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);

        }

        public void DrawTileMask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Rectangle rectangleMask)
        {
            // Calculate the source rectangle of the current frame.
            //128-62
            //148-20
            //Rectangle source = new Rectangle(0, 128, 62, 20);
            //position.Y = position.Y + 128;
            //Rectangle source = new Rectangle(62, 20, sequence.frames[frameIndex].texture.Width, sequence.frames[frameIndex].texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[m_frameIndex].texture, position, rectangleMask, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);
        }


        public void DrawItem(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, sequence.frames[m_frameIndex].texture.Width, sequence.frames[m_frameIndex].texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[m_frameIndex].texture, position, source, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);
        }



        public void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth)
        {
            // Calculate the source rectangle of the current frame.

            //Texture2D value = (Texture2D)Maze.dContentRes[System.Configuration.ConfigurationManager.AppSettings[sequence.config_type].ToString().ToUpper() + sequence.frames[m_frameIndex].value.ToUpper()];
 
            Rectangle source = new Rectangle(0, 0, sequence.frames[m_frameIndex].texture.Height, sequence.frames[m_frameIndex].texture.Height);

            position = new Vector2(position.X + Tile.PERSPECTIVE, position.Y - Tile.GROUND);
            //position = new Vector2(position.X + Tile.PERSPECTIVE, position.Y );

            //Maze.dEffect.Parameters["FromColor"].SetValue(Color.White);
            //Maze.dEffect.Parameters["ToColor"].SetValue(Color.Red);

            //spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, Maze.dEffect);

            // Draw the current frame.
            spriteBatch.Draw(sequence.frames[m_frameIndex].texture, position, source, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);
                

        }


        public void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Texture2D texture)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);

            position = new Vector2(position.X + Tile.PERSPECTIVE + (texture.Width / 2), position.Y - Tile.GROUND + (texture.Height / 2));

            //Begin
            //spriteBatch.Begin();

            // Draw 
            spriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, depth);

            //spriteBatch.End();

        }


    }

}
