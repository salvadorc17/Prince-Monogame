using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace PrinceGame
{
    public class Maze
    {
        // Level content.        
        public GraphicsDevice graphicsDevice;
        public ContentManager content;
        public List<Level> levels = new List<Level>();
        public List<RoomNew> rooms = new List<RoomNew>();

        public Player player;
        public List<Sprite> sprites = new List<Sprite>();

        public int levelindex;
        private RoomNew playerRoom;

        private static RoomNew blockRoom;

        //List for retain and load maze tiles textures
        public static Dictionary<string, object> dContentRes = null;
        //public static Dictionary<string, SoundEffect> dSoundEffect = null;

        public Vector2 positionArrive;
        //test
        public static Effect dEffect = null;

        public string Currentlevelname;



        private void PopNet()
	{
		DirectoryInfo files = new DirectoryInfo(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS);

		foreach (FileInfo f in files.GetFiles()) {
			//LOAD MXL CONTENT
			Stream txtReader = default(Stream);
			//#if ANDROID
			//txtReader = Game.Activity.Assets.Open(@PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + "LEVEL_dungeon_prison.xml");
			//#endif
			txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + f.Name);
			System.Xml.Serialization.XmlSerializer ax = null;
			ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));


			levels.Add((Level)ax.Deserialize(txtReader));




		}



		//Define and build a generic blockroom for usefull
		blockRoom = new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml", 1, 0);




		//load all room
		for (int z = 0; z <= levels.Count() - 1; z++) {
			//int newX = 1;
			for (int y = 0; y <= levels[z].rows.Count() - 1; y++) {
				for (int x = 0; x <= levels[z].rows[y].columns.Count() - 1; x++) {
					if (levels[z].rows[y].columns[x].FilePath == string.Empty) {
						levels[z].rows[y].columns[x].FilePath = "MAP_blockroom.xml";
					}

					RoomNew room = new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + levels[z].rows[y].columns[x].FilePath, levels[z].rows[y].columns[x].RoomIndex, levels[z].rows[y].columns[x].roomType);
					//RoomNew room = new RoomNew(this, "Maze/"+ levels[z].rows[y].columns[x].FilePath);
					room.roomStart = levels[z].rows[y].columns[x].RoomStart;
					room.roomName = levels[z].rows[y].columns[x].FilePath;
					room.roomZ = z;
					room.roomX = x;
					room.roomY = y;
					//newX++;
					rooms.Add(room);
				}
			}
		}
	}



        public Maze(GraphicsDevice GraphicsDevice__1, ContentManager contentmanager)
        {
            content = contentmanager;
            graphicsDevice = GraphicsDevice__1;
            levelindex = 1;
            LoadContent();

            //dEffect = content.Load<Effect>(@"Effects\SwapColor");
            PopNet();
        }

        public Dictionary<String, T> LoadContentRes<T>(ContentManager contentManager, string contentFolder)
       {
	      Dictionary<String, T> result = new Dictionary<String, T>();
	      string key = string.Empty;

	        //Load directory info, abort if none
	         DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
	           if (!dir.Exists) {
		           throw new DirectoryNotFoundException();
	         }




	        object fls = (from file in Directory.EnumerateFiles(contentManager.RootDirectory + "/" + contentFolder, "*.xnb", SearchOption.AllDirectories)
                                select file);

            var files = Directory
                .GetFiles(contentManager.RootDirectory, "*.*", SearchOption.AllDirectories)
            .Where(file => file.ToLower().EndsWith("xnb") || file.ToLower().EndsWith("fnt"))
            .ToList();



            foreach (var f in files)
            {
		     key = string.Empty;
		     string fileName = f.ToString().Replace('\\', '/');
		     int fileExtPos = fileName.LastIndexOf(".");
		    if (fileExtPos >= 0) {
			fileName = fileName.Substring(0, fileExtPos);
		    }
		           //remove contentManager.RootDirectory

		          fileExtPos = fileName.LastIndexOf(contentManager.RootDirectory) + (contentManager.RootDirectory + "/").Length;
		          if (fileExtPos >= 0) {
			      key = fileName.Substring(fileExtPos);
		          }



		          try {
			         //bug in the monogame load song, i will add the wav extension??!?!?
			         if (key.Contains("Songs/") == true) {
				       result[key.ToUpper()] = (T)(object)contentManager.Load<Song>(key + ".mp3");
			        } else {
				        result[key.ToUpper()] = contentManager.Load<T>(key);
			         }
		           } catch (Exception ex) {
			        //result[f.N.ToUpper()] = contentManager.Load<T>(sFolder + key); 
		         }
	            }

	          return result;
	     
        }

        //Load all texture in a dictiornary
        private void LoadContent()
        {
            //dSoundEffect = Program.LoadContent<SoundEffect>(content, string.Empty);
            dContentRes = LoadContentRes<object>(content, string.Empty);


        }

        public Level CurrentLevel()
        {

            foreach (Level l in levels)
            {
                Enumeration.LevelName levelName = (Enumeration.LevelName)Enum.Parse(typeof(Enumeration.LevelName), levelindex.ToString());
                if (l.levelIndex == levelindex)
                {
                    Currentlevelname = Enum.GetName(typeof(Enumeration.LevelName), l.levelIndex);

                    return l;

                }


            }



            return null;

        }


        public void NextLevel()
        {

            if (levelindex <= 0)
            {
                levelindex = 1;


            }
            else if (levelindex >= levels.Count)
            {
                levelindex = levels.Count;


            }
            else
            {

                foreach (Level l in levels)
                {
                    Enumeration.LevelName levelName = (Enumeration.LevelName)Enum.Parse(typeof(Enumeration.LevelName), levelindex.ToString());


                    if (l.levelIndex == levelindex)
                    {

                        player.StartLevel(FindRoom(l.startroom));
                    }
                }



                levelindex += 1;
                player.SpriteRoom.StartNewLife(graphicsDevice);

            }
        }


        public void PreviousLevel()
        {

            if (levelindex <= 0)
            {
                levelindex = 1;


            }
            else if (levelindex >= levels.Count)
            {
                levelindex = levels.Count;


            }
            else
            {

                foreach (Level l in levels)
                {
                    Enumeration.LevelName levelName = (Enumeration.LevelName)Enum.Parse(typeof(Enumeration.LevelName), levelindex.ToString());


                    if (l.levelIndex == levelindex)
                    {
                        player.StartLevel(FindRoom(l.startroom));
                    }
                }

                levelindex -= 1;
                player.SpriteRoom.StartNewLife(graphicsDevice);

            }

        }

        public List<Tile> GetTiles(Enumeration.TileType tileType)
        {
            List<Tile> list = new List<Tile>();
            foreach (RoomNew r in rooms)
            {
                foreach (Tile t in r.GetTiles(tileType))
                {
                    list.Add(t);
                    //list.Concat(r.GetTiles(tileType));
                }
            }
            return list;
        }

        public RoomNew LeftRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (x != 0)
            {
                x = System.Threading.Interlocked.Decrement(ref x);
            }
            else
            {
                return blockRoom;
            }
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                {
                    return r;
                }
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew DownRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
            {
                y = System.Threading.Interlocked.Increment(ref y);
            }
            else
            {
                return blockRoom;
            }
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                {
                    return r;
                }
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew RightRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (x != levels[z].rows[y].columns.Count() - 1)
            {
                x = System.Threading.Interlocked.Increment(ref x);
            }
            else
            {
                return blockRoom;
            }
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                {
                    return r;
                }
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew UpRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
            {
                y = System.Threading.Interlocked.Decrement(ref y);
            }
            else
            {
                return blockRoom;
            }
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                {
                    return r;
                }
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew NextRoom(RoomNew room)
        {
            int c = room.roomIndex;


            if (c != 0)
            {
                c = System.Threading.Interlocked.Increment(ref c);
            }


            foreach (RoomNew r in rooms)
            {

                if (r.roomIndex == c)
                {
                    return r;

                }

            }


            return blockRoom;
        }

        public RoomNew PreviuosRoom(RoomNew room)
        {
            int c = room.roomIndex;


            if (c != 0)
            {
                c = System.Threading.Interlocked.Decrement(ref c);

            }
            else if (c == 0)
            {
                c = System.Threading.Interlocked.Increment(ref c);



            }


            foreach (RoomNew r in rooms)
            {

                if (r.roomIndex == c)
                {
                    return r;

                }

            }


            return blockRoom;
        }

        public RoomNew RoomGet(RoomNew room)
        {

            int c = room.roomIndex;



            if (c == 0)
            {
                c = 1;

            }


            foreach (RoomNew r in rooms)
            {

                if (r.roomIndex == c)
                {
                    return r;

                }

            }


            return blockRoom;
        }



        public RoomNew FindRoom(int roomIndex)
	{
		foreach (RoomNew r in rooms) {
			if (r.roomIndex == roomIndex) {
				return r;
			}
		}
		return null;
	}

        public RoomNew StartRoom()
	{
        foreach (RoomNew r in rooms)
        {

			if (r.roomStart == true) {

				return r;
			}
		}
		return null;
	}


    }
}
