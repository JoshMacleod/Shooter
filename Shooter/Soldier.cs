using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shooter.Properties;
using System.Drawing;

namespace Shooter
{
    class Soldier : CImageBase
    {
        private Rectangle soldierHotSpot = new Rectangle(); //creating a rectangle for the soldier's position (hitbox) with size defined below

        public Soldier() : base(Resources.Character)
        {
            soldierHotSpot.X = Left;
            soldierHotSpot.Y = Top;
            soldierHotSpot.Width = 203;
            soldierHotSpot.Height = 380;
        }

        public void Update(int X, int Y)
        {
            Left = X;
            Top = Y;
            soldierHotSpot.X = Left;
            soldierHotSpot.Y = Top;
        }

        public bool Hit(int X, int Y)
        {
            Rectangle c = new Rectangle(X, Y, 1, 1); //creates a 1x1 rectangle as the mouse pointer's position

            if (soldierHotSpot.Contains(c)) //is the rectangle (i.e. mouse pointer) in the same position 
            {                               //as the soldier? (has the soldier been hit?)
                return true;
            }
            return false;
        }
    }
}
