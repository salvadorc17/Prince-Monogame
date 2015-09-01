using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PrinceGame
{
    public class Sprite
    {

        private bool m_sword = false;
        public const int SPRITE_SIZE_X = 114;
        //to be var
        public const int SPRITE_SIZE_Y = 114;
        //to be var
        public const int PLAYER_L_PENETRATION = 19;

        public const int PLAYER_R_PENETRATION = 30;
        public const int PLAYER_STAND_BORDER_FRAME = 47;
        //47+20player+47=114
        public const int PLAYER_STAND_FRAME = 20;
        public const int PLAYER_STAND_FEET = 4;
        public const int PLAYER_STAND_WALL_PEN = 30;
        //wall penetration
        public const int PLAYER_STAND_FLOOR_PEN = 26;
        //floor border penetration
        public const int PLAYER_STAND_HANG_PEN = 46;
        //floor border penetration for hangup
        public Vector2 startPosition = Vector2.Zero;
        //where the sprite begins

        public SpriteEffects startFlip = SpriteEffects.None;

        protected List<Sequence> spriteSequence = null;
        protected GraphicsDevice graphicsDevice;
        public SpriteEffects flip = SpriteEffects.None;
        protected Position _position = null;
        //protected Maze _maze;
        protected Vector2 m_velocity;
        protected bool m_isOnGround;
        protected Rectangle localBounds;
        public Point RealPosition;
        protected RoomNew m_spriteRoom = null;
        private bool m_alert = false;
        //set true when there is a enemy in the room or near
        public AnimationSequence sprite;
        public PlayerState spriteState = new PlayerState();
        public SpriteEffects startFace = SpriteEffects.None;

        public SpriteEffects face = SpriteEffects.None;
        public bool Sword
        {
            get { return m_sword; }
            set { m_sword = value; }
        }

        public Maze Maze
        {
            get { return m_spriteRoom.maze; }
        }

        // Alert state
        public bool Alert
        {
            get { return m_alert; }
            set { m_alert = value; }
        }

        // Physics state
        public RoomNew SpriteRoom
        {
            get { return m_spriteRoom; }
            set { m_spriteRoom = value; }
        }

        // Physics state, used by calculate falldrop distance
        public Vector2 PositionFall
        {
            get { return m_positionFall; }
            set { m_positionFall = value; }
        }

        protected Vector2 m_positionFall = Vector2.Zero;

        public bool IsAlive
        {
            get
            {
                if (m_energy > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //set { isAlive = value; }


        //How many energy triangle sprite have
        public int LivePoints
        {
            get { return m_livePoints; }
            set { m_livePoints = value; }
        }

        private int m_livePoints = 3;
        //energy...
        public int Energy
        {
            get { return m_energy; }
            set
            {
                m_energy = value;
                if (m_energy > m_livePoints)
                {
                    m_energy = m_livePoints;
                }
            }
        }

        private int m_energy = 3;

        public Position Position
        {
            get { return _position; }
        }

        public Vector2 Velocity
        {
            get { return m_velocity; }
            set { m_velocity = value; }
        }


        public bool IsOnGround
        {
            get { return m_isOnGround; }
        }
        /// <summary>
        /// Gets a rectangle which bounds this player in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = Convert.ToInt32(Math.Truncate(Math.Round(_position.X))) + localBounds.X;
                int top = Convert.ToInt32(Math.Truncate(Math.Round(_position.Y))) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public Rectangle BoundingRectangleReal
        {
            get
            {
                int left = Convert.ToInt32(Math.Truncate(Math.Round(_position.X))) - (localBounds.Width);
                //square 114x114
                int top = Convert.ToInt32(Math.Truncate(Math.Round(_position.Y))) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width * 2, localBounds.Height);
            }
        }


        public bool CheckCollision(Position position__1)
        {
            if (object.ReferenceEquals(position__1, Position))
            {
                return true;
            }
            return false;
        }

        public bool CheckOnRow(Position position__1)
        {
            if (position__1.Y == Position.Y)
            {
                return true;
            }
            return false;
        }


        public void isGround()
        {
            if (IsAlive == false)
            {
                return;
            }

            m_isOnGround = false;

            RoomNew room = null;
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = SpriteRoom.getCenterTile(playerBounds);
            Rectangle tileBounds = SpriteRoom.GetBounds(Convert.ToInt32(Math.Truncate(v2.X)), Convert.ToInt32(Math.Truncate(v2.Y)));

            //Check if kid outside Room 
            if (v2.X < 0)
            {
                room = Maze.LeftRoom(SpriteRoom);
            }
            else
            {
                room = SpriteRoom;
            }

            if (v2.Y > 2)
            {
                m_isOnGround = false;
            }
            else if (v2.Y < 0)
            {
                m_isOnGround = false;
            }
            else
            {
                if (room.GetCollision(Convert.ToInt32(Math.Truncate(v2.X)), Convert.ToInt32(Math.Truncate(v2.Y))) != Enumeration.TileCollision.Passable)
                {
                    if (playerBounds.Bottom >= tileBounds.Bottom)
                    {
                        m_isOnGround = true;
                    }
                }
            }


            if (m_isOnGround == false)
            {
                if (sprite.sequence.raised == false)
                {
                    if (spriteState.Value().state == Enumeration.State.runjump)
                    {
                        spriteState.Add(Enumeration.State.rjumpfall, Enumeration.PriorityState.Force);
                        sprite.PlayAnimation(spriteSequence, spriteState.Value());
                    }
                    else
                    {
                        if (spriteState.Previous().state == Enumeration.State.runjump)
                        {
                            spriteState.Add(Enumeration.State.stepfall, Enumeration.PriorityState.Force, new Vector2(20, 15));
                        }
                        else if (spriteState.Value().state != Enumeration.State.freefall)
                        {
                            spriteState.Add(Enumeration.State.stepfall, Enumeration.PriorityState.Force);
                        }
                    }
                    //SpriteRoom.LooseShake();
                    //and for upper room...
                    SpriteRoom.maze.UpRoom(SpriteRoom).LooseShake();
                }
                return;
            }


            //IS ON GROUND!
            if (spriteState.Value().state == Enumeration.State.freefall)
            {
                //Align to tile x
                _position.Y = tileBounds.Bottom - _position._spriteRealSize.Y;
                //CHECK IF LOOSE ENERGY...
                int Rem = 0;
                Rem = Convert.ToInt32(Math.Truncate(Math.Abs(Position.Y - PositionFall.Y))) / Tile.REALHEIGHT;

                if (Rem == 0)
                {
                    ((SoundEffect)Maze.dContentRes["Sounds/dos/falling echo".ToUpper()]).Play();
                }
                else if (Rem >= 1 & Rem < 3)
                {
                    ((SoundEffect)Maze.dContentRes["Sounds/dos/loosing a life falling".ToUpper()]).Play();
                }
                else
                {
                    ((SoundEffect)Maze.dContentRes["Sounds/dos/falling".ToUpper()]).Play();
                    //you should dead!!!
                    DeadFall();
                }
                Energy = Energy - Rem;
                spriteState.Add(Enumeration.State.crouch, Enumeration.PriorityState.Force, false);
                SpriteRoom.LooseShake();
            }

        }

        public void DeadFall()
        {
            spriteState.Add(Enumeration.State.deadfall, Enumeration.PriorityState.Force);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void DropDead()
        {
            if (spriteState.Value().state != Enumeration.State.dropdead)
            {
                spriteState.Add(Enumeration.State.dropdead, Enumeration.PriorityState.Force);
                sprite.PlayAnimation(spriteSequence, spriteState.Value());
            }
        }

        public void Resheathe()
        {
            spriteState.Add(Enumeration.State.resheathe, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        public void Fastheathe()
        {
            spriteState.Add(Enumeration.State.fastsheathe, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public virtual void Strike()
        {
            spriteState.Add(Enumeration.State.strike, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public virtual void Retreat()
        {
            spriteState.Add(Enumeration.State.retreat, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void StrikeRetreat()
        {
            spriteState.Add(Enumeration.State.strikeret, Enumeration.PriorityState.Force);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        //show splash hit

        public void Splash(bool player, GameTime gametime)
        {
            Splash splash__1 = new Splash(Maze.player.SpriteRoom, Position.Value, graphicsDevice, SpriteEffects.None, player);
            Maze.sprites.Add(splash__1);

        }



        public Sprite()
        {
        }
    }
}
