using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;

namespace PrinceGame
{
    public class PrinceOfPersiaGame : GameScreen
    {

        //private Texture2D[] playerTexture = new Texture2D[128];

        // Resources for drawing.
        //private GraphicsDeviceManager graphics;
        //private SpriteBatch spriteBatch;

        // Global content.
        //[ContentSerializerIgnore] 
        //List<Sequence> l ;
        //[ContentSerializerIgnore] 
        private SpriteFont hudFont;
        //[ContentSerializerIgnore]
        private SpriteFont PoPFont;
        //[ContentSerializerIgnore] 

        public static Texture2D player_livePoints;

        public static Texture2D player_energy;
        public static Texture2D enemy_livePoints;

        public static Texture2D enemy_energy;
        //public static Texture2D player_splash;
        //public static Texture2D enemy_splash;

        // Meta-level game state.
        private Level[] levels = new Level[30];
        //private int levelIndex = 0;
        private bool wasContinuePressed;

        private Maze maze;

        // When the time remaining is less than the warning time, it blinks on the hud

        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);
        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;

        private AccelerometerState accelerometerState;
        // The number of levels in the Levels directory of our content. We assume that
        // levels in our content are 0-based and that all numbers under this constant
        // have a level file present. This allows us to not need to check for the file
        // or handle exceptions, both of which can add unnecessary time to level loading.

        private const int numberOfLevels = 3;
        public static int CONFIG_SCREEN_WIDTH = 640;
        public static int CONFIG_SCREEN_HEIGHT = 400;
        public static bool CONFIG_DEBUG = false;
        public static float CONFIG_FRAMERATE = 0.09f;
        public static string CONFIG_SPRITE_KID;
        public static string CONFIG_SPRITE_GUARD;
        public static string CONFIG_SPRITE_SKELETON;
        public static string CONFIG_SPRITE_SERPENT;
        public static string CONFIG_SPRITE_EFFECTS;
        public static string CONFIG_SONGS;
        public static string CONFIG_SOUNDS;
        public static string[] CONFIG_TILES = new string[11];
        public static string CONFIG_ITEMS;
        private Texture2D[] layer = new Texture2D[6];

        private Timer Timer;
        public static string CONFIG_PATH_CONTENT = System.AppDomain.CurrentDomain.BaseDirectory + "Content" + Path.DirectorySeparatorChar;
        public static string CONFIG_PATH_APOPLEXY = "Apoplexy" + Path.DirectorySeparatorChar;
        public static string CONFIG_PATH_LEVELS = "Levels" + Path.DirectorySeparatorChar;
        public static string CONFIG_PATH_ROOMS = "Rooms" + Path.DirectorySeparatorChar;

        public static string CONFIG_PATH_SEQUENCES = "Sequences" + Path.DirectorySeparatorChar;

        public static int CONFIG_KID_START_ENERGY = 3;





        private ContentManager content;
        public SpriteBatch spriteBatch
        {
            get { return base.ScreenManager.SpriteBatch; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return base.ScreenManager.GraphicsDevice; }
        }






        public PrinceOfPersiaGame()
        {



            //READ APP.CONFIG for configuration settings
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["CONFIG_debug"].ToString(), out CONFIG_DEBUG);
            float.TryParse(System.Configuration.ConfigurationManager.AppSettings["CONFIG_framerate"].ToString(), out CONFIG_FRAMERATE);

            //READ CONTENT RESOURCES PATH
            CONFIG_SPRITE_KID = System.Configuration.ConfigurationManager.AppSettings["CONFIG_sprite_kid"].ToString();
            CONFIG_SPRITE_GUARD = System.Configuration.ConfigurationManager.AppSettings["CONFIG_sprite_guard"].ToString();
            CONFIG_SPRITE_SKELETON = System.Configuration.ConfigurationManager.AppSettings["CONFIG_sprite_skeleton"].ToString();
            CONFIG_SPRITE_SERPENT = System.Configuration.ConfigurationManager.AppSettings["CONFIG_sprite_serpent"].ToString();
            CONFIG_SOUNDS = System.Configuration.ConfigurationManager.AppSettings["CONFIG_sound"].ToString().ToUpper();
            CONFIG_SONGS = System.Configuration.ConfigurationManager.AppSettings["CONFIG_songs"].ToString().ToUpper();


            CONFIG_TILES[0] = ConfigurationManager.AppSettings["CONFIG_tiles"].ToString().ToUpper();

