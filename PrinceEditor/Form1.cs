﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using PrinceGame;
using System.Xml.Serialization;

namespace PrinceEditor
{
    public partial class Form1 : Form
    {
        private Stream reader;
        private string line;
        private string[,] levelmap = new string[10, 10];
        private string[,] block = new string[10, 10];
        private string[,] sprite = new string[10, 10];
        private string[,] item = new string[10, 10];
        private string[,] switche = new string[10, 10];
        private XDocument xml;
        private StreamWriter xmlwriter;
        public int rowcount, columncount, roomcount;
        private int selectedtile;
        private int selectedsprite;
        private int selecteditem;
        private Graphics graphics, BBG;
        private Rectangle srect, drect, playerrec;
        private Bitmap bb;
        private DirectoryInfo directory;
        private Rectangle[] guardrect = new Rectangle[31];
        private int guards;
        private int playerx;
        private int playery;
        private int[] guardx = new int[30];
        private int[] guardy = new int[30];
        private Map map;
        public List<Room> rooms;
        private Room currentroom;
        private Level level;
        public static int mouseX;
        public static int mouseY;
        public static int mMapX;
        public static int mMapY;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Show();
            this.Focus();

            graphics = this.pictureBox1.CreateGraphics();
            bb = new Bitmap(640, 222);

            GenerateNewMap();

            LoadFiles();

            if (graphics != null)
                timer1.Enabled = true;

        }

        private void GenerateNewMap()
        {

            for (int y = 0; y <= 2; y++)
            {

                for (int x = 0; x <= 9; x++)
                {
                    block[x, y] = "space";
                    sprite[x, y] = "nothing";
                    item[x, y] = "none";
                    switche[x, y] = "normal";

                }

            }

            selectedtile = 0;
            selectedsprite = 0;
            selecteditem = 0;


        }

        private void LoadFiles()
        {
	          listBox1.Items.Clear();


	          directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Content\\Levels");

              label2.Text = directory.GetFiles().Count().ToString();

	         foreach (FileInfo file in directory.GetFiles()) {
		        if (file.Extension == ".xml")
                    listBox1.Items.Add(file.Name);

		

	     }


        }

