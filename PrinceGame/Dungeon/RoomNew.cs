using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceGame
{
    public class RoomNew
    {
        //: IDisposable

        public Maze maze;
        public Map map;
        //public Player player;

        public bool roomStart = false;
        //Coordinate system
        public string roomName;
        public int roomIndex;
        public int roomZ;
        public int roomX;
        public int roomY;

        public int roomType;
        // Physical structure of the level.
        private Tile[,] tiles;
        public Exit ExitTile;
        //private Tile[,] tilesMask;

        private const int pWidth = 10;
        private const int pHeight = 3;
        private const int pSize = 30;
        public const int BOTTOM_BORDER = 16;
        //Bottom border for live energy and message space
        public const int TOP_BORDER = 6;
        // = (400 pixel - BOTTOM_BORDER % 3 ROWS)
        public const int LEFT_LIMIT = -70 - 20;
        public const int RIGHT_LIMIT = 568 - 10;
        public const int TOP_LIMIT = -50;
        public static int BOTTOM_LIMIT = PrinceOfPersiaGame.CONFIG_SCREEN_HEIGHT - BOTTOM_BORDER - 50;
        //
        public int widthInLevel = 0;

        public int heightInLevel = 0;
        public List<Sprite> elements = new List<Sprite>();
        //private List<Tile> tilesTemporaney = new List<Tile>();
        //public ArrayList tilesTemporaney = ArrayList.Synchronized(_tilesTemporaney);

        //System.Collections.Concurrent. a = new System.Collections.Concurrent();

        public ArrayList tilesTemporaney = new ArrayList();
        //public ArrayList tilesTemporaney;

        // Key locations in the level.        

        public List<Sprite> SpritesInRoom()
        {
            List<Sprite> list = new List<Sprite>();

            foreach (Sprite s in maze.sprites)
            {
                if (object.ReferenceEquals(s.SpriteRoom, this))
                {
                    list.Add(s);
                }
            }


            if (maze.player.SpriteRoom.roomName == this.roomName)
            {
                list.Add(maze.player);
            }

            return list;
        }


        public ContentManager content
        {
            get { return maze.content; }
        }
        #region "Loading"

        public RoomNew(Maze maze, string filePath, int roomIndex, int roomType)
        {
            this.maze = maze;
            this.roomIndex = roomIndex;
            this.roomType = roomType;

            //tilesTemporaney = ArrayList.Synchronized(_tilesTemporaney);
            //LOAD MXL CONTENT
            //map = content.Load<Map>(filePath);

            //LOAD RES CONTENT
            System.Xml.Serialization.XmlSerializer ax = default(System.Xml.Serialization.XmlSerializer);

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(filePath);

            //TextReader txtReader = File.OpenText(filePath);


            //Stream astream = this.GetType().Assembly.GetManifestResourceStream(filePath);
            ax = new System.Xml.Serialization.XmlSerializer(typeof(Map));
            map = (Map)ax.Deserialize(txtReader);

            //LoadTilesMask();
            LoadTiles();
        }


        public void LooseShake()
        {
            List<Tile> l = (List<Tile>)GetTiles(Enumeration.TileType.loose);
            foreach (Loose item in l)
            {
                item.Shake();
            }
        }





        private void LoadTiles()
        {
            // Allocate the Tile grid.
            tiles = new Tile[map.rows[0].columns.Length, map.rows.Length];
            int x = 0;
            int y = 0;
            int newX = 0;

            foreach (Row r in map.rows)
            {
                for (int ix = 0; ix <= r.columns.Length - 1; ix++)
                {
                    Enumeration.TileType nextTileType = Enumeration.TileType.space;
                    if (ix + 1 < r.columns.Length)
                    {
                        nextTileType = r.columns[ix + 1].tileType;
                    }

                    tiles[x, y] = LoadTile(r.columns[ix].tileType, r.columns[ix].state, r.columns[ix].switchButton, r.columns[ix].item, nextTileType, r.columns[ix].timeOpen);
                    //tiles[x, y].tileAnimation.fra = maze.player.sprite.frameRate_background;
                    Rectangle rect = new Rectangle(x * Convert.ToInt32(Math.Truncate(Tile.Size.X)), y * Convert.ToInt32(Math.Truncate(Tile.Size.Y)) - BOTTOM_BORDER, Convert.ToInt32(tiles[x, y].Texture.Width), Convert.ToInt32(tiles[x, y].Texture.Height));
                    Vector2 v = new Vector2(rect.X, rect.Y);

                    tiles[x, y].Position = new Position(v, v);
                    tiles[x, y].Position.X = v.X;
                    tiles[x, y].Position.Y = v.Y;

                    //x+1 for avoid base zero x array, WALL POSITION 0-29
                    tiles[x, y].panelInfo = newX + roomIndex;

                    switch (r.columns[ix].spriteType)
                    {
                        case Enumeration.SpriteType.kid:
                            int xPlayer = (x - 1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                            int yPlayer = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + RoomNew.TOP_BORDER;
                            maze.player = new Player(this, new Vector2(xPlayer, yPlayer), new Point(x, y), maze.graphicsDevice, r.columns[ix].spriteEffect);
                            break; 

                        case Enumeration.SpriteType.guard:
                            int xGuard = (x - 1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                            //int yGuard = (y + 1) * (Tile.HEIGHT - Sprite.PLAYER_STAND_FLOOR_PEN - RoomNew.BOTTOM_BORDER + RoomNew.TOP_BORDER);
                            int yGuard = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + RoomNew.TOP_BORDER;
                            Guard g = new Guard(this, new Vector2(xGuard, yGuard), maze.graphicsDevice, r.columns[ix].spriteEffect);
                            maze.sprites.Add(g);
                            break;

                        case Enumeration.SpriteType.skeleton:
                            int xSkel = (x - 1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                            //int yGuard = (y + 1) * (Tile.HEIGHT - Sprite.PLAYER_STAND_FLOOR_PEN - RoomNew.BOTTOM_BORDER + RoomNew.TOP_BORDER);
                            int ySkel = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + RoomNew.TOP_BORDER;
                            Skeleton h = new Skeleton(this, new Vector2(xSkel, ySkel), maze.graphicsDevice, r.columns[ix].spriteEffect);
                            maze.sprites.Add(h);
                            break; 


                        case Enumeration.SpriteType.serpent:
                            int xSerp = (x - 1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                            //int yGuard = (y + 1) * (Tile.HEIGHT - Sprite.PLAYER_STAND_FLOOR_PEN - RoomNew.BOTTOM_BORDER + RoomNew.TOP_BORDER);
                            int ySerp = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + RoomNew.TOP_BORDER;
                            Serpent s = new Serpent(this, new Vector2(xSerp, ySerp), maze.graphicsDevice, r.columns[ix].spriteEffect);
                            maze.sprites.Add(s);
                            break; 

                        default:


                            break; 
                    }


                    x += 1;
                    newX += 1;
                }
                x = 0;
                y += 1;
            }
        }



        /// <summary>
        /// Creates a new Tile. The other Tile loading methods typically chain to this
        /// method after performing their special logic.
        /// </summary>
        /// <param name="name">
        /// Path to a Tile texture relative to the Content/Tiles directory.
        /// </param>
        /// <param name="collision">
        /// The Tile collision type for the new Tile.
        /// </param>
        /// <returns>The new Tile.</returns>
        private Tile LoadTile(Enumeration.TileType tiletype)
        {
            return new Tile(this, content, tiletype, Enumeration.StateTile.normal, Enumeration.Items.nothing, Enumeration.TileType.space);
        }

        private Tile LoadTile(Enumeration.TileType tiletype, Enumeration.StateTile state, int switchButton, Enumeration.Items item, Enumeration.TileType nextTileType, float timeOpen)
        {
            switch (tiletype)
            {
                case Enumeration.TileType.spikes:
                    return new Spikes(this, content, tiletype, state, nextTileType);
                    


                case Enumeration.TileType.lava:
                    return new Lava(this, content, tiletype, state, nextTileType);
                  


                case Enumeration.TileType.pressplate:
                    return new PressPlate(this, content, tiletype, state, switchButton, nextTileType);


                case Enumeration.TileType.closeplate:
                    return new ClosePlate(this, content, tiletype, state, switchButton, nextTileType);

                case Enumeration.TileType.gate:
                    return new Gate(this, content, tiletype, state, switchButton, nextTileType, timeOpen);

                case Enumeration.TileType.mirror:
                    return new Mirror(this, content, tiletype, state, switchButton, nextTileType, timeOpen);

                case Enumeration.TileType.loose:
                    return new Loose(this, content, tiletype, state, nextTileType);
                 


                case Enumeration.TileType.block:
                    return new Block(this, content, tiletype, state, nextTileType);
                     


                case Enumeration.TileType.chomper:
                    return new Chomper(this, content, tiletype, state, nextTileType);

                case Enumeration.TileType.exit:
                    return new Exit(this, content, tiletype, state, 0, nextTileType);

                   

                default:

                    return new Tile(this, content, tiletype, state, item, nextTileType);

                    
            }

        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            content.Unload();
        }

        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife(GraphicsDevice graphicsDevice)
        {
            //Play Sound presentation
            ((SoundEffect)Maze.dContentRes[PrinceOfPersiaGame.CONFIG_SOUNDS + "presentation".ToUpper()]).Play();
            maze.player.Reset(SpriteEffects.None);

            LoadTiles();

        }

        #endregion

        #region "Bounds and collision"
        /// <summary>
        /// Gets the collision mode of the Tile at a particular location.
        /// This method handles tiles outside of the levels boundries by making it
        /// impossible to escape past the left or right edges, but allowing things
        /// to jump beyond the top of the level and fall off the bottom.
        /// </summary>
        public Enumeration.TileCollision GetCollision(int x, int y)
        {
            if (x < 0)
            {
                if (y < 0)
                {
                    return maze.LeftRoom(this).tiles[Width - 1, Height - 1].collision;
                }
                else
                {
                    return maze.LeftRoom(this).tiles[Width - 1, y].collision;
                }
            }

            if (x >= Width)
            {
                if (y < 0)
                {
                    return maze.RightRoom(this).tiles[0, Height - 1].collision;
                }
                else
                {
                    return maze.RightRoom(this).tiles[0, y].collision;
                }
            }

            if (y >= Height)
            {
                return maze.DownRoom(this).tiles[x, 0].collision;
            }

            if (y < 0)
            {
                return maze.UpRoom(this).tiles[x, Height - 1 ].collision;
            }

            return tiles[x, y].collision;
        }

        public Enumeration.TileType GetType(int x, int y)
        {
            if (x < 0)
            {
                if (y < 0)
                {
                    return maze.LeftRoom(this).tiles[Width - 1, Height - 1].Type;
                }
                else
                {
                    return maze.LeftRoom(this).tiles[Width - 1,  y].Type;
                }
            }

            if (x >= Width)
            {
                if (y < 0)
                {
                    return maze.RightRoom(this).tiles[0, Height - 1].Type;
                }
                else
                {
                    return maze.RightRoom(this).tiles[0, y].Type;
                }
            }

            if (y >= Height)
            {
                return maze.DownRoom(this).tiles[x, 0].Type;
            }

            if (y < 0)
            {
                return maze.UpRoom(this).tiles[x, Height - 1].Type;
            }
            return tiles[x, y].Type;
        }


        public Tile GetTile(Vector2 playerPosition)
        {
            int x = Convert.ToInt32(Math.Truncate(Math.Floor(Convert.ToSingle(playerPosition.X) / Tile.WIDTH)));
            int y = Convert.ToInt32(Math.Truncate(Math.Floor((Convert.ToSingle(playerPosition.Y) / Tile.HEIGHT))));
            //int y = (int)Math.Floor(((float)(playerPosition.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT));

            return GetTile(x, y);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0)
            {
                return maze.LeftRoom(this).tiles[Width - 1,  y];
            }

            if (x >= Width)
            {
                return maze.RightRoom(this).tiles[0, y];
            }

            if (y >= Height)
            {
                return maze.DownRoom(this).tiles[x, 0];
            }

            if (y < 0)
            {
                return maze.UpRoom(this).tiles[x, Height - 1];
            }
            return tiles[x, y];
        }

        public Exit GetExit(int x, int y)
        {
            ExitTile = (Exit)tiles[x, y];

            if (x < 0)
            {
                ExitTile = (Exit)maze.LeftRoom(this).tiles[Width - 1, y];
                return ExitTile;
            }
 
            if (x >= Width)
            {
                ExitTile = (Exit)maze.RightRoom(this).tiles[0, y];
                return ExitTile;
            }

            if (y >= Height)
            {
                ExitTile = (Exit)maze.DownRoom(this).tiles[x, 0];
                return ExitTile;
            }

            if (y < 0)
            {
                ExitTile = (Exit)maze.UpRoom(this).tiles[x, Height - 1];
                return ExitTile;
            }

            
            return ExitTile;

        }

        public List<Tile> GetTiles(Enumeration.TileType tileType)
        {
            List<Tile> list = new List<Tile>();
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {
                    if (tileType == tiles[x, y].Type)
                    {
                        list.Add(tiles[x, y]);
                    }
                }
            }
            return list;
        }



        /// <summary>
        /// Gets the bounding rectangle of a Tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            int @by = 0;
            if (y == 0)
            {
                @by = TOP_BORDER;
            }
            else
            {
                @by = (y * Tile.HEIGHT) + TOP_BORDER;
            }

            return new Rectangle(x * Tile.WIDTH, @by, Tile.WIDTH, Tile.HEIGHT);
        }



        /// <summary>
        /// Width of level measured in tiles.
        /// </summary>
        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        #endregion

        #region "Update"

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            //THIS IS FOR NOT UPDATE THE BLOCK ROOM AND SAVE SOME CPU TIME....
            if (this.roomName == "MAP_blockroom.xml")
            {
                return;
            }
            //player.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            UpdateTilesTemporaney(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            UpdateTiles(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            //maze.LeftRoom(this).UpdateTilesLeft(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            //maze.UpRoom(this).UpdateTilesUp(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            UpdateItems(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            //update spritesssss
            //UpdateSprites(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

        }




        #endregion

        #region "Draw"

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesInverseNew(gameTime, spriteBatch);


        }

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void DrawMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesMask(gameTime, spriteBatch);


            if (roomType == 0)
            {

                DrawTilesBlocks(gameTime, spriteBatch);


            }
            else if (roomType == 1)
            {
                DrawTilesBlocks2(gameTime, spriteBatch);


            }



        }

        public void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Sprite s in SpritesInRoom())
            {
                switch (s.GetType().Name)
                {
                    case "Guard":
                        ((Guard)s).Draw(gameTime, spriteBatch);
                        break;

                    case "Skeleton":
                        ((Skeleton)s).Draw(gameTime, spriteBatch);
                        break; 

                    case "Serpent":
                        ((Serpent)s).Draw(gameTime, spriteBatch);
                        break;


                    case "Splash":
                        ((Splash)s).Draw(gameTime, spriteBatch);
                        break;

                    default:

                        break; 

                }
            }

        }


        private void UpdateTilesLeft(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = Width - 1; x <= Width - 1; x++)
                {
                    // If there is a visible Tile in that position
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Rectangle rect = new Rectangle(-1 * Convert.ToInt32(Math.Truncate(Tile.Size.X)), y * Convert.ToInt32(Math.Truncate(Tile.Size.Y)) - BOTTOM_BORDER, Convert.ToInt32(texture.Width), Convert.ToInt32(texture.Height));
                        Vector2 position = new Vector2(rect.X, rect.Y);
                        tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                    }
                }
            }
        }

        private void UpdateTiles(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {
                    tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                }
            }
        }

        private void UpdateItems(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {
                    if (tiles[x, y].item != null)
                    {
                        tiles[x, y].item.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                    }
                }
            }
        }


        private void UpdateSprites(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {

            foreach (Sprite s in SpritesInRoom())
            {
                switch (s.GetType().Name)
                {
                    case "Guard":
                        ((Guard)s).Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                        break;

                    case "Skeleton":
                        ((Skeleton)s).Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                        break;


                    case "Serpent":
                        ((Serpent)s).Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                        break; 


                }

            }

        }

        private void UpdateTilesTemporaney(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            try
            {
                //workaroud to thread unsafe...?
                lock (tilesTemporaney)
                {
                    foreach (Tile item in tilesTemporaney)
                    {
                        // Insert your code here.
                        item.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void UpdateTilesUp(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            int y = Height - 1;
            for (int x = 0; x <= Width - 1; x++)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    // Draw it in screen space.
                    Rectangle rect = new Rectangle(x * Convert.ToInt32(Math.Truncate(Tile.Size.X)), -1 * Convert.ToInt32(Math.Truncate(Tile.Size.Y)) - BOTTOM_BORDER, Convert.ToInt32(texture.Width), Convert.ToInt32(texture.Height));
                    Vector2 position = new Vector2(rect.X, rect.Y);
                    tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                }
            }
        }



        private void DrawTilesLeft(GameTime gametime, SpriteBatch spriteBatch)
        {
            // For each Tile position
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = Width - 1; x <= Width - 1; x++)
                {
                    // If there is a visible Tile in that position
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Rectangle rect = new Rectangle(-1 * Convert.ToInt32(Math.Truncate(Tile.Size.X)), y * Convert.ToInt32(Math.Truncate(Tile.Size.Y)) - BOTTOM_BORDER, Convert.ToInt32(texture.Width), Convert.ToInt32(texture.Height));
                        Vector2 position = new Vector2(rect.X, rect.Y);
                        tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                    }
                }
            }

        }

        private void DrawTilesUp(GameTime gametime, SpriteBatch spriteBatch)
        {
            // For each Tile position
            int y = Height - 1;
            for (int x = 0; x <= Width - 1; x++)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    Vector2 position = default(Vector2);
                    Rectangle rect = default(Rectangle);
                    // Draw it in screen space.
                    //if (tiles[x, y].Type == Enumeration.TileType.loose & tiles[x, y].tileState.Value().state == Enumeration.StateTile.loose)
                    //{
                    //    position.X = tiles[x, y].Position.X;
                    //    position.Y = tiles[x, y].Position.Y;
                    //}
                    //else
                    if (true)
                    {
                        rect = new Rectangle(x * Convert.ToInt32(Math.Truncate(Tile.Size.X)), -1 * Convert.ToInt32(Math.Truncate(Tile.Size.Y)) - BOTTOM_BORDER, Convert.ToInt32(texture.Width), Convert.ToInt32(texture.Height));
                        position = new Vector2(rect.X, rect.Y);
                    }

                    //tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1F)
                }
            }

        }


        private void DrawTilesDown(GameTime gametime, SpriteBatch spriteBatch)
        {
            // For each Tile position
            int y = 0;
            for (int x = 0; x <= Width - 1; x++)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    Vector2 position = default(Vector2);
                    Rectangle rect = default(Rectangle);
                    rect = new Rectangle(x * Convert.ToInt32(Math.Truncate(Tile.Size.X)), (Height - y) * Convert.ToInt32(Math.Truncate(Tile.Size.Y)) - BOTTOM_BORDER, Convert.ToInt32(texture.Width), Convert.ToInt32(texture.Height));
                    position = new Vector2(rect.X, rect.Y);

                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                }
            }

        }

        private void DrawTilesBlocks(GameTime gametime, SpriteBatch spriteBatch)
        {


            Texture2D texture = null;
            //Rectangle rectangleMask = new Rectangle();

            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {

                    if (tiles[x, y].Type == Enumeration.TileType.block)
                    {
                        if (x > 0 && (tiles[x - 1, y].Type == Enumeration.TileType.space | tiles[x - 1, y].Type == Enumeration.TileType.floor))
                        {
                            position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                            texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[0] + "Block_left");
                            tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                            //divider
                            position = new Vector2(tiles[x, y].Position.X + 16, tiles[x, y].Position.Y + 64);
                            texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[0] + "Block_divider2");

                            tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);
                        }

                        if (tiles[x, y].nextTileType == Enumeration.TileType.block)
                        {
                            for (int s = 0; s <= Block.seed_graystone.GetLength(1) - 1; s++)
                            {
                                if (Block.seed_graystone[0, s] == tiles[x, y].panelInfo)
                                {
                                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y + 21);
                                    texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[0] + "Block_random");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);
                                }
                                if (Block.seed_left_top[2, s] == ((Block)tiles[x, y]).panelInfo)
                                {
                                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                                    texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[0] + "Block_left");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                                    //divider
                                    position = new Vector2(tiles[x, y].Position.X + 22, tiles[x, y].Position.Y + 64);
                                    texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[0] + "Block_divider");

                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);


                                }



                            }
                        }
                    }
                }
            }

            maze.UpRoom(this).DrawTilesUp(gametime, spriteBatch);
        }


        private void DrawTilesBlocks2(GameTime gametime, SpriteBatch spriteBatch)
        {

            Texture2D texture = null;
            //Rectangle rectangleMask = new Rectangle();

            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {

                    if (tiles[x, y].Type == Enumeration.TileType.block)
                    {
                        if (x > 0 && (tiles[x - 1, y].Type == Enumeration.TileType.space | tiles[x - 1, y].Type == Enumeration.TileType.floor))
                        {
                            position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                            texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[1] + "Block_left");
                            tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                            //divider
                            position = new Vector2(tiles[x, y].Position.X + 16, tiles[x, y].Position.Y + 64);
                            texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[1] + "Block_divider2");

                            tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);
                        }

                        if (tiles[x, y].nextTileType == Enumeration.TileType.block)
                        {
                            for (int s = 0; s <= Block.seed_graystone.GetLength(1) - 1; s++)
                            {
                                if (Block.seed_graystone[0, s] == tiles[x, y].panelInfo)
                                {
                                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y + 21);
                                    texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[1] + "Block_random");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);
                                }
                                if (Block.seed_left_top[2, s] == ((Block)tiles[x, y]).panelInfo)
                                {
                                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                                    texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[1] + "Block_left");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                                    //divider
                                    position = new Vector2(tiles[x, y].Position.X + 22, tiles[x, y].Position.Y + 64);
                                    texture = maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[1] + "Block_divider");

                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);


                                }



                            }
                        }
                    }
                }
            }

            maze.UpRoom(this).DrawTilesUp(gametime, spriteBatch);
        }


        private void DrawTilesMask(GameTime gametime, SpriteBatch spriteBatch)
        {
            Rectangle rectangleMask = new Rectangle();
            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {
                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);

                    switch (tiles[x, y].Type)
                    {
                        case Enumeration.TileType.posts:
                            rectangleMask = Tile.MASK_POSTS;
                            break; 

                           
                        case Enumeration.TileType.gate:
                            position.X = position.X + 50;
                            rectangleMask = Tile.MASK_DOOR;
                            break;

                        case Enumeration.TileType.mirror:
                            position.X = position.X + 50;
                            rectangleMask = Tile.MASK_DOOR;
                            break; 
                            
                        case Enumeration.TileType.block:
                            rectangleMask = Tile.MASK_BLOCK;
                            break; 

                           
                        default:
                            position.Y = position.Y + 128;
                            rectangleMask = Tile.MASK_FLOOR;
                            break; 

                         
                    }

                    tiles[x, y].tileAnimation.DrawTileMask(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, rectangleMask);

                    if (tiles[x, y].item != null)
                    {
                        position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                        tiles[x, y].item.itemAnimation.DrawItem(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                    }
                }
            }

            maze.UpRoom(this).DrawTilesUp(gametime, spriteBatch);
        }



        private void DrawTilesInverseNew(GameTime gametime, SpriteBatch spriteBatch)
        {

            maze.LeftRoom(this).DrawTilesLeft(gametime, spriteBatch);
          


            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; y += -1)
            {
                for (int x = 0; x <= Width - 1; x++)
                {
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;

                    //Rectangle rect = new Rectangle(x * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                    //tiles[x, y].Position = new Position(new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y));
                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);



                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                }
            }
            //maze.RoomUp(this).DrawTilesUp(spriteBatch);
            maze.DownRoom(this).DrawTilesDown(gametime, spriteBatch);

            lock (tilesTemporaney)
            {
                foreach (Tile item in tilesTemporaney)
                {
                    Vector2 p = new Vector2(item.Position.X, item.Position.Y);
                    item.tileAnimation.DrawTile(gametime, spriteBatch, p, SpriteEffects.None, 0.1f);
                }
            }
        }


        public void SubsTile(Vector2 coordinate, Enumeration.TileType tileType)
        {
            int x = Convert.ToInt32(Math.Truncate(coordinate.X));
            // (int)Math.Floor((float)position.X / Tile.WIDTH);
            int y = Convert.ToInt32(Math.Truncate(coordinate.Y));
            //(int)Math.Ceiling(((float)(position.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT));
            Tile t = new Tile(this, content, tileType, Enumeration.StateTile.normal, Enumeration.Items.nothing, Enumeration.TileType.space);
            Position p = new Position(tiles[x, y].Position._screenRealSize, tiles[x, y].Position._spriteRealSize);
            p.X = tiles[x, y].Position.X;
            p.Y = tiles[x, y].Position.Y;
            t.Position = p;
            tiles[x, y] = t;
            tiles[x, y].tileAnimation.PlayAnimation(t.TileSequence, t.tileState.Value());
        }

        public void SubsTileState(Vector2 position, Enumeration.StateTile state)
        {
            int x = Convert.ToInt32(Math.Truncate(position.X));
            int y = Convert.ToInt32(Math.Truncate(position.Y));

            tiles[x, y].tileState.Value().state = state;
            tiles[x, y].tileAnimation.PlayAnimation(tiles[x, y].TileSequence, tiles[x, y].tileState.Value());
        }


        public Vector4 getBoundTiles(Rectangle playerBounds)
        {
            int leftTile = Convert.ToInt32(Math.Truncate(Math.Floor(Convert.ToSingle(playerBounds.Left) / Tile.WIDTH)));
            int rightTile = Convert.ToInt32(Math.Truncate(Math.Ceiling((Convert.ToSingle(playerBounds.Right) / Tile.WIDTH)))) - 1;
            //tile dal bordo sx dello schermo al bordo dx del rettangolo sprite
            int topTile = Convert.ToInt32(Math.Truncate(Math.Floor(Convert.ToSingle(playerBounds.Top - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT)));
            //tiles from the top screen border
            int bottomTile = Convert.ToInt32(Math.Truncate(Math.Ceiling((Convert.ToSingle(playerBounds.Bottom - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT)))) - 1;

            //if (topTile < 0)
            //    topTile = 0;
            if (bottomTile > RoomNew.pHeight - 1)
            {
                bottomTile = topTile;
            }


            return new Vector4(leftTile, rightTile, topTile, bottomTile);
        }

        public Vector2 getCenterTile(Rectangle playerBounds)
        {
            int leftTile = Convert.ToInt32(Math.Truncate(Math.Floor(Convert.ToSingle(playerBounds.Center.X) / Tile.WIDTH)));
            int topTile = Convert.ToInt32(Math.Truncate(Math.Floor(Convert.ToSingle(playerBounds.Center.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT)));
            //tiles from the top screen border
            return new Vector2(leftTile, topTile);
        }

        #endregion
    }


}
