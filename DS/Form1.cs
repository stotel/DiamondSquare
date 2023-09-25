using OpenTK.Graphics.ES20;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS
{
    public partial class Form1 : Form
    {
        public int iterationNumber = 0;
        public Random random = new Random(0);
        public List<int[]> cubes = new List<int[]>();
        public Form1()
        {
            InitializeComponent();
        }

        private void display(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            e.Surface.Canvas.DrawRect(0, 0, 100, 100, new SKPaint
            {
                Color = new SKColor((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255))
            }) ;
        }

        private void Do1Iteration_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}
