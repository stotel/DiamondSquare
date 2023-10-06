//Rublevsky O O
/*
 *Eventually this will become a compleate Dimond-Square algoruthm implementation
 *but this version is only implementing midpoint generation
*/

using OpenTK.Graphics.ES20;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS
{
    public partial class Form1 : Form
    {
        private static int DIMENSION = 1024;
        private float ROUGHNESS = 1.5f;
        public int iterationNumber = 0;
        public static Random random = new Random(132);
        /*private List<List<Node>> nodes = new List<List<Node>>() { 
            new List<Node>() { new Node(0, 0, (float)random.NextDouble()), new Node(dimension, 0, (float)random.NextDouble()) },
            new List<Node>() { new Node(0, dimension, (float)random.NextDouble()), new Node(dimension, dimension, (float)random.NextDouble()) } 
        };//Initial 4 points
        */
        public List<Node> midpointLine = new List<Node>() { new Node(0, 0, 512.0f), new Node(DIMENSION, 0, 512.0f) };
        public Form1()
        {
            InitializeComponent();
        }

        private void display(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            /*
            e.Surface.Canvas.DrawRect(0, 0, 100, 100, new SKPaint
            {
                Color = new SKColor((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255))
            });*/
            e.Surface.Canvas.DrawRect(DIMENSION, 0, 3, DIMENSION, new SKPaint
            {
                Color = new SKColor(255, 0, 0)
            });
            subdivide(sender,e);
        }
        private void subdivide(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            int segmentLen = DIMENSION / midpointLine.Count;
            List<Node> midpoints = new List<Node>();
            Node midpoint;
            for (int i = 0; i < midpointLine.Count-1; i+=2)
            {
                midpoint = new Node(
                    (midpointLine[i].x + midpointLine[i+1].x) / 2,//new x
                    0,//new y
                    (float)((midpointLine[i].height + midpointLine[i + 1].height) / 2 + ROUGHNESS * segmentLen * (random.NextDouble() - 0.5)));//new z
                midpoints.Add(midpoint);
            }
            List<Node> oldMidpoints = midpointLine;
            for (int i = 0; i<midpoints.Count;i++)
            {
                midpointLine.Insert(2*i+1,midpoints[i]);
            }
            int segmentDrawingLen = DIMENSION / midpointLine.Count;
            byte color;
            for (int i = 0;i < midpointLine.Count; i++)
            {
                color = (byte)(midpointLine[i].height * 255 / DIMENSION);
                e.Surface.Canvas.DrawRect(i * segmentDrawingLen, DIMENSION - midpointLine[i].height, segmentDrawingLen, midpointLine[i].height, new SKPaint
                {
                    Color = new SKColor(color, color, color)
                });
            }
        }
    private void Do1Iteration_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}
