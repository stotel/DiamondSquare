using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS
{
    public class Moover
    {
        public static int PosX;
        public static int PosY;
        public Moover(int X, int Y) 
        {
            PosX = X;
            PosY = Y;
        }
        public static void Moove(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    PosY += 50;
                    break;
                case Keys.A:
                    PosX += 50;
                    break;
                case Keys.S:
                    PosY -= 50;
                    break;
                case Keys.D:
                    PosX -= 50;
                    break;
            }
        }
    }
}
