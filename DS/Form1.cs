//Rublevsky O O
/*
 *Eventually this will become a compleate Dimond-Square algoruthm implementation
 *but this version is only implementing midpoint generation , now in 2d
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

namespace DS
{
    public partial class Form1 : Form
    {
        private static int DIMENSION = 1024;
        private static int worldsize_param =10;
        private static int WORLDSIZE = (int)Math.Pow(2,worldsize_param);
        private float ROUGHNESS = 0.5f;
        public int iterationNumber = 0;
        public static Random random = new Random();
        public Node[,] World;
        public List<Node> midpointLine = new List<Node>() { new Node(0, 0, 512.0f), new Node(DIMENSION, 0, 512.0f) };
        public Form1()
        {
            InitializeComponent();
        }

        private void display(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            //e.Surface.Canvas.Clear();
            //e.Surface.Canvas.DrawRect(DIMENSION, 0, 3, DIMENSION, new SKPaint
            //{
            //    Color = new SKColor(255, 0, 0)
            //});
            InitWorld();
            generateLandscape(sender,e);
            //subdivide(sender,e);
        }
        //Sets up the initial 4 points for the world corners
        private void InitWorld()
        {
            World = new Node[WORLDSIZE+1, WORLDSIZE+1];
            World[0, 0] = new Node(0, 0, 512.0f);
            World[0, WORLDSIZE] = new Node(0, WORLDSIZE, 512.0f);
            World[WORLDSIZE, 0] = new Node(WORLDSIZE, 0, 512.0f);
            World[WORLDSIZE, WORLDSIZE] = new Node(WORLDSIZE, WORLDSIZE, 512.0f);
        }
        //the main function
        private void generateLandscape(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            for (int i = 1; i <= worldsize_param; i++)
            {
                int step = WORLDSIZE;
                step = step / (int)Math.Pow(2, i);
                int numberOfSteps = WORLDSIZE/ step;
                byte color = (byte)(random.NextDouble() * 255);
                for (int y = 0; y < numberOfSteps+1; y++)
                {
                    for (int x = 0; x < numberOfSteps+1; x++) 
                    {   
                        if ((x+y)%2 != 0)
                        {
                            //e.Surface.Canvas.DrawRect(x*step, y*step,5,5, new SKPaint { Color = new SKColor(color, color, color) });
                            e.Surface.Canvas.DrawPoint 
                                (new SKPoint(x*step,y*step),
                                new SKColor(color, 0, 0));
                        }
                    }
                }
                for (int y = 0; y < numberOfSteps + 1; y++)
                {
                    for (int x = 0; x < numberOfSteps + 1; x++)
                    {
                        if ((x + y) % 2 == 0)
                        {
                            //e.Surface.Canvas.DrawRect(x*step, y*step,5,5, new SKPaint { Color = new SKColor(color, color, color) });
                            e.Surface.Canvas.DrawPoint
                                (new SKPoint(x * step, y * step),
                                new SKColor(0, color, 0));
                        }
                    }
                }

            }
        }
        //Step 1 : generate wall middles
        /*private void subdivide(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            int segmentLen = DIMENSION / midpointLine.Count;
            List<Node> midpoints = new List<Node>();
            Node midpoint;
            for (int i = 0; i < midpointLine.Count-1; i++)
            {
                midpoint = new Node(
                    (midpointLine[i].x + midpointLine[i+1].x) / 2,//new x
                    0,//new y
                    (float)((midpointLine[i].height + midpointLine[i + 1].height) / 2 + ROUGHNESS * segmentLen * (random.NextDouble() - 0.5)));//new z
                midpoints.Add(midpoint);
            }
            for (int i = 0; i<midpoints.Count;i++)
            {
                midpointLine.Insert(2*i+1,midpoints[i]);
            }
            Debug.WriteLine(midpointLine.Count);
            int segmentDrawingLen = (DIMENSION / midpointLine.Count)+1;
            byte color;
            for (int i = 0;i < midpointLine.Count; i++)
            {
                color = (byte)(midpointLine[i].height * 255 / DIMENSION);
                e.Surface.Canvas.DrawRect(i * segmentDrawingLen, DIMENSION - midpointLine[i].height, segmentDrawingLen, midpointLine[i].height, new SKPaint
                {
                    Color = new SKColor(color, color, color)
                });
            }
        }*/
    private void Do1Iteration_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}
