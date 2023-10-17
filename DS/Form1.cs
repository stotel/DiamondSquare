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
        private float ROUGHNESS = 1f;
        public static Random random = new Random();
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
            for (int i = 1; i < 2; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 256-120, j * 256-120, 8, 2f, random.Next(2147483647));
                    chunk.InitChunk(w);
                    chunk.generateLandscape(w);
                }
            }
            for (int i = 4; i < 6; i++)
            {
                for (int j = 2; j < 6; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 128-120, j * 128-120, 7, 2f, random.Next(2147483647));
                    chunk.InitChunk(w);
                    chunk.generateLandscape(w);
                }
            }
            for (int i = 12; i < 15; i++)
            {
                for (int j = 4; j < 12; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 64-120, j * 64-120, 6, 2f, random.Next(2147483647));
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
