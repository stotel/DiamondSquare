using OpenTK.Graphics.ES20;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS
{
    public partial class Form1 : Form
    {
        private static int dimension = 1025;
        public int iterationNumber = 0;
        public static  Random random = new Random(0);
        private List<List<Node>> nodes = new List<List<Node>>() { 
            new List<Node>() { new Node(0, 0, (float)random.NextDouble()), new Node(dimension, 0, (float)random.NextDouble()) },
            new List<Node>() { new Node(0, dimension, (float)random.NextDouble()), new Node(dimension, dimension, (float)random.NextDouble()) } 
        };//Initial 4 points
        private float roughness = 1.5f;

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
        private void startingConditions(List<List<Node>> nodes)
        {
            nodes.Add(new List<Node>());
            nodes.Add(new List<Node>());//new Node(0, 0, (float)random.NextDouble()), new Node(dimension, 0, (float)random.NextDouble())
            nodes[0].Add(new Node(0, 0, (float)random.NextDouble()));
            nodes[0].Add(new Node(0, 0, (float)random.NextDouble()));
        }
        private void subdivide(List<List<Node>> field)
        {

        }

        private void Do1Iteration_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}
