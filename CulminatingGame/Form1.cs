using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CulminatingGame
{
    public partial class Form1 : Form
    {

        //Variables
        Bitmap Player, select, Police; //bitmap for player sprite and selected portion of sheet
        Bitmap[] EnSelect = new Bitmap[6];
        Rectangle srcRect = new Rectangle(); //select a specific part of sprite sheet.
        Rectangle[] EnsrcRect = new Rectangle[6];
        Image Char;//image to draw player character
        Image[] Popo=new Image[6];

        Rectangle CharBounds;//collisions
        Rectangle hitbox;//player hit box
        Point CharPos, ScreenPos;
        Point[] GuardPos = new Point[6];
        Boolean[] GuardLeft = new Boolean[6]; //directions of guards, only using left and right to keep it simple
        Boolean[] GuardRight = new Boolean[6];
        Point[] LightPos = new Point[6];
        Rectangle[] Light = new Rectangle[6]; // to draw the light in a cone/pie shape
        Rectangle[] LDetect = new Rectangle[6]; // the detection box of the drawn light because the actual rectangle of the light is far bigger than the drawn pie
        Size LSize = new Size(500, 500); // size of the Light rectangle

        Boolean upMove, up, downMove, down, leftMove, l, rightMove, r;  //keypressed and movement 
        int move;
        int[] enmove= new int[6];//for looping through animation
        int standPose; // select a standing animation

        Rectangle[] Wall = new Rectangle[100];//walls
        Point[] WallLoc = new Point[100];//locations of rectangles are read only

        Rectangle[] Comp = new Rectangle[2];//walls
        Point[] CompLoc = new Point[2];//locations of rectangles are read only
        Image PC;

        Rectangle[] HideSpot = new Rectangle[7]; //hiding spots
        Point[] HideLoc = new Point[7]; // hiding location

        Pen penColor = new Pen(Color.Black); //pen colour
        Brush brushColor = new SolidBrush(Color.Red);//brushe colour

        SolidBrush Shade = new SolidBrush(Color.FromArgb(200, 0, 0, 0)); //for hiding spots
        SolidBrush Stealth = new SolidBrush(Color.FromArgb(50, 0, 0, 0)); //filter on top of everything
        Rectangle RecShade; //filter rectangle
        int Fade; //for filter fade change

        SolidBrush FlashL = new SolidBrush(Color.FromArgb(150, 255, 255, 0)); //flashlight colour

        SolidBrush InvisWall = new SolidBrush(Color.FromArgb(25, 0, 255, 0)); //for when invisble walls are visable
        Pen Invisoutline = new Pen(Color.FromArgb(200, 0, 255, 0));
 
        Boolean SKey1 = false, Hidden,Detect,Interact,Visable=false, Artifact=false; //booleans controlling the game
        Boolean Win = false;
        
        //varibles for the floor panels
        Rectangle[] Switch= new Rectangle[5];
        Point[] SwitchPos = new Point[5];
        int Trap;

        //chests
        Image LChest, SChest;
        Rectangle[]Chest=new Rectangle[2];
        Point[] ChestLoc = new Point[2];

        //lbInfo location/counter
        Point InfoLoc;
        int InfoTmr;

        //score
        int money;

        //music
        WMPLib.WindowsMediaPlayer backtrack = new WMPLib.WindowsMediaPlayer();
        int clocktime; //for looping music
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //setting up resources
            PC = new Bitmap(CulminatingGame.Properties.Resources.CompSprite);
            LChest = new Bitmap(CulminatingGame.Properties.Resources.LargeChestClosed);
            SChest = new Bitmap(CulminatingGame.Properties.Resources.SmallChest);
            //background music
            backtrack.URL = @"Stealthstep.mp3";
            Clock.Enabled = true;

            InfoLoc.Y = -50; //LOCATION OF THE LABLE

            //set player sprites sheet
            Player = new Bitmap(CulminatingGame.Properties.Resources.Character);
            srcRect = new Rectangle(0, 0, 15, 20);
            select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
            Char = select;
            CharPos.X = (this.Width / 2) - (Char.Width * 3 / 2); //center horizontal
            CharPos.Y = (this.Height / 2) - (Char.Height * 3 / 2); //center vertical
            hitbox = new Rectangle(CharPos.X-2, CharPos.Y+30, Char.Width * 3+4, Char.Height*2-8); //for collisions

            //set police sprites
            Police = new Bitmap(CulminatingGame.Properties.Resources.PoliceFix);
            for (int ndx = 0; ndx <= 5; ndx++)
            {
                EnsrcRect[ndx] = new Rectangle(0, 0, 15, 20);
                EnSelect[ndx] = (Bitmap)Police.Clone(srcRect, Police.PixelFormat);
                Popo[ndx] = EnSelect[ndx];
            }
            //guard start positions
            GuardPos[0].X = 1800;
            GuardPos[0].Y = 1150;
            LightPos[0].X = GuardPos[0].X - LSize.Width / 2 + (Popo[0].Width * 3 / 2);
            LightPos[0].Y = GuardPos[0].Y -  LSize.Height / 2 +(Popo[0].Height*3/2) ;
            Light[0] = new Rectangle(LightPos[0],LSize);
            LDetect[0] = new Rectangle(LightPos[0].X, LightPos[0].Y, Light[0].Width / 2, 2000);

            GuardPos[1].X = 1250;
            GuardPos[1].Y = 1550;
            LightPos[1].X = GuardPos[1].X - LSize.Width / 2 + (Popo[1].Width * 3 / 2);
            LightPos[1].Y = GuardPos[1].Y - LSize.Height / 2 + (Popo[1].Height * 3 / 2);
            Light[1] = new Rectangle(LightPos[1], LSize);

            GuardPos[2].X = 1800;
            GuardPos[2].Y = 1950;
            LightPos[2].X = GuardPos[2].X - LSize.Width / 2 + (Popo[2].Width * 3 / 2);
            LightPos[2].Y = GuardPos[2].Y - LSize.Height / 2 + (Popo[2].Height * 3 / 2);
            Light[2] = new Rectangle(LightPos[2], LSize);

            GuardPos[3].X = 1250;
            GuardPos[3].Y = 2350;
            LightPos[3].X = GuardPos[3].X - LSize.Width / 2 + (Popo[3].Width * 3 / 2);
            LightPos[3].Y = GuardPos[3].Y - LSize.Height / 2 + (Popo[3].Height * 3 / 2);
            Light[3] = new Rectangle(LightPos[3], LSize);

            GuardPos[4].X = 2200;
            GuardPos[4].Y = 2050;
            LightPos[4].X = GuardPos[4].X - LSize.Width / 2 + (Popo[4].Width * 3 / 2);
            LightPos[4].Y = GuardPos[4].Y - LSize.Height / 2 + (Popo[4].Height * 3 / 2);
            Light[4] = new Rectangle(LightPos[4], LSize);

            GuardPos[5].X = 4000;
            GuardPos[5].Y = 1750;
            LightPos[5].X = GuardPos[5].X - LSize.Width / 2 + (Popo[5].Width * 3 / 2);
            LightPos[5].Y = GuardPos[5].Y - LSize.Height / 2 + (Popo[5].Height * 3 / 2);
            Light[5] = new Rectangle(LightPos[5], LSize);

            //computer positions
            CompLoc[0].X = 4400;
            CompLoc[0].Y = 225;
            Comp[0] = new Rectangle(CompLoc[0].X, CompLoc[0].Y, 100, 100);

            CompLoc[1].X = 300;
            CompLoc[1].Y = 1725;
            Comp[1] = new Rectangle(CompLoc[1].X, CompLoc[1].Y, 100, 100);

            //size of screen
            RecShade = new Rectangle(0, 0, pbWorld.Width, pbWorld.Height);

            //boundaries at 90000x90000
            WallLoc[0].X = -90000;
            WallLoc[0].Y = -90000;
            Wall[0] = new Rectangle(WallLoc[0].X, WallLoc[0].Y, 270000, 90000); //top bounds

            WallLoc[1].X = -90000;
            WallLoc[1].Y = 0;
            Wall[1] = new Rectangle(WallLoc[1].X, WallLoc[1].Y, 90000, 180000);//left bounds

            WallLoc[2].X = 90000;
            WallLoc[2].Y = 0;
            Wall[2] = new Rectangle(WallLoc[2].X, WallLoc[2].Y, 90000, 180000);//right bounds

            WallLoc[3].X = 0;
            WallLoc[3].Y = 90000;
            Wall[3] = new Rectangle(WallLoc[3].X, WallLoc[3].Y, 90000, 90000);//bottom bounds

            //Parts of buildings and structures
            WallLoc[4].X = 0;
            WallLoc[4].Y = 0;
            Wall[4] = new Rectangle(WallLoc[4].X, WallLoc[4].Y, 300, 90000);

            WallLoc[5].X = 0;
            WallLoc[5].Y = 900;
            Wall[5] = new Rectangle(WallLoc[5].X, WallLoc[5].Y, 900, 900);

            WallLoc[9].X = 300;
            WallLoc[9].Y = 1900;
            Wall[9] = new Rectangle(WallLoc[9].X, WallLoc[9].Y, 600, 100);

            WallLoc[10].X = 300;
            WallLoc[10].Y = 1700;
            Wall[10] = new Rectangle(WallLoc[10].X, WallLoc[10].Y, 600, 200);

            WallLoc[12].X = 300;
            WallLoc[12].Y = 1700;
            Wall[12] = new Rectangle(WallLoc[12].X, WallLoc[12].Y, 600, 100);

            WallLoc[11].X = 1000;
            WallLoc[11].Y = 1800;
            Wall[11] = new Rectangle(WallLoc[11].X, WallLoc[11].Y, 10, 50); //secret key

            WallLoc[8].X = 1000;
            WallLoc[8].Y = 900;
            Wall[8] = new Rectangle(WallLoc[8].X, WallLoc[8].Y, 100, 1600);

            WallLoc[6].X = 0;
            WallLoc[6].Y = 0;
            Wall[6] = new Rectangle(WallLoc[6].X, WallLoc[6].Y, 90000, 300);

            WallLoc[7].X = 0;
            WallLoc[7].Y = 175;
            Wall[7] = new Rectangle(WallLoc[7].X, WallLoc[7].Y, 90000, 125);

            WallLoc[13].X = 1000;
            WallLoc[13].Y = 0;
            Wall[13] = new Rectangle(WallLoc[8].X, WallLoc[8].Y, 1000, 800);

            WallLoc[14].X = 1000;
            WallLoc[14].Y = 900;
            Wall[14] = new Rectangle(WallLoc[14].X, WallLoc[14].Y, 900, 150);

            WallLoc[15].X = 1000;
            WallLoc[15].Y = 1000;
            Wall[15] = new Rectangle(WallLoc[15].X, WallLoc[15].Y, 450, 100);

            HideLoc[0].X = 1450;
            HideLoc[0].Y = 1000;
            HideSpot[0] = new Rectangle(HideSpot[0].X, HideSpot[0].Y, 100, 100);

            WallLoc[16].X = 1550;
            WallLoc[16].Y = 1000;
            Wall[16] = new Rectangle(WallLoc[16].X, WallLoc[16].Y, 350, 100);

            WallLoc[17].X = 2000;
            WallLoc[17].Y = 0;
            Wall[17] = new Rectangle(WallLoc[16].X, WallLoc[16].Y,100, 2200);

            WallLoc[18].X = 1300;
            WallLoc[18].Y = 1300;
            Wall[18] = new Rectangle(WallLoc[18].X, WallLoc[18].Y, 700, 150);

            WallLoc[19].X = 1100;
            WallLoc[19].Y = 1700;
            Wall[19] = new Rectangle(WallLoc[19].X, WallLoc[19].Y, 700, 150);

            WallLoc[20].X = 1300;
            WallLoc[20].Y = 1400;
            Wall[20] = new Rectangle(WallLoc[20].X, WallLoc[20].Y, 250, 100);

            HideLoc[1].X = 1550;
            HideLoc[1].Y = 1400;
            HideSpot[1] = new Rectangle(HideSpot[1].X, HideSpot[1].Y, 100, 100);

            WallLoc[21].X = 1650;
            WallLoc[21].Y = 1400;
            Wall[21] = new Rectangle(WallLoc[21].X, WallLoc[21].Y, 400, 100);

            WallLoc[22].X = 1000;
            WallLoc[22].Y = 1800;
            Wall[22] = new Rectangle(WallLoc[22].X, WallLoc[22].Y, 450, 100);

            HideLoc[2].X = 1450;
            HideLoc[2].Y = 1800;
            HideSpot[2] = new Rectangle(HideSpot[2].X, HideSpot[2].Y, 100, 100);

            WallLoc[23].X = 1550;
            WallLoc[23].Y = 1800;
            Wall[23] = new Rectangle(WallLoc[23].X, WallLoc[23].Y, 250, 100);

            WallLoc[24].X = 1300;
            WallLoc[24].Y = 2100;
            Wall[24] = new Rectangle(WallLoc[23].X, WallLoc[23].Y, 700, 150);

            WallLoc[25].X = 1650;
            WallLoc[25].Y = 2200;
            Wall[25] = new Rectangle(WallLoc[25].X, WallLoc[25].Y, 450, 100);

            HideLoc[3].X = 1550;
            HideLoc[3].Y = 2200;
            HideSpot[3] = new Rectangle(HideSpot[3].X, HideSpot[3].Y, 100, 100);

            WallLoc[26].X = 1300;
            WallLoc[26].Y = 2200;
            Wall[26] = new Rectangle(WallLoc[26].X, WallLoc[26].Y, 250, 100);

            WallLoc[27].X = 0;
            WallLoc[27].Y = 2500;
            Wall[27] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 6000, 1000);

            WallLoc[32].X = 4000;
            WallLoc[32].Y = 350;
            Wall[32] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 100, 6000);

            WallLoc[33].X = 4010;
            WallLoc[33].Y = 600;
            Wall[33] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 6000, 6000);

            WallLoc[34].X = 4500;
            WallLoc[34].Y = 0;
            Wall[34] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 6000, 6000);

            //floor switches that activate certain parts of the puzzle
            SwitchPos[0].X = 350;
            SwitchPos[0].Y = 2050;
            Switch[0] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[1].X = 450;
            SwitchPos[1].Y = 2050;
            Switch[1] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[2].X = 550;
            SwitchPos[2].Y = 2050;
            Switch[2] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[3].X = 650;
            SwitchPos[3].Y = 2050;
            Switch[3] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[4].X = 650;
            SwitchPos[4].Y = 2050;
            Switch[4] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);


            //puzzle walls
            WallLoc[28].X = 350;
            WallLoc[28].Y = 2250;
            Wall[28] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 200, 50); //draw in different colour for moving wall puzzle

            WallLoc[29].X = 550;
            WallLoc[29].Y = 2250;
            Wall[29] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 200, 50); //draw in different colour for moving wall puzzle

            WallLoc[30].X = 700;
            WallLoc[30].Y = 2300;
            Wall[30] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 50, 200); //draw in different colour for moving wall puzzle

            WallLoc[31].X = 350;
            WallLoc[31].Y = 2300;
            Wall[31] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 50, 200); //draw in different colour for moving wall puzzle

            //invisible "maze" walls (will not be drawn but will be there)
            WallLoc[35].X = 2200;
            WallLoc[35].Y = 400;
            Wall[35] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 100, 1900);

            WallLoc[36].X = 2200;
            WallLoc[36].Y = 400;
            Wall[36] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 1800, 100);

            WallLoc[37].X = 2200;
            WallLoc[37].Y = 2200;
            Wall[37] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 800, 100);

            WallLoc[38].X = 3200;
            WallLoc[38].Y = 2200;
            Wall[38] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 800, 100);

            WallLoc[39].X = 2600;
            WallLoc[39].Y = 1900;
            Wall[39] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 1400, 100);

            WallLoc[40].X = 2200;
            WallLoc[40].Y = 1900;
            Wall[40] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 200, 100);

            WallLoc[41].X = 3500;
            WallLoc[41].Y = 1600;
            Wall[41] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 500, 100);

            WallLoc[42].X = 2200;
            WallLoc[42].Y = 1600;
            Wall[42] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 1100, 100);

            //chest locations
            ChestLoc[0].X = 525;
            ChestLoc[0].Y = 2400;
            Chest[0] = new Rectangle(ChestLoc[0].X, ChestLoc[0].Y, 50, 50);

            ChestLoc[1].X = 3025;
            ChestLoc[1].Y = 600;
            Chest[1] = new Rectangle(ChestLoc[1].X, ChestLoc[1].Y, 175, 100);
        }

        private void pbWorld_Paint(object sender, PaintEventArgs e)
        {
            for (int ndx = 0; ndx <= 3; ndx++)
            {
                e.Graphics.FillRectangle(Brushes.Black, Wall[ndx]);
            }

            for (int ndx = 0; ndx <= 4; ndx++)
            {
                e.Graphics.FillRectangle(Brushes.BurlyWood, Switch[ndx]);
            }

            //chests

            e.Graphics.DrawImage(SChest, Chest[0]);
            e.Graphics.DrawImage(LChest, Chest[1]);
            

            //moving walls
            e.Graphics.FillRectangle(Brushes.Aquamarine, Wall[28]);
            e.Graphics.FillRectangle(Brushes.Aquamarine, Wall[29]);
            e.Graphics.FillRectangle(Brushes.Aquamarine, Wall[30]);
            e.Graphics.FillRectangle(Brushes.Aquamarine, Wall[31]);

            //e.Graphics.FillRectangle(
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[6]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[7]);

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[5]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[14]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[15]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[16]);

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[18]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[20]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[21]);

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[19]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[22]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[23]);

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[8]);

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[24]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[25]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[26]);

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[27]);
           

            e.Graphics.FillRectangle(Brushes.DimGray, Wall[9]);
            e.Graphics.FillRectangle(Brushes.Gray, Wall[12]);
            e.Graphics.DrawImage(PC, Comp[1]); //computers
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[10]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[11]); //hidden switch only detectable by hitting it
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[13]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[4]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[17]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[32]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[33]);
            e.Graphics.FillRectangle(Brushes.DimGray, Wall[34]);
            
            //invible walls (for testing remove for final)
            if (Visable == true)
            {
                e.Graphics.FillRectangle(InvisWall, Wall[35]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[35]);
                e.Graphics.FillRectangle(InvisWall, Wall[36]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[36]);
                e.Graphics.FillRectangle(InvisWall, Wall[37]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[37]);
                e.Graphics.FillRectangle(InvisWall, Wall[38]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[38]);
                e.Graphics.FillRectangle(InvisWall, Wall[39]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[39]);
                e.Graphics.FillRectangle(InvisWall, Wall[40]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[40]);
                e.Graphics.FillRectangle(InvisWall, Wall[41]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[41]);
                e.Graphics.FillRectangle(InvisWall, Wall[42]);
                e.Graphics.DrawRectangle(Invisoutline, Wall[42]);
            }
            e.Graphics.DrawImage(PC, Comp[0]);
            
            e.Graphics.ResetTransform(); // reset them and divides draw codes.

            //character
            e.Graphics.DrawImage(Char, CharPos.X, CharPos.Y, Char.Width * 3, Char.Height * 3); //image and Location

            for (int ndx = 0; ndx <= 5; ndx++)
            {
                e.Graphics.DrawImage(Popo[ndx], GuardPos[ndx].X, GuardPos[ndx].Y, Popo[ndx].Width * 3, Popo[ndx].Height * 3); //image and Location

            }
            e.Graphics.ResetTransform();

            //draw hiding spots
            for (int ndx = 0; ndx <= 6; ndx++)
            {
                e.Graphics.FillRectangle(Shade, HideSpot[ndx]);
            }
            e.Graphics.ResetTransform();

            //filter colour
                e.Graphics.FillRectangle(Stealth, RecShade);

            e.Graphics.ResetTransform();

            //light detection
            for (int ndx = 0; ndx <= 5; ndx++)
            {
                if (GuardRight[ndx] == true)
                {
                    e.Graphics.FillPie(FlashL, Light[ndx], -45, 90); //points right
                    LDetect[ndx] = new Rectangle(LightPos[ndx].X + Light[ndx].Width / 2, LightPos[ndx].Y + 75, Light[ndx].Width / 2, 350);
                   
                }
                if (GuardLeft[ndx] == true)
                {
                    e.Graphics.FillPie(FlashL, Light[ndx], 225, -90); //points left
                    LDetect[ndx] = new Rectangle(LightPos[ndx].X, LightPos[ndx].Y + 75, Light[ndx].Width / 2, 350);
                   
                }
            }
            e.Graphics.ResetTransform();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            pbWorld.Refresh(); //refresh pbWorld

            //move the drawn objects to their calculated location
            for (int ndx = 0; ndx <= 99; ndx++) 
            {
                Wall[ndx].Location = WallLoc[ndx];
            }

            for (int ndx = 0; ndx <= 6; ndx++)
            {
                HideSpot[ndx].Location = HideLoc[ndx];
            }

            for (int ndx = 0; ndx <= 3; ndx++)
            {
                Switch[ndx].Location = SwitchPos[ndx];
            }

            for (int ndx = 0; ndx <= 5; ndx++)
            {
                LightPos[ndx].X = GuardPos[ndx].X - LSize.Width / 2 + (Popo[0].Width * 3 / 2);
                LightPos[ndx].Y = GuardPos[ndx].Y + 20 - LSize.Height / 2 + (Popo[0].Height * 3 / 2);
                Light[ndx] = new Rectangle(LightPos[ndx], LSize);   
            }

            for (int ndx = 0; ndx <= 1; ndx++)
            {
                Comp[ndx].Location = CompLoc[ndx];
                Chest[ndx].Location= ChestLoc[ndx];
            }
            lbInfo.Location = InfoLoc;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Left || e.KeyData == Keys.A)
            {
                l = true;
            }

            if (e.KeyData == Keys.Right || e.KeyData == Keys.D)
            {
                r = true;
            }

            if (e.KeyData == Keys.Up || e.KeyData == Keys.W)
            {
                up = true;
            }

            if (e.KeyData == Keys.Down || e.KeyData == Keys.S)
            {
                down = true;
            }

            if (e.KeyData == Keys.E)
            {
                Interact = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Left || e.KeyData == Keys.A)
            {
                l = false;
            }

            if (e.KeyData == Keys.Right || e.KeyData == Keys.D)
            {
                r = false;
            }

            if (e.KeyData == Keys.Up || e.KeyData == Keys.W)
            {
                up = false;
            }

            if (e.KeyData == Keys.Down || e.KeyData == Keys.S)
            {
                down = false;
            }

            if (e.KeyData == Keys.E)
            {
                Interact = false;
            }
        }

        private void tmrMove_Tick(object sender, EventArgs e)
        {
            //win by getting artifact and getting back to start
            if (Artifact == true && ScreenPos.X <= 0 && ScreenPos.Y <= 0)
            {
                Win = true;
                tmrFin.Enabled = true;
            }
            //what button is pressed
            if (l == true)
                leftMove = true;
            else
                leftMove = false;

            if (r == true)
                rightMove = true;
            else
                rightMove = false;

            if (up == true)
                upMove = true;
            else
                upMove = false;

            if (down == true)
                downMove = true;
            else
                downMove = false;            

            lbScreenLoc.Text = ScreenPos.ToString(); //display"location" of character

            //player enters hidding spots
            for (int ndx = 0; ndx <= 6; ndx++)
            {

                if (HideSpot[ndx].IntersectsWith(hitbox) && HideSpot[ndx].Bottom >= hitbox.Bottom-2) // have to be entirely in the shadow
                    CharBounds = Rectangle.Intersect(hitbox, HideSpot[ndx]);
                else
                    CharBounds = new Rectangle(0, 0, 0, 0);
                if (!CharBounds.IsEmpty)
                {
                    Hidden = true;
                }
            }

            //player gets detected by police
            for (int ndx = 0; ndx <= 5; ndx++)
            {
                if (LDetect[ndx].IntersectsWith(hitbox)) // have to be entirely in the shadow
                    CharBounds = Rectangle.Intersect(hitbox, LDetect[ndx]);
                else
                    CharBounds = new Rectangle(0, 0, 0, 0);
                if (!CharBounds.IsEmpty)
                {
                             Detect = true;                            
                }

                if (tmrFin.Enabled == false) // so the code doesn't loop when end game.
                {
                    if (Detect == true && Hidden == false)
                    {
                        lbHide.Text = "DETECTED!! ";
                        lbInfo.BackColor = Color.DarkRed;
                        lbInfo.ForeColor = Color.Red;
                        InfoLoc.Y = 10;

                        Stealth = new SolidBrush(Color.FromArgb(70, 150, 0, 0));
                        Win = false;
                        // begin game over
                        tmrFin.Enabled = true;
                    }

                    if (Detect == false && Hidden == false)
                    {
                        lbHide.Text = "undetected ";
                        Stealth = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
                        lbInfo.BackColor = Color.LightSeaGreen;
                        lbInfo.ForeColor = Color.PaleTurquoise;
                    }

                    if (Detect == true && Hidden == true)
                    {
                        lbHide.Text = "HIDDEN!!";
                        Stealth = new SolidBrush(Color.FromArgb(100, 0, 0, 25));
                        lbInfo.BackColor = Color.LightSeaGreen;
                        lbInfo.ForeColor = Color.PaleTurquoise;
                    }

                    if (Detect == false && Hidden == true)
                    {
                        lbHide.Text = "Hidden";
                        Stealth = new SolidBrush(Color.FromArgb(100, 0, 0, 25));
                        lbInfo.BackColor = Color.LightSeaGreen;
                        lbInfo.ForeColor = Color.PaleTurquoise;
                    }
                }
            }

                     Hidden = false; // resets to false if not in shadow
                     Detect = false; // resets to false if not in detection range

            //computer activate/hack
          for (int ndx = 0; ndx <= 1; ndx++)
           {

             if (Comp[ndx].IntersectsWith(hitbox)) // have to be entirely in the shadow
                   CharBounds = Rectangle.Intersect(hitbox, Comp[ndx]);
              else
                 CharBounds = new Rectangle(0, 0, 0, 0);
            if (!CharBounds.IsEmpty)
              {
                  if (Interact == true && ndx == 1)  //computer in secret room de activates trap
                  {
                      Trap = 2;
                      if (lbInfo.Text != "Electric Field Offline")
                          TrapActive();
                  }

                  if (Interact == true && ndx == 0)  //computer in secret room de activates trap
                  {
                      Visable = true;
                      lbInfo.Text = "Wall Detector Found, Electronic Key Found";
                      InfoDisplay.Enabled = true;
                  }
              }
           }

        //open chest
          for (int ndx = 0; ndx <= 1; ndx++)
          {
              if (Chest[ndx].IntersectsWith(hitbox)) // have to be entirely in the shadow
                  CharBounds = Rectangle.Intersect(hitbox, Chest[ndx]);
              else
                  CharBounds = new Rectangle(0, 0, 0, 0);
              if (!CharBounds.IsEmpty)
              {
                  if (Interact == true && ndx == 1)  //computer in secret room de activates trap
                  {
                    //finish game
                      if (Visable == true) //acts as the key since they are both obtained at the same place
                      {
                          lbInfo.Text = "Atifact Found. Escape with Artifact";
                          LChest = new Bitmap(CulminatingGame.Properties.Resources.LargeChestOpen);
                          Artifact = true; //obtained artifact
                      }
                      else
                      {
                          lbInfo.Text = "There seems to be a lock on this one..."; //player doesn't have a key
                      }
                      InfoDisplay.Enabled = true;                    
                  }

                  if (Interact == true && ndx == 0)  //extra loot, was going to implament a score system
                  {
                      lbInfo.Text = "Loot Found";
                      InfoDisplay.Enabled = true;
                  }
              }
          }

            //collisions with walls
            for (int ndx = 0; ndx <= 99; ndx++)
            {
                if (Wall[ndx].IntersectsWith(hitbox))
                    CharBounds = Rectangle.Intersect(hitbox, Wall[ndx]);
                else
                    CharBounds = new Rectangle(0, 0, 0, 0);

                if (!CharBounds.IsEmpty)  
                {
                    if (ndx == 28 || ndx == 29 || ndx == 30 || ndx == 31) //electric field walls
                    {
                        Detect = true; // electricity kills
                    }

                    lbWall.Text = "Wall = " + ndx.ToString() + " Width: " + Wall[ndx].Width.ToString() + " Height: " + Wall[ndx].Height.ToString() + " Location: " + Wall[ndx].Location.ToString();  //for testing and building
                    if ((hitbox.Location.X + hitbox.Width) >= Wall[ndx].Location.X && (hitbox.Location.X + hitbox.Width) < (Wall[ndx].Location.X + 20) && (hitbox.Location.Y + hitbox.Height) > Wall[ndx].Location.Y && hitbox.Location.Y < (Wall[ndx].Location.Y + Wall[ndx].Height))
                    {
                        rightMove = false;
                        if (ndx == 11) //press button to open door
                        {
                            SKey1 = true;
                            lbInfo.Text = "Sectret Switch Found";
                            InfoDisplay.Enabled = true;
                        }
                    }

                    if (hitbox.Location.X <= (Wall[ndx].Location.X + Wall[ndx].Width) && hitbox.Location.X > (Wall[ndx].Location.X + Wall[ndx].Width - 20) && (hitbox.Location.Y + hitbox.Height) > Wall[ndx].Location.Y && hitbox.Location.Y < (Wall[ndx].Location.Y + Wall[ndx].Height))
                    {
                        leftMove = false;
                    }

                    if (hitbox.Location.Y <= (Wall[ndx].Location.Y + Wall[ndx].Height) && hitbox.Location.Y > (Wall[ndx].Location.Y + Wall[ndx].Height - 20) && (hitbox.Location.X + hitbox.Width) > Wall[ndx].Location.Y && hitbox.Location.X < (Wall[ndx].Location.X + Wall[ndx].Width))
                    {
                        upMove = false;
                    }

                    if ((hitbox.Location.Y + hitbox.Height) >= Wall[ndx].Location.Y && (hitbox.Location.Y + hitbox.Height) < (Wall[ndx].Location.Y + 20) && (hitbox.Location.X + hitbox.Width) > Wall[ndx].Location.Y && hitbox.Location.X < (Wall[ndx].Location.X + Wall[ndx].Width))
                    {
                        downMove = false;
                    }
                }

                //secret room open
                if (SKey1 == true)
                {
                    Wall[10].Width -= 1;
                    if (Wall[10].Width <= 1)
                        SKey1 = false;
                }
            }

            for ( int ndx = 0; ndx <= 3; ndx++) //pressure plates interaction and trap activation
            {
                if (Switch[ndx].IntersectsWith(hitbox))
                    CharBounds = Rectangle.Intersect(hitbox, Switch[ndx]);
                else
                    CharBounds = new Rectangle(0, 0, 0, 0);

                if (!CharBounds.IsEmpty)
                {
                    if (ndx == 0)
                    {
                        Trap=1;
                    }

                    if (ndx == 1)
                    {
                        Trap=2;
                    }

                    if (ndx == 2)
                    {
                        Trap = 3;
                    }

                    if (ndx == 3)
                    {
                        Trap = 4;
                    }

                   if( lbInfo.Text != "Electric Field Offline") //if it has been disabled it will not activate the traps (until the player does something to change the text, just pretend the traps reset after a certain amount of time) could have added a boolean but was lazy
                    TrapActive();
                }

            }

            //movement eaach for loop for each array of objects that need to move
           //walls
            for (int ndx = 0; ndx <= 99; ndx++)
            {
                if (leftMove == true)
                {
                    WallLoc[ndx].X += 5;
                }
                if (rightMove == true)
                {
                    WallLoc[ndx].X -= 5;
                }
                if (upMove == true)
                {
                    WallLoc[ndx].Y += 5;
                }
                if (downMove == true)
                {
                    WallLoc[ndx].Y -= 5;
                }
            }

            //hidding spots,light,and guards
            for (int ndx = 0; ndx <= 5; ndx++)
            {
                if (leftMove == true)
                {
                    HideLoc[ndx].X += 5;
                    LightPos[ndx].X += 5;
                    GuardPos[ndx].X += 5;
                }
                if (rightMove == true)
                {
                    HideLoc[ndx].X -= 5;
                    LightPos[ndx].X -= 5;
                    GuardPos[ndx].X -= 5;
                }
                if (upMove == true)
                {
                    HideLoc[ndx].Y += 5;
                    LightPos[ndx].Y += 5;
                    GuardPos[ndx].Y += 5;
                }
                if (downMove == true)
                {
                    HideLoc[ndx].Y -= 5;
                    LightPos[ndx].Y -= 5;
                    GuardPos[ndx].Y -= 5;
                }
            }

            //pressure plates
            for (int ndx = 0; ndx <= 3; ndx++)
            {
                if (leftMove == true)
                {
                    SwitchPos[ndx].X += 5;
                }
                if (rightMove == true)
                {
                    SwitchPos[ndx].X -= 5;
                }
                if (upMove == true)
                {
                    SwitchPos[ndx].Y += 5;
                }
                if (downMove == true)
                {
                    SwitchPos[ndx].Y -= 5;
                }
            }

            //Computers and chest
            for (int ndx = 0; ndx <= 1; ndx++)
            {
                if (leftMove == true)
                {
                    CompLoc[ndx].X += 5;
                    ChestLoc[ndx].X += 5;
                }
                if (rightMove == true)
                {
                    CompLoc[ndx].X -= 5;
                    ChestLoc[ndx].X -= 5;
                }
                if (upMove == true)
                {
                    CompLoc[ndx].Y += 5;
                    ChestLoc[ndx].Y += 5;
                }
                if (downMove == true)
                {
                    CompLoc[ndx].Y -= 5;
                    ChestLoc[ndx].Y -= 5;
                }
            }

            //Screen Position
            if (leftMove == true)
            {               
                ScreenPos.X -= 5;
            }
            if (rightMove == true)
            {               
                ScreenPos.X += 5;
            }
            if (upMove == true)
            {                
                ScreenPos.Y -= 5;
            }
            if (downMove == true)
            {              
                ScreenPos.Y += 5;
            }
        }

        private void TrapActive() //what it says activates different traps
        {
            if (Trap ==1) //die
            {
                if (WallLoc[28].Y >= SwitchPos[1].Y)
                    WallLoc[28].Y = SwitchPos[1].Y;
            }

            if (Trap == 2) //open
            {
                if (WallLoc[28].Y <= WallLoc[27].Y )
                    WallLoc[28].Y += 300;

                if (WallLoc[29].Y <= WallLoc[27].Y)
                    WallLoc[29].Y += 300;

                if (WallLoc[30].Y <= WallLoc[27].Y)
                    WallLoc[30].Y += 300;

                if (WallLoc[31].Y <= WallLoc[27].Y)
                    WallLoc[31].Y += 300;

                lbInfo.Text = "Electric Field Offline";
                InfoDisplay.Enabled = true;
            }

            if (Trap == 3) //trapped
            {
                if (WallLoc[28].Y >= SwitchPos[1].Y+70)
                    WallLoc[28].Y = SwitchPos[1].Y + 70;

                if (WallLoc[29].Y >= SwitchPos[1].Y+70)
                    WallLoc[29].Y = SwitchPos[1].Y + 70;

                if (WallLoc[30].Y > WallLoc[9].Y+100  )
                    WallLoc[30].Y = WallLoc[9].Y + 100;

                if (WallLoc[31].Y > WallLoc[9].Y + 100)
                    WallLoc[31].Y -= WallLoc[9].Y + 100;
                if (WallLoc[31].X < SwitchPos[1].X)// + 100)
                    WallLoc[31].X = SwitchPos[1].X;
            }

            if (Trap == 4)//die
            {
                if(WallLoc[29].Y>=SwitchPos[1].Y)
                    WallLoc[29].Y = SwitchPos[1].Y;
            }
        }
        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            //controls all the player animation
            if (down == true) //downward animation
            {
                if (move >= 2)
                    move = 0;
                srcRect = new Rectangle((15 * move + 47), 0, 15, 20);
                select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                Char = select;
                move++;
                standPose = 1;
            }

            if (up == true) //upward animation
            {
                if (move >= 2)
                    move = 0;
                srcRect = new Rectangle((15 * move + 79), 0, 15, 20);
                select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                Char = select;
                move++;
                standPose = 2;
            }

            if (l == true) //left animation
            {
                if (move >= 2)
                    move = 0;
                srcRect = new Rectangle((15 * move + 110), 0, 15, 20);
                select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                Char = select;
                move++;
                standPose = 3;
            }

            if (l == true && (down == true || up == true)) //left diagonal animation
            {
                if (move >= 2)
                    move = 0;
                srcRect = new Rectangle((15 * move + 110), 0, 15, 20);
                select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                Char = select;
                move++;
                standPose = 3;
            }


            if (r == true)  //Right animation
            {
                if (move >= 2)
                    move = 0;
                srcRect = new Rectangle((15 * move + 110), 0, 15, 20);
                select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                select.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Char = select;
                move++;
                standPose = 4;
            }

            if (r == true && (down == true || up == true)) //Right diagonal animation
            {
                if (move >= 2)
                    move = 0;
                srcRect = new Rectangle((15 * move + 110), 0, 15, 20);
                select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                select.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Char = select;
                move++;
                standPose = 4;
            }

            //standing
            if (down == false && up == false && l == false && r == false)
            {
                if (standPose == 1)
                {
                    srcRect = new Rectangle(0, 0, 15, 20);
                    select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                    Char = select;
                }

                if (standPose == 2)
                {
                    srcRect = new Rectangle(15, 0, 15, 20);
                    select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                    Char = select;
                }

                if (standPose == 3)
                {
                    srcRect = new Rectangle(30, 0, 15, 20);
                    select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                    Char = select;
                }

                if (standPose == 4)
                {
                    srcRect = new Rectangle(30, 0, 15, 20);
                    select = (Bitmap)Player.Clone(srcRect, Player.PixelFormat);
                    select.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    Char = select;
                }
            }
        }

        private void tmrEnAnimate_Tick(object sender, EventArgs e)
        {
            //controls the enemy sprite animations and movement

            //first 4 guards
            for (int ndx = 0; ndx <= 3; ndx++)
            {
                if (GuardPos[ndx].X >= WallLoc[16].X + Wall[16].Width-100)
                {
                    GuardLeft[ndx] = true;
                    GuardRight[ndx] = false;
                }

                if (GuardPos[ndx].X <= WallLoc[15].X + 250)
                {
                    GuardLeft[ndx] = false;
                    GuardRight[ndx] = true;
                }

                if (GuardLeft[ndx] == true) //left animation
                {
                    GuardPos[ndx].X -= 5;
                    if (enmove[ndx] >= 2)
                        enmove[ndx] = 0;
                    EnsrcRect[ndx] = new Rectangle((15 * enmove[ndx] + 110), 0, 15, 20);
                    EnSelect[ndx] = (Bitmap)Police.Clone(EnsrcRect[ndx], Police.PixelFormat);
                    Popo[ndx] = EnSelect[ndx];
                    enmove[ndx]++;
                }

                if (GuardRight[ndx] == true)  //Right animation
                {
                    GuardPos[ndx].X += 5;
                    if (enmove[ndx] >= 2)
                        enmove[ndx] = 0;
                    EnsrcRect[ndx] = new Rectangle((15 * enmove[ndx] + 110), 0, 15, 20);
                    EnSelect[ndx] = (Bitmap)Police.Clone(EnsrcRect[ndx], Police.PixelFormat);
                    EnSelect[ndx].RotateFlip(RotateFlipType.RotateNoneFlipX);
                    Popo[ndx] = EnSelect[ndx];
                    enmove[ndx]++;
                }
            }

            for (int ndx = 4; ndx <= 5; ndx++)
            {
                if (GuardPos[ndx].X >= WallLoc[38].X + Wall[38].Width - 100)
                {
                    GuardLeft[ndx] = true;
                    GuardRight[ndx] = false;
                }

                if (GuardPos[ndx].X <= WallLoc[37].X + 250)
                {
                    GuardLeft[ndx] = false;
                    GuardRight[ndx] = true;
                }

                if (GuardLeft[ndx] == true) //left animation
                {
                    GuardPos[ndx].X -= 5;
                    if (enmove[ndx] >= 2)
                        enmove[ndx] = 0;
                    EnsrcRect[ndx] = new Rectangle((15 * enmove[ndx] + 110), 0, 15, 20);
                    EnSelect[ndx] = (Bitmap)Police.Clone(EnsrcRect[ndx], Police.PixelFormat);
                    Popo[ndx] = EnSelect[ndx];
                    enmove[ndx]++;
                }

                if (GuardRight[ndx] == true)  //Right animation
                {
                    GuardPos[ndx].X += 5;
                    if (enmove[ndx] >= 2)
                        enmove[ndx] = 0;
                    EnsrcRect[ndx] = new Rectangle((15 * enmove[ndx] + 110), 0, 15, 20);
                    EnSelect[ndx] = (Bitmap)Police.Clone(EnsrcRect[ndx], Police.PixelFormat);
                    EnSelect[ndx].RotateFlip(RotateFlipType.RotateNoneFlipX);
                    Popo[ndx] = EnSelect[ndx];
                    enmove[ndx]++;
                }
            }
        }

        private void Clock_Tick(object sender, EventArgs e)
        {
            //loops music when it ends
            clocktime++;
            if (clocktime == 214)
            {
                backtrack.URL = @"Stealthstep.mp3";
                clocktime = 0;
            }
        }

        private void InfoDisplay_Tick(object sender, EventArgs e)
        {
            InfoTmr++;
            //displayes then hides info after a certain amount of time has passed
            if (InfoTmr <= 30)
            {
                InfoLoc.Y += 2;
            }

            if (InfoTmr >= 60)
            {
                InfoLoc.Y -= 2;
            }

            if (InfoLoc.Y<=-50)
            {
                InfoTmr = 0;
                InfoDisplay.Enabled = false;
            }
        }

        private void tmrFin_Tick(object sender, EventArgs e)
        {
            //Reset Code (simply copy and pasted almost everything from form load. * was going to impliment a better way using the screenLoc variable, but this way was faster.
            if (Interact == true)
            {
                //reset
                Reset();
            }

            //end game code  Had to leave some of the timers enabled to avoid a glitch where everything would reset but you would not be able to move
            
            if(Win==true)
                lbInfo.Text = "Congrats on completing your First Mission! press e to play again";
            else
                lbInfo.Text = "GAME OVER, Press e to try again"; // haven't implimented restart code
            InfoDisplay.Enabled = true;
            Stealth=new SolidBrush(Color.FromArgb(100+Fade,0,0,0));
            if (Fade < 150)
            {
                Fade += 5;
            }         
        }

        private void Reset() //what it says resets. was going to use the screen location variable to mathematically adjust the position of everything, would have been less lines but this was faster to do at this point in time
        {
            tmrFin.Enabled = false;
            //setting up resource
            LChest = new Bitmap(CulminatingGame.Properties.Resources.LargeChestClosed);
            SChest = new Bitmap(CulminatingGame.Properties.Resources.SmallChest);

            Win = false;
            Artifact = false;
            SKey1 = false;
            Visable = false;
            Detect = false;
            Hidden = false;
            Fade = 0;

            //boundaries at 90000x90000 
            WallLoc[0].X = -90000;
            WallLoc[0].Y = -90000;
            Wall[0] = new Rectangle(WallLoc[0].X, WallLoc[0].Y, 270000, 90000); //top bounds

            WallLoc[1].X = -90000;
            WallLoc[1].Y = 0;
            Wall[1] = new Rectangle(WallLoc[1].X, WallLoc[1].Y, 90000, 180000);//left bounds

            WallLoc[2].X = 90000;
            WallLoc[2].Y = 0;
            Wall[2] = new Rectangle(WallLoc[2].X, WallLoc[2].Y, 90000, 180000);//right bounds

            WallLoc[3].X = 0;
            WallLoc[3].Y = 90000;
            Wall[3] = new Rectangle(WallLoc[3].X, WallLoc[3].Y, 90000, 90000);//bottom bounds

            //Parts of buildings and structures
            WallLoc[4].X = 0;
            WallLoc[4].Y = 0;
            Wall[4] = new Rectangle(WallLoc[4].X, WallLoc[4].Y, 300, 90000);

            WallLoc[5].X = 0;
            WallLoc[5].Y = 900;
            Wall[5] = new Rectangle(WallLoc[5].X, WallLoc[5].Y, 900, 900);

            WallLoc[9].X = 300;
            WallLoc[9].Y = 1900;
            Wall[9] = new Rectangle(WallLoc[9].X, WallLoc[9].Y, 600, 100);

            WallLoc[10].X = 300;
            WallLoc[10].Y = 1700;
            Wall[10] = new Rectangle(WallLoc[10].X, WallLoc[10].Y, 600, 200);

            WallLoc[12].X = 300;
            WallLoc[12].Y = 1700;
            Wall[12] = new Rectangle(WallLoc[12].X, WallLoc[12].Y, 600, 100);

            WallLoc[11].X = 1000;
            WallLoc[11].Y = 1800;
            Wall[11] = new Rectangle(WallLoc[11].X, WallLoc[11].Y, 10, 50); //secret key

            WallLoc[8].X = 1000;
            WallLoc[8].Y = 900;
            Wall[8] = new Rectangle(WallLoc[8].X, WallLoc[8].Y, 100, 1600);

            WallLoc[6].X = 0;
            WallLoc[6].Y = 0;
            Wall[6] = new Rectangle(WallLoc[6].X, WallLoc[6].Y, 90000, 300);

            WallLoc[7].X = 0;
            WallLoc[7].Y = 175;
            Wall[7] = new Rectangle(WallLoc[7].X, WallLoc[7].Y, 90000, 125);

            WallLoc[13].X = 1000;
            WallLoc[13].Y = 0;
            Wall[13] = new Rectangle(WallLoc[8].X, WallLoc[8].Y, 1000, 800);

            WallLoc[14].X = 1000;
            WallLoc[14].Y = 900;
            Wall[14] = new Rectangle(WallLoc[14].X, WallLoc[14].Y, 900, 150);

            WallLoc[15].X = 1000;
            WallLoc[15].Y = 1000;
            Wall[15] = new Rectangle(WallLoc[15].X, WallLoc[15].Y, 450, 100);

            HideLoc[0].X = 1450;
            HideLoc[0].Y = 1000;
            HideSpot[0] = new Rectangle(HideSpot[0].X, HideSpot[0].Y, 100, 100);

            WallLoc[16].X = 1550;
            WallLoc[16].Y = 1000;
            Wall[16] = new Rectangle(WallLoc[16].X, WallLoc[16].Y, 350, 100);

            WallLoc[17].X = 2000;
            WallLoc[17].Y = 0;
            Wall[17] = new Rectangle(WallLoc[16].X, WallLoc[16].Y, 100, 2200);

            WallLoc[18].X = 1300;
            WallLoc[18].Y = 1300;
            Wall[18] = new Rectangle(WallLoc[18].X, WallLoc[18].Y, 700, 150);

            WallLoc[19].X = 1100;
            WallLoc[19].Y = 1700;
            Wall[19] = new Rectangle(WallLoc[19].X, WallLoc[19].Y, 700, 150);

            WallLoc[20].X = 1300;
            WallLoc[20].Y = 1400;
            Wall[20] = new Rectangle(WallLoc[20].X, WallLoc[20].Y, 250, 100);

            HideLoc[1].X = 1550;
            HideLoc[1].Y = 1400;
            HideSpot[1] = new Rectangle(HideSpot[1].X, HideSpot[1].Y, 100, 100);

            WallLoc[21].X = 1650;
            WallLoc[21].Y = 1400;
            Wall[21] = new Rectangle(WallLoc[21].X, WallLoc[21].Y, 400, 100);

            WallLoc[22].X = 1000;
            WallLoc[22].Y = 1800;
            Wall[22] = new Rectangle(WallLoc[22].X, WallLoc[22].Y, 450, 100);

            HideLoc[2].X = 1450;
            HideLoc[2].Y = 1800;
            HideSpot[2] = new Rectangle(HideSpot[2].X, HideSpot[2].Y, 100, 100);

            WallLoc[23].X = 1550;
            WallLoc[23].Y = 1800;
            Wall[23] = new Rectangle(WallLoc[23].X, WallLoc[23].Y, 250, 100);

            WallLoc[24].X = 1300;
            WallLoc[24].Y = 2100;
            Wall[24] = new Rectangle(WallLoc[23].X, WallLoc[23].Y, 700, 150);

            WallLoc[25].X = 1650;
            WallLoc[25].Y = 2200;
            Wall[25] = new Rectangle(WallLoc[25].X, WallLoc[25].Y, 450, 100);

            HideLoc[3].X = 1550;
            HideLoc[3].Y = 2200;
            HideSpot[3] = new Rectangle(HideSpot[3].X, HideSpot[3].Y, 100, 100);

            WallLoc[26].X = 1300;
            WallLoc[26].Y = 2200;
            Wall[26] = new Rectangle(WallLoc[26].X, WallLoc[26].Y, 250, 100);

            WallLoc[27].X = 0;
            WallLoc[27].Y = 2500;
            Wall[27] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 6000, 1000);

            WallLoc[32].X = 4000;
            WallLoc[32].Y = 350;
            Wall[32] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 100, 6000);

            WallLoc[33].X = 4010;
            WallLoc[33].Y = 600;
            Wall[33] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 6000, 6000);

            WallLoc[34].X = 4500;
            WallLoc[34].Y = 0;
            Wall[34] = new Rectangle(WallLoc[27].X, WallLoc[27].Y, 6000, 6000);

            //floor switches that activate certain parts of the puzzle
            SwitchPos[0].X = 350;
            SwitchPos[0].Y = 2050;
            Switch[0] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[1].X = 450;
            SwitchPos[1].Y = 2050;
            Switch[1] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[2].X = 550;
            SwitchPos[2].Y = 2050;
            Switch[2] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[3].X = 650;
            SwitchPos[3].Y = 2050;
            Switch[3] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);

            SwitchPos[4].X = 650;
            SwitchPos[4].Y = 2050;
            Switch[4] = new Rectangle(SwitchPos[0].X, SwitchPos[0].Y, 50, 50);


            //puzzle walls
            WallLoc[28].X = 350;
            WallLoc[28].Y = 2250;
            Wall[28] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 200, 50); //draw in different colour for moving wall puzzle

            WallLoc[29].X = 550;
            WallLoc[29].Y = 2250;
            Wall[29] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 200, 50); //draw in different colour for moving wall puzzle

            WallLoc[30].X = 700;
            WallLoc[30].Y = 2300;
            Wall[30] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 50, 200); //draw in different colour for moving wall puzzle

            WallLoc[31].X = 350;
            WallLoc[31].Y = 2300;
            Wall[31] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 50, 200); //draw in different colour for moving wall puzzle

            //invisible "maze" walls (will not be drawn but will be there)
            WallLoc[35].X = 2200;
            WallLoc[35].Y = 400;
            Wall[35] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 100, 1900);

            WallLoc[36].X = 2200;
            WallLoc[36].Y = 400;
            Wall[36] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 1800, 100);

            WallLoc[37].X = 2200;
            WallLoc[37].Y = 2200;
            Wall[37] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 800, 100);

            WallLoc[38].X = 3200;
            WallLoc[38].Y = 2200;
            Wall[38] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 800, 100);

            WallLoc[39].X = 2600;
            WallLoc[39].Y = 1900;
            Wall[39] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 1400, 100);

            WallLoc[40].X = 2200;
            WallLoc[40].Y = 1900;
            Wall[40] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 200, 100);

            WallLoc[41].X = 3500;
            WallLoc[41].Y = 1600;
            Wall[41] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 500, 100);

            WallLoc[42].X = 2200;
            WallLoc[42].Y = 1600;
            Wall[42] = new Rectangle(WallLoc[28].X, WallLoc[28].Y, 1100, 100);

            //chest locations
            ChestLoc[0].X = 525;
            ChestLoc[0].Y = 2400;
            Chest[0] = new Rectangle(ChestLoc[0].X, ChestLoc[0].Y, 50, 50);

            ChestLoc[1].X = 3025;
            ChestLoc[1].Y = 600;
            Chest[1] = new Rectangle(ChestLoc[1].X, ChestLoc[1].Y, 175, 100);

            //guard start positions
            GuardPos[0].X = 1800;
            GuardPos[0].Y = 1150;
            LightPos[0].X = GuardPos[0].X - LSize.Width / 2 + (Popo[0].Width * 3 / 2);
            LightPos[0].Y = GuardPos[0].Y - LSize.Height / 2 + (Popo[0].Height * 3 / 2);
            Light[0] = new Rectangle(LightPos[0], LSize);
            LDetect[0] = new Rectangle(LightPos[0].X, LightPos[0].Y, Light[0].Width / 2, 2000);

            GuardPos[1].X = 1250;
            GuardPos[1].Y = 1550;
            LightPos[1].X = GuardPos[1].X - LSize.Width / 2 + (Popo[1].Width * 3 / 2);
            LightPos[1].Y = GuardPos[1].Y - LSize.Height / 2 + (Popo[1].Height * 3 / 2);
            Light[1] = new Rectangle(LightPos[1], LSize);

            GuardPos[2].X = 1800;
            GuardPos[2].Y = 1950;
            LightPos[2].X = GuardPos[2].X - LSize.Width / 2 + (Popo[2].Width * 3 / 2);
            LightPos[2].Y = GuardPos[2].Y - LSize.Height / 2 + (Popo[2].Height * 3 / 2);
            Light[2] = new Rectangle(LightPos[2], LSize);

            GuardPos[3].X = 1250;
            GuardPos[3].Y = 2350;
            LightPos[3].X = GuardPos[3].X - LSize.Width / 2 + (Popo[3].Width * 3 / 2);
            LightPos[3].Y = GuardPos[3].Y - LSize.Height / 2 + (Popo[3].Height * 3 / 2);
            Light[3] = new Rectangle(LightPos[3], LSize);

            GuardPos[4].X = 2200;
            GuardPos[4].Y = 2050;
            LightPos[4].X = GuardPos[4].X - LSize.Width / 2 + (Popo[4].Width * 3 / 2);
            LightPos[4].Y = GuardPos[4].Y - LSize.Height / 2 + (Popo[4].Height * 3 / 2);
            Light[4] = new Rectangle(LightPos[4], LSize);

            GuardPos[5].X = 4000;
            GuardPos[5].Y = 1750;
            LightPos[5].X = GuardPos[5].X - LSize.Width / 2 + (Popo[5].Width * 3 / 2);
            LightPos[5].Y = GuardPos[5].Y - LSize.Height / 2 + (Popo[5].Height * 3 / 2);
            Light[5] = new Rectangle(LightPos[5], LSize);

            //computer locations
            CompLoc[0].X = 4400;
            CompLoc[0].Y = 225;
            Comp[0] = new Rectangle(CompLoc[0].X, CompLoc[0].Y, 100, 100);

            CompLoc[1].X = 300;
            CompLoc[1].Y = 1725;
            Comp[1] = new Rectangle(CompLoc[1].X, CompLoc[1].Y, 100, 100);

            //
            ScreenPos.X = 0;
            ScreenPos.Y = 0;
            //re-enable timers
            timerRefresh.Enabled = true;
            tmrMove.Enabled = true;
            tmrEnAnimate.Enabled = true;
        }
    }
}