            CONFIG_TILES[1] = System.Configuration.ConfigurationManager.AppSettings["CONFIG_tiles2"].ToString().ToUpper();

            CONFIG_ITEMS = System.Configuration.ConfigurationManager.AppSettings["CONFIG_items"].ToString().ToUpper();
            CONFIG_PATH_SEQUENCES = System.Configuration.ConfigurationManager.AppSettings["CONFIG_path_Sequences"].ToString();
            CONFIG_SPRITE_EFFECTS = System.Configuration.ConfigurationManager.AppSettings["CONFIG_sprite_effects"].ToString().ToUpper();


            CONFIG_KID_START_ENERGY = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CONFIG_kid_start_energy"].ToString());

            CONFIG_PATH_CONTENT = "Content" + Path.DirectorySeparatorChar;


            Timer = new Timer();

            AnimationSequence.frameRate = CONFIG_FRAMERATE;


            Accelerometer.Initialize();
        }



        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                {
                    content = ScreenManager.content;
                }
                //content = new ContentManager(ScreenManager.Game.Services, CONFIG_PATH_CONTENT);

                LoadContent();

                //LOAD MAZE
                maze = new Maze(GraphicsDevice, content);
                //NOW START
                maze.StartRoom().StartNewLife(ScreenManager.GraphicsDevice);

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

        }


        public override void Deactivate()
        {
            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected void LoadContent()
        {
            // Load fonts
            hudFont = content.Load<SpriteFont>("Fonts/Hud");
            PoPFont = content.Load<SpriteFont>("Fonts/Pop");

            //energy...
            player_energy = content.Load<Texture2D>("Sprites/bottom/player_live_full");
            player_livePoints = content.Load<Texture2D>("Sprites/bottom/player_live_empty");
            enemy_energy = content.Load<Texture2D>("Sprites/bottom/enemy_live_full");
            enemy_livePoints = content.Load<Texture2D>("Sprites/bottom/enemy_live_empty");
            layer[0] = content.Load<Texture2D>("Backgrounds/0");
            layer[1] = content.Load<Texture2D>("Backgrounds/Layer");
            layer[2] = content.Load<Texture2D>("Backgrounds/Layer2");
            layer[3] = content.Load<Texture2D>("Backgrounds/Layer3");
            //Splash
            //player_splash = content.Load<Texture2D>("Sprites/bottom/player_splash");
            //enemy_splash = content.Load<Texture2D>("Sprites/bottom/enemy_splash");

            
            //try
            //{
            //    MediaPlayer.IsRepeating = true;
            //    MediaPlayer.Play(content.Load<Song>("Sounds/dos/main theme"));
            //}
            //catch { }







        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {



            HandleInput(gameTime, null);

            foreach (RoomNew r in maze.rooms)
            {
                r.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
            }
            maze.player.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);




            //Other sprites update

            foreach (Sprite s in maze.player.SpriteRoom.SpritesInRoom())
            {
                switch (s.GetType().Name)
                {
                    case "Guard":
                        if (true)
                        {
                            ((Guard)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                            break; 
                        }

                    case "Skeleton":
                        if (true)
                        {
                            ((Skeleton)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                            break;
                        }

                    case "Serpent":
                        if (true)
                        {
                            ((Serpent)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                            break; 
                        }

                    

                    case "Splash":
                        if (true)
                        {
                            ((Splash)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                            break; 
                        }
                        
                    default:

                        break; 

                }
                //delete object in state == delete
                if (s.spriteState.Value().state == Enumeration.State.delete)
                {
                    //s.sprite.FrameIndex = 0;
                    maze.sprites.Remove(s);
                }
            }

            if (Timer.isActive == false)
            {
                Timer.set(gameTime, 240);
            }
            else
            {
                Timer.checkTimer(gameTime);
            }



            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            //// get all of our input states
            keyboardState = Keyboard.GetState();
            //gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();

            if (maze.player.IsAlive == false)
            {
                if (keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || touchState.Any() == true)
                {
                    maze.StartRoom().StartNewLife(ScreenManager.GraphicsDevice);
                }
            }

            // Exit the game when back is pressed.
            ///// if (gamePadState.Buttons.Back == ButtonState.Pressed)
            ////// Exit();

            bool continuePressed = keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || touchState.Any();


            wasContinuePressed = continuePressed;
        }



        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
	{

		base.ScreenManager.SpriteBatch.Begin();

		//base.ScreenManager.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
		DrawBackground();

		maze.player.SpriteRoom.Draw(gameTime, spriteBatch);
		maze.player.Draw(gameTime, spriteBatch);


		//now drow sprites
		maze.player.SpriteRoom.DrawSprites(gameTime, spriteBatch);


		//now drow the mask
		maze.player.SpriteRoom.DrawMask(gameTime, spriteBatch);


		DrawBottom();
		DrawHud();


		foreach (RoomNew r in maze.rooms) 
			DrawDebug(r);



		//spriteBatch.End();

		base.Draw(gameTime);

		base.ScreenManager.SpriteBatch.End();
	}

        private void DrawBottom()
        {
            Vector2 hudLocation = new Vector2(0, CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);
            Vector2 textlocation = new Vector2(180, 250);
            //DRAW BLACK SQUARE
            Rectangle r = new Rectangle(0, 0, CONFIG_SCREEN_WIDTH, CONFIG_SCREEN_HEIGHT);
            Texture2D dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.Black });
            //Texture2D tx = new Texture2D(spriteBatch.GraphicsDevice, Game.CONFIG_SCREEN_WIDTH, Game.CONFIG_SCREEN_HEIGHT);
            //Texture2D tx = livePoints;
            spriteBatch.Draw(dummyTexture, hudLocation, r, Color.White);


            //check if death
            hudLocation = new Vector2(CONFIG_SCREEN_WIDTH / 3, CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);
            if (maze.player.IsAlive == false)
            {
                DrawShadowedString(PoPFont, "Press Space to Continue", textlocation, Color.White);
            }

            Rectangle source = new Rectangle(0, 0, player_livePoints.Width, player_livePoints.Height);

            int offset = 1;
            Texture2D player_triangle = player_livePoints;
            for (int x = 1; x <= maze.player.LivePoints; x++)
            {
                hudLocation = new Vector2(0 + offset, CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);
                // Calculate the source rectangle of the current frame.


                if (x <= maze.player.Energy)
                {
                    player_triangle = player_energy;
                }
                else
                {
                    player_triangle = player_livePoints;
                }

                // Draw the current tile.
                spriteBatch.Draw(player_triangle, hudLocation, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                offset += player_livePoints.Width + 1;
            }


            //Draw opponent energy
            offset = 1;
            foreach (Sprite s in maze.player.SpriteRoom.SpritesInRoom())
            {
                switch (s.GetType().Name)
                {
                    case "Guard":
                        if (true)
                        {
                            offset = enemy_livePoints.Width + 1;
                            Texture2D enemy_triangle = enemy_livePoints;
                            for (int x = 1; x <= maze.player.LivePoints; x++)
                            {
                                hudLocation = new Vector2(CONFIG_SCREEN_WIDTH - offset, CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);

                                if (x <= s.Energy)
                                {
                                    enemy_triangle = enemy_energy;
                                }
                                else
                                {
                                    enemy_triangle = enemy_livePoints;
                                }

                                // Draw the current tile.
                                spriteBatch.Draw(enemy_triangle, hudLocation, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                                offset += enemy_livePoints.Width + 1;
                            }
                            break; 
                        }

                    case "Skeleton":
                        if (true)
                        {
                            offset = enemy_livePoints.Width + 1;
                            Texture2D enemy_triangle = enemy_livePoints;
                            for (int x = 1; x <= maze.player.LivePoints; x++)
                            {
                                hudLocation = new Vector2(CONFIG_SCREEN_WIDTH - offset, CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);

                                if (x <= s.Energy)
                                {
                                    enemy_triangle = enemy_energy;
                                }
                                else
                                {
                                    enemy_triangle = enemy_livePoints;
                                }

                                // Draw the current tile.
                                spriteBatch.Draw(enemy_triangle, hudLocation, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                                offset += enemy_livePoints.Width + 1;
                            }
                            break;
                        }
                        
                    case "Serpent":
                        if (true)
                        {
                            offset = enemy_livePoints.Width + 1;
                            Texture2D enemy_triangle = enemy_livePoints;
                            for (int x = 1; x <= maze.player.LivePoints; x++)
                            {
                                hudLocation = new Vector2(CONFIG_SCREEN_WIDTH - offset, CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);

                                if (x <= s.Energy)
                                {
                                    enemy_triangle = enemy_energy;
                                }
                                else
                                {
                                    enemy_triangle = enemy_livePoints;
                                }

                                // Draw the current tile.
                                spriteBatch.Draw(enemy_triangle, hudLocation, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                                offset += enemy_livePoints.Width + 1;
                            }
                            break; 
                        }
                        
                    default:
                        break; 


                }
            }

            //DrawShadowedString(PoPFont, maze.PlayerRoom.roomName, hudLocation, Color.White);
        }

        private void DrawBackground()
        {
            Rectangle rectangle = new Rectangle(0, 0, 640, 480);


            if ((maze.CurrentLevel() != null))
            {
                switch (maze.CurrentLevel().levelIndex)
                {

                    case 1:

                        spriteBatch.Draw(layer[1], rectangle, Color.White);
                        break; 

                      
                    case 2:
                        spriteBatch.Draw(layer[2], rectangle, Color.White);
                        break; 

                       
                    case 3:

                        spriteBatch.Draw(layer[3], rectangle, Color.White);
                        break; 

                    default:
                        spriteBatch.Draw(layer[0], rectangle, Color.White);

                        break; 


                }

            }
            else
            {
                spriteBatch.Draw(layer[0], rectangle, Color.White);


            }



        }


        private void DrawDebug(RoomNew room)
        {
            //if (room.player.sprite.sequence == null)
            //    return;

            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            hudLocation.X = hudLocation.X + 180;
            hudLocation.Y = hudLocation.Y + 380;

            if (CONFIG_DEBUG == true)
            {
                
           
            var CurrentLvl = maze.CurrentLevel();

            if (CurrentLvl != null)
                DrawShadowedString(hudFont, "LEVEL NAME=" + CurrentLvl.levelIndex + "-" + CurrentLvl.levelName + "-" + maze.levelindex, hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;

            
            //DrawShadowedString(hudFont, "FRAME RATE=" + AnimationSequence.frameRate.ToString(), hudLocation, Color.White);

            //hudLocation.Y = hudLocation.Y + 10;

            if (maze.player != null)
                {

                    int CoordX = maze.player.RoomCoord.X;
                    int CoordY = maze.player.RoomCoord.Y;

                    DrawShadowedString(hudFont, "ROOM NAME=" + maze.player.SpriteRoom.roomName, hudLocation, Color.White);
                    hudLocation.Y = hudLocation.Y + 10;

                    DrawShadowedString(hudFont, "POSTION X=" + maze.player.Position.X + " Y=" + maze.player.Position.Y + " TILE= " + maze.player.SpriteRoom.GetTile(CoordX, CoordY).Type.ToString(), hudLocation, Color.White);
                    hudLocation.Y = hudLocation.Y + 10;
                
                Rectangle playerBound = maze.player.Position.Bounding;
                Rectangle tileBounds = maze.player.SpriteRoom.GetBounds(maze.player.RealPosition.X, maze.player.RealPosition.Y);
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBound, tileBounds);
                DrawShadowedString(hudFont, "PLAYER BOUNDS =" + "X = " + depth.X.ToString() + " Y = " + depth.Y.ToString(), hudLocation, Color.White);

                }

            hudLocation.Y = hudLocation.Y + 10;
            if (maze.player.sprite.sequence != null)
                DrawShadowedString(hudFont, "PLAYER STATE=" + Convert.ToString(maze.player.spriteState.Value().state) + " SEQUENCE CountOffset=" + Convert.ToString(maze.player.sprite.sequence.CountOffSet), hudLocation, Color.White);

            }


            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = maze.player.Position.Bounding;
            Vector4 v4 = room.getBoundTiles(playerBounds);


        }


        private void DrawHud()
        {
            //RoomNew room = maze.playerRoom;
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2f, titleSafeArea.Y + titleSafeArea.Height / 2f);

            // Draw score
            hudLocation.X = hudLocation.X + 60;
            hudLocation.Y = hudLocation.Y + 440;

            if (Timer.isActive == true)
            {
                DrawShadowedString(hudFont, "Game Time : " + Timer.displayValue, hudLocation, Color.White);
            }


        }



        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color__1)
        {
            if (value == null)
            {
                value = "MAP_blockroom.xml";
            }
            spriteBatch.DrawString(font, value, position + new Vector2(1f, 1f), Color.Black);
            spriteBatch.DrawString(font, value, position, color__1);
        }




        private DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

    }
}