        private void LoadLevel(string path)
        {

            rowcount = 0;
            columncount = 0;
            roomcount = 0;

            rooms = new List<Room>();


            System.Xml.Serialization.XmlSerializer ax = default(System.Xml.Serialization.XmlSerializer);

            Stream txtReader = File.Open(path, FileMode.Open);

            //TextReader txtReader = File.OpenText(filePath);

            ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            level = (Level)ax.Deserialize(txtReader);

            

            for (int r = 0; r <= level.rows.Count() - 1; r++)
            {
                if (level.rows[r] != null)
                    rowcount += 1;


                for (int i = 0; i <= level.rows[r].columns.Length - 1; i++)
                {
                    if (level.rows[r].columns[i] != null)
                        columncount += 1;

                    if (level.rows[r].columns[i].RoomIndex != 0)
                         {
                        
                        roomcount += 1;
                        rooms.Add(new Room(level.rows[r].columns[i].RoomIndex, level.rows[r].columns[i].FilePath, 
                            level.rows[r].columns[i].RoomStart));
                        rooms[roomcount - 1].AssignRoomPosition(i, r, 0);
                        if (level.rows[r].columns[i].RoomStart == true)
                            currentroom = rooms[roomcount - 1];
                         }
                    //Row 1st

                    this.TextBox4.Text = level.rows[0].columns[0].RoomIndex.ToString();
                    this.TextBox5.Text = level.rows[0].columns[1].RoomIndex.ToString();
                    this.TextBox6.Text = level.rows[0].columns[2].RoomIndex.ToString();
                    this.TextBox7.Text = level.rows[0].columns[3].RoomIndex.ToString();
                    this.TextBox8.Text = level.rows[0].columns[4].RoomIndex.ToString();
                    this.TextBox9.Text = level.rows[0].columns[5].RoomIndex.ToString(); 
                    this.TextBox10.Text = level.rows[0].columns[6].RoomIndex.ToString();
                    this.TextBox11.Text = level.rows[0].columns[7].RoomIndex.ToString();

                    //Row 2nd

                    this.TextBox12.Text = level.rows[1].columns[0].RoomIndex.ToString();
                    this.TextBox13.Text = level.rows[1].columns[1].RoomIndex.ToString();
                    this.TextBox14.Text = level.rows[1].columns[2].RoomIndex.ToString();
                    this.TextBox15.Text = level.rows[1].columns[3].RoomIndex.ToString();
                    this.TextBox16.Text = level.rows[1].columns[4].RoomIndex.ToString();
                    this.TextBox17.Text = level.rows[1].columns[5].RoomIndex.ToString();
                    this.TextBox18.Text = level.rows[1].columns[6].RoomIndex.ToString();
                    this.TextBox19.Text = level.rows[1].columns[7].RoomIndex.ToString();

                    //Row 3rd

                    this.TextBox20.Text = level.rows[2].columns[0].RoomIndex.ToString();
                    this.TextBox21.Text = level.rows[2].columns[1].RoomIndex.ToString();
                    this.TextBox22.Text = level.rows[2].columns[2].RoomIndex.ToString();
                    this.TextBox23.Text = level.rows[2].columns[3].RoomIndex.ToString();
                    this.TextBox24.Text = level.rows[2].columns[4].RoomIndex.ToString();
                    this.TextBox25.Text = level.rows[2].columns[5].RoomIndex.ToString();
                    this.TextBox26.Text = level.rows[2].columns[6].RoomIndex.ToString();
                    this.TextBox27.Text = level.rows[2].columns[7].RoomIndex.ToString();

                }

            }

            txtReader.Close();

            TextBox32.Text = rowcount.ToString();
            TextBox33.Text = columncount.ToString();
            Label16.Text = roomcount.ToString();

            for (int i = 0; i <= 25; i++)
            {
                ComboBox1.Items.Add(i);

            }


        }

        private void DrawBlocks()
        {


            for (int x = 0; x <= 9; x++)
            {

                for (int y = 0; y <= 2; y++)
                {

                    srect = new Rectangle(x * 64 - y * 26, x * 74 - y * 18, 64, 74);


                    switch (block[x, y])
                    {

                        case "space":



                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.Space, drect);

                            break;


                        case "floor":


                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.FloorA, drect);

                            break;

                        case "torch":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.FloorB_1, drect);

                            break;
                        case "loose":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.FloorLooseA_1, drect);

