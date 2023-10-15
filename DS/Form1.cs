//Rublevsky O O
/*
 *Eventually this will become a compleate Dimond-Square algoruthm implementation
 *And thats it! im going to do it rn
*/

using OpenTK.Graphics.ES20;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp.Views.Desktop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace DS
{
    public partial class Form1 : Form
    {
        private static int DIMENSION = 1024;
        private float ROUGHNESS = 2f;
        public static Random random = new Random(0);
        public const int randInt = 0;
        public List<LogicalChunk> logicalChunks = new List<LogicalChunk>();
        public Form1()
        {
            InitializeComponent();
        }

        private void display(object sender, SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            random = new Random(0);
            World w = new World();
            for (int i = 3; i < 29; i++)
            {
                for (int j = 3; j < 29; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 32, j * 32, 5, ROUGHNESS, random.Next(2147483647));
                    chunk.InitChunk(w);
                    chunk.generateLandscape(w);
                }
            }
            w.visualiseWorld(sender,e);
        }
        private void Do1Iteration_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}
