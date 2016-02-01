//#define MyDebug //- used to contain code that can be commented out all at once, e.g. for debugging
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shooter.Properties;
using System.Media;
using System.IO;
using System.Xml.Serialization;

namespace Shooter
{
    public struct IconInfo
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hmbColour;
    }

    public partial class Shooter : Form
    {
#if MyDebug
        int cursX = 0;      //debug code
        int cursY = 0;
#endif

        Soldier soldier;
        Splat splat;
        ScoreFrame scoreFrame;
        Sign sign;
        Random rnd = new Random();
        int gameFrame = 0;
        int splatTime = 0;
        bool _splat = false;
        int hits = 0;
        int misses = 0;
        int totalShots = 0;
        double avgHits = 0;

        const int constSplatTime = 3;

        int level = 5;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public Shooter()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);                //stops background refreshing
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            soldier = new Soldier() { Left = 500, Top = 200 };
            splat = new Splat() { };
            sign = new Sign() { Left = 1090, Top = 10 };
            scoreFrame = new ScoreFrame() { Left = 10, Top = 10 };
            Bitmap newCur = new Bitmap(Resources.Crosshairs);
            this.Cursor = CustomCursor.CreateCursor(newCur, newCur.Height / 2, newCur.Width / 2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_splat)
            {
                if (splatTime >= constSplatTime)
                {
                    _splat = false;
                    splatTime = 0;
                    UpdateSoldier();
                }
                splatTime++;
            }
            if (gameFrame >= Level)
            {
                UpdateSoldier();
                gameFrame = 0;
            }
            gameFrame++;
            this.Refresh();
        }

        private void Shooter_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e) //used for drawing graphics on screen
        {
            Graphics dc = e.Graphics;
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
            Font font = new System.Drawing.Font("Stencil", 14, FontStyle.Regular);

            sign.DrawImage(dc);
            scoreFrame.DrawImage(dc);

            TextRenderer.DrawText(e.Graphics, "Shots: " + totalShots.ToString(), font, new Rectangle(25, 25, 120, 45), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Hits: " + hits.ToString(), font, new Rectangle(25, 70, 120, 45), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Misses: " + misses.ToString(), font, new Rectangle(25, 115, 120, 45), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Avg: " + avgHits.ToString() + "%", font, new Rectangle(25, 160, 120, 45), SystemColors.ControlText, flags);

#if MyDebug
            TextRenderer.DrawText(dc, "X=" + cursX.ToString() + ":" + "Y=" + cursY.ToString(), font,
                new Rectangle(200, 200, 120, 20), SystemColors.ControlText, flags);
#endif

            base.OnPaint(e);

            if (_splat == true)
            {
                splat.DrawImage(dc);
            }
            else
            {
                soldier.DrawImage(dc);
            }
        }

        private void Shooter_MouseMove(object sender, MouseEventArgs e) //used to detect and store cursor position
        {
#if MyDebug
            cursX = e.X;
            cursY = e.Y;
#endif
            this.Refresh();
        }

        private void Shooter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 1097 && e.X < 1244 && e.Y >= 17 && e.Y < 68) //start
            {
                timerGameLoop.Start();
            }
            if (e.X > 1096 && e.X < 1244 && e.Y >= 68 && e.Y < 116) //stop
            {
                timerGameLoop.Stop();
            }
            if (e.X > 1096 && e.X < 1244 && e.Y >= 116 && e.Y < 166) //reset
            {
                timerGameLoop.Stop();
                hits = 0;
                misses = 0;
                totalShots = 0;
                avgHits = 0;
            }
            if (e.X > 1096 && e.X < 1244 && e.Y >= 166 && e.Y < 214) //quit
            {
                timerGameLoop.Stop();
                Environment.Exit(0);
            }
            else
            {
                //FireGun();
                    if (soldier.Hit(e.X, e.Y))
                    {
                        _splat = true;
                        splat.Left = soldier.Left - Resources.Splat.Width / 3;
                        splat.Top = soldier.Top - Resources.Splat.Height / 3;
                        hits++;
                    }
                    else
                    {
                        misses++;
                    }
                    totalShots++;
                avgHits = (int)(0.5f + ((100f * hits) / totalShots));
            }
        }

        //private void FireGun()
        //{
        //    SoundPlayer simpleSound = new SoundPlayer(Resources.Gunshot);
        //    simpleSound.Play();
        //}

        private void UpdateSoldier()
        {
            soldier.Update(rnd.Next(80, 1080), rnd.Next(120, 480)); //calculated values
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GameData info = new GameData();
                info.hits = hits;
                info.misses = misses;
                info.totalShots = totalShots;
                info.avgHits = avgHits;
                SaveLoad.SaveData(info, "data.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists("data.xml"))
            {
                GameData info = new GameData();
                info = SaveLoad.LoadData("data.xml");
                hits = info.hits;
                misses = info.misses;
                totalShots = info.totalShots;
                avgHits = info.avgHits;
            }
            else
            {
                MessageBox.Show("File does not exist");
            }
        }
    }
}