                            break;
                        case "spikes":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.SpikeA_5b, drect);

                            break;
                        case "posts":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.PostsA, drect);

                            break;
                        case "block":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.Block_single, drect);

                            break;
                        case "pressplate":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.FloorC1, drect);

                            break;

                        case "closeplate":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.FloorC2, drect);
                            break;

                        case "gate":

                            if (switche[x, y] == "open")
                                {
                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.DoorA_1, drect);
                                }

                               else
                               {
                                drect = new Rectangle((x * 64), (y * 74), 64, 74);
                                graphics.DrawImage(Properties.Resources.DoorA_12, drect);
                               }
                            break;
                        case "chomper":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.ChomperA_0, drect);


                            break;

                        case "lava":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.LavaA1, drect);


                            break;

                        case "exit":

                            if (switche[x, y] == "exit_close_right")
                            {
                                drect = new Rectangle((x * 64), (y * 74), 64, 74);
                                graphics.DrawImage(Properties.Resources.ExitA_Right, drect);
                            }

                            else if (switche[x, y] == "exit_close_left")
                            {
                                drect = new Rectangle((x * 64), (y * 74), 64, 74);
                                graphics.DrawImage(Properties.Resources.ExitA_Left, drect);
                            }


                            break;

                    }

                    switch (sprite[x, y])
                    {

                        case "kid":

                            playerrec = new Rectangle(playerx * 64, playery * 74, 64, 74);
                            graphics.DrawImage(Properties.Resources.Kid_1, playerrec);

                            break;
                        case "guard":


                            for (int i = 0; i <= guards - 1; i++)
                            {
                                guardrect[i] = new Rectangle(guardx[i] * 64, guardy[i] * 74, 64, 74);
                                graphics.DrawImage(Properties.Resources.Guard, guardrect[i]);
                            }


                            break;

                        default:

                            break;

                    }


                    switch (item[x, y])
                    {

                        case "flaskbig":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.Flask_big, drect);

                            break;

                        case "flask":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.Flask_small, drect);

                            break;
                        case "sword":

                            drect = new Rectangle((x * 64), (y * 74), 64, 74);
                            graphics.DrawImage(Properties.Resources.Sword_1, drect);

                            break;
                        default:

                            break;



                    }

                    graphics.DrawRectangle(Pens.Red, (mouseX * 64), (mouseY * 74), 64, 74);
                }
            }
            // COPY BACKBUFFER TO GRAPHICS OBJECT
            graphics = Graphics.FromImage(bb);

            // DRAW BACKBUFFER TO SCREEN
            BBG = pictureBox1.CreateGraphics();
            BBG.DrawImage(bb, 0, 0, 640, 222);

            graphics.Clear(Color.Black);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawBlocks();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (mouseX >= 0)
                mMapX = mouseX;

            if (mouseY >= 0)
                mMapY = mouseY;
            
            if (selectedtile >= 0)
            {



                switch (selectedtile)
                {

                    case 0:


                        block[mMapX, mMapY] = "space";

                        break;


                    case 1:

                        block[mMapX, mMapY] = "floor";

                        break;

                    case 2:

                        block[mMapX, mMapY] = "torch";

                        break;
                    case 3:

                        block[mMapX, mMapY] = "loose";

                        break;

                    case 4:

                        block[mMapX, mMapY] = "spikes";

                        break;

                    case 5:

                        block[mMapX, mMapY] = "posts";

                        break;
                    case 6:

                        block[mMapX, mMapY] = "block";

                        break;

                    case 7:

                        block[mMapX, mMapY] = "block";

                        break;
                    case 8:

                        block[mMapX, mMapY] = "pressplate";
                        switche[mMapX, mMapY] = "open";

                        break;
                    case 9:

                        block[mMapX, mMapY] = "closeplate";
                        switche[mMapX, mMapY] = "open";

                        break;
                    case 10:

                        block[mMapX, mMapY] = "gate";
                        switche[mMapX, mMapY] = "open";
                        
                        break;
                    case 11:

                        block[mMapX, mMapY] = "gate2";
                        switche[mMapX, mMapY] = "close";

                        break;
                    case 12:

                        block[mMapX, mMapY] = "chomper";

                        break;
                    case 13:

                        block[mMapX, mMapY] = "lava";

                        break;
                    case 14:

                        block[mMapX, mMapY] = "exit";

                        break;
                    case 15:

                        block[mMapX, mMapY] = "exit";

                        break;

                }

            }


            if (selectedsprite >= 0)
            {
                switch (selectedsprite)
                {

                    case 0:

                        sprite[mMapX, mMapY] = "nothing";

                        break;
                    case 1:


                        if (playerx != 0 & playery != 0)
                        {
                            sprite[playerx, playery] = "nothing";

                            TextBox28.Text = mMapX.ToString();
                            TextBox29.Text = mMapY.ToString();


                            playerx = Convert.ToInt32(TextBox28.Text);
                            playery = Convert.ToInt32(TextBox29.Text);

                            sprite[mMapX, mMapY] = "kid";


                        }
                        else
                        {
                            sprite[playerx, playery] = "nothing";

                            TextBox28.Text = mMapX.ToString();
                            TextBox29.Text = mMapY.ToString();



                            playerx = Convert.ToInt32(TextBox28.Text);
                            playery = Convert.ToInt32(TextBox29.Text);

                            sprite[mMapX, mMapY] = "kid";


                        }

                        break;
                    case 2:

                        guards += 1;


                        for (int i = 0; i <= guards - 1; i++)
                        {


                            if (guardx[i] >= 0 & guardy[i]  >= 0)
                            {

                                sprite[guardx[i], guardy[i]] = "nothing";

                                TextBox30.Text = mMapX.ToString();
                                TextBox31.Text = mMapY.ToString();

                                guardx[i] = Convert.ToInt32(TextBox30.Text);
                                guardy[i] = Convert.ToInt32(TextBox31.Text);

                                sprite[guardx[i], guardy[i]] = "guard";
 


                            }
                            else
                            {
                                sprite[guardx[i], guardy[i]] = "nothing";

                                TextBox30.Text = mMapX.ToString();
                                TextBox31.Text = mMapY.ToString();

                                guardx[i] = Convert.ToInt32(TextBox30.Text);
                                guardy[i] = Convert.ToInt32(TextBox31.Text);

                                sprite[guardx[i], guardy[i]] = "guard";


                            }

                        }


                        break;
                }



            }


            if (selecteditem >= 0)
            {
                switch (selecteditem)
                {

                    case 0:
                        item[mMapX, mMapY] = "nothing";

                        break;
                    case 1:
                        item[mMapX, mMapY] = "flaskbig";

                        break;
                    case 2:
                        item[mMapX, mMapY] = "flask";

                        break;
                    case 3:
                        item[mMapX, mMapY] = "sword";

                        break;


                }




            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            double X = e.X;
            double Y = e.Y;

            mouseX = (int)Math.Floor(X / 64);

            mouseY = (int)Math.Floor(Y / 74);


            Label9.Text = mouseX.ToString();
            Label10.Text = mouseY.ToString();

        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            selectedtile = 0;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Blackspace";
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            selectedtile = 1;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Floor";
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            selectedtile = 2;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Torch";
        }

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            selectedtile = 3;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Loose";
        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {
            selectedtile = 4;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Spikes";
        }

        private void PictureBox9_Click(object sender, EventArgs e)
        {
            selectedtile = 5;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Posts";
        }

        private void PictureBox10_Click(object sender, EventArgs e)
        {
            selectedtile = 6;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Wall";
        }

        private void PictureBox11_Click(object sender, EventArgs e)
        {
            selectedtile = 7;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Block";
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            selectedsprite = 1;
            selectedtile = 1;
            selecteditem = 0;
            this.TextBox3.Text = "Kid";
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            selectedsprite = 2;
            selectedtile = 1;
            selecteditem = 0;
            this.TextBox3.Text = "Guard";
        }

        private void PictureBox12_Click(object sender, EventArgs e)
        {
            selecteditem = 1;
            selectedsprite = 0;
            selectedtile = 0;
            this.TextBox3.Text = "Flask big";
        }

        private void PictureBox13_Click(object sender, EventArgs e)
        {
            selecteditem = 2;
            selectedsprite = 0;
            selectedtile = 0;
            this.TextBox3.Text = "Flask";
        }

        private void PictureBox14_Click(object sender, EventArgs e)
        {
            selecteditem = 3;
            selectedsprite = 0;
            selectedtile = 0;
            this.TextBox3.Text = "Sword";
        }

        private void LoadRoom(string path)
        {
            GenerateNewMap();

            System.Xml.Serialization.XmlSerializer ax = default(System.Xml.Serialization.XmlSerializer);
            Stream txtReader = File.Open(path, FileMode.Open);

            //TextReader txtReader = File.OpenText(filePath);

            ax = new XmlSerializer(typeof(Map));
            map = (Map)ax.Deserialize(txtReader);

            //Stream astream = this.GetType().Assembly.GetManifestResourceStream(filePath);
            richTextBox1.AppendText("<Map>" + Environment.NewLine);
            richTextBox1.AppendText("<Rows>" + Environment.NewLine);
            richTextBox1.AppendText("<Row>" + Environment.NewLine);
            richTextBox1.AppendText("<columns>" + Environment.NewLine);

            for (int r = 0; r <= map.rows.Count() - 1; r++)
            {
                Label21.Text = r.ToString();


                for (int i = 0; i <= map.rows[r].columns.Length - 1; i++)
                {
                    Label22.Text = i.ToString();

                    block[i, r] = Enum.GetName(typeof(Enumeration.TileType), map.rows[r].columns[i].tileType);
                    
                    //Load gate switch state
                    if (block[i, r] == "gate" && map.rows[r].columns[i].state == Enumeration.StateTile.opened)
                        switche[i, r] = "open";
                    else if (block[i, r] == "gate" && map.rows[r].columns[i].state == Enumeration.StateTile.closed)
                        switche[i, r] = "closed";

                    //Load exit switch state
                    if (block[i, r] == "exit" && map.rows[r].columns[i].state == Enumeration.StateTile.exit_close_left)
                        switche[i, r] = "exit_close_left";
                    else if (block[i, r] == "exit" && map.rows[r].columns[i].state == Enumeration.StateTile.exit_close_right)
                        switche[i, r] = "exit_close_right";

                    sprite[i, r] = Enum.GetName(typeof(Enumeration.SpriteType), map.rows[r].columns[i].spriteType);
                    if (sprite[i, r] == "kid")
                         {
                             playerx = i;
                             playery = r;

                         }
                    else if (sprite[i, r] == "guard")
                        {
                            
                            guardx[guards] = i;
                            guardy[guards] = r;
                            guards += 1;
                        }


                    item[i, r] = Enum.GetName(typeof(Enumeration.Items), map.rows[r].columns[i].item);


                    richTextBox1.AppendText("<Column>" + Environment.NewLine);

                    if ((sprite[r, i] != null))
                    {
                        richTextBox1.AppendText("<spriteType>" + sprite[r, i] + "</spriteType>" + Environment.NewLine);

                    }

                    if ((item[r, i] != null))
                    {
                        richTextBox1.AppendText("<item>" + item[r, i] + "</item>" + Environment.NewLine);

                    }
                    richTextBox1.AppendText("<tileType>" + block[r, i] + "</tileType>" + Environment.NewLine);
                    richTextBox1.AppendText("</Column>" + Environment.NewLine);

                }


            }

            txtReader.Close();

            richTextBox1.AppendText("</columns>" + Environment.NewLine);
            richTextBox1.AppendText("</Row>" + Environment.NewLine);
            richTextBox1.AppendText("</rows>" + Environment.NewLine);

            richTextBox1.AppendText("</Map>" + Environment.NewLine);

        }

        private void SaveRoom(string path)
        {

            Map map = new Map();


            for (int r = 0; r <= map.rows.Count() - 1; r++)
            {
                Label21.Text = r.ToString();


                for (int i = 0; i <= map.rows[r].columns.Length - 1; i++)
                {
                    Label22.Text = i.ToString();

                    map.rows[r].columns[i].tileType = (Enumeration.TileType)Enum.Parse(typeof(Enumeration.TileType), block[i, r]);
                    map.rows[r].columns[i].spriteType = (Enumeration.SpriteType)Enum.Parse(typeof(Enumeration.SpriteType), sprite[i, r]);
                    map.rows[r].columns[i].item = (Enumeration.Items)Enum.Parse(typeof(Enumeration.Items), item[i, r]);

                    if (switche[i, r] != null)
                        map.rows[r].columns[i].state = (Enumeration.StateTile)Enum.Parse(typeof(Enumeration.StateTile), switche[i, r]);
                }

            }

            XmlSerializer serialize = new XmlSerializer(typeof(Map));

            using (var writer = new StreamWriter(path))
            {
                serialize.Serialize(writer, map);
            }

        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox2.Items.Clear();
            ComboBox1.Items.Clear();

            if (listBox1.SelectedItem != null)
                {
            int current = listBox1.SelectedIndex;
            string currenti = listBox1.SelectedItem.ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + "Content\\Levels\\";

            if (!ComboBox2.Items.Contains(current))
            {

                ComboBox2.Items.Add(current);

            }


            this.TextBox2.Text = path + currenti;

            LoadLevel(path + currenti);

                }

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string current = "";
            int x = 0;

            for (int i = 0; i <= ComboBox1.Items.Count; i++)
            {
                

                x = ComboBox1.SelectedIndex;

                current = x.ToString();

                
            }

            foreach (Room r in rooms)
                if (r.roomIndex == x)
                    currentroom = r;

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            if (File.Exists(path))
            LoadRoom(path);

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            selectedtile = 8;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Switch Open";
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            selectedtile = 9;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Switch Close";
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            selectedtile = 10;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Door Open";
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            selectedtile = 11;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Door Close";
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            selectedtile = 12;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Chomper";
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            selectedtile = 13;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Lava";
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            selectedtile = 14;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Exit Left";
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            selectedtile = 15;
            selectedsprite = 0;
            selecteditem = 0;
            this.TextBox3.Text = "Exit Right";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to create new map?", "New Map", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
               GenerateNewMap();
 
    
            
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "Prince Level files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if ((this.openFileDialog1.ShowDialog() == DialogResult.OK))
            {

                LoadLevel(openFileDialog1.FileName);
               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (level != null)
            {
            if (currentroom != null)
                currentroom = level.UpRoom(rooms, currentroom, level);

            string current = currentroom.roomIndex.ToString();
            //MessageBox.Show(currentroom.roomIndex + "," + currentroom.roomName + "x=" + currentroom.roomX + " y=" + currentroom.roomY);

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            ComboBox1.SelectedIndex = currentroom.roomIndex;

            if (File.Exists(path))
                LoadRoom(path);
             }
            else
                MessageBox.Show("No level has been loaded");

        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (level != null)
            {
            if (currentroom != null)
               currentroom = level.DownRoom(rooms, currentroom, level);

            string current = currentroom.roomIndex.ToString();
            //MessageBox.Show(currentroom.roomIndex + "," + currentroom.roomName + "x=" + currentroom.roomX + " y=" + currentroom.roomY);

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            ComboBox1.SelectedIndex = currentroom.roomIndex;

            if (File.Exists(path))
                LoadRoom(path);
            }
            else
                MessageBox.Show("No level has been loaded");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (level != null)
             {
            if (currentroom != null)
                currentroom = level.RightRoom(rooms, currentroom, level);

            string current = currentroom.roomIndex.ToString();
            //MessageBox.Show(currentroom.roomIndex + "," + currentroom.roomName + "x=" + currentroom.roomX + " y=" + currentroom.roomY);

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            ComboBox1.SelectedIndex = currentroom.roomIndex;

            if (File.Exists(path))
                LoadRoom(path);
             }
            else
                MessageBox.Show("No level has been loaded");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (level != null)
             {
            if (currentroom != null)
                currentroom = level.LeftRoom(rooms, currentroom, level);

            string current = currentroom.roomIndex.ToString();
            //MessageBox.Show(currentroom.roomIndex + "," + currentroom.roomName + "x=" + currentroom.roomX + " y=" + currentroom.roomY);

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            ComboBox1.SelectedIndex = currentroom.roomIndex;

            if (File.Exists(path))
                LoadRoom(path);
             }
            else
                MessageBox.Show("No level has been loaded");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            if (level != null)
             {
            if (currentroom != null)
                currentroom = level.StartRoom(rooms, currentroom, level);

            string current = currentroom.roomIndex.ToString();
            //MessageBox.Show(currentroom.roomIndex + "," + currentroom.roomName + "x=" + currentroom.roomX + " y=" + currentroom.roomY);

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            ComboBox1.SelectedIndex = currentroom.roomIndex;

            if (File.Exists(path))
                LoadRoom(path);
             }
            else
             {

                 MessageBox.Show("No level has been loaded");
             }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (level != null)
             
            if (currentroom != null)
                {

            string current = currentroom.roomIndex.ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/Rooms/MAP_dungeon_prison_" + current + ".xml";

            SaveRoom(path);

                 }

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog1.Filter = "Prince Level files (*.xml)|*.xml|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;

            if ((this.saveFileDialog1.ShowDialog() == DialogResult.OK))
            {

                SaveRoom(saveFileDialog1.FileName);

            }
        }


               

    }
}