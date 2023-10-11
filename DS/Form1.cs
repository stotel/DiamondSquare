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

namespace DS
{
    public partial class Form1 : Form
    {
        private static int DIMENSION = 1024;
        private static int worldsiseParam = 9;
        private static int WORLDSIZE = (int)Math.Pow(2, worldsiseParam);
        private float ROUGHNESS = 5f;
        public int iterationNumber = 0;
        public static Random random = new Random();
        public Node[,] World;
        public List<Node> midpointLine = new List<Node>() { new Node(0, 0, 512.0f), new Node(DIMENSION, 0, 512.0f) };
        public Form1()
        {
            InitializeComponent();
        }

        private void display(object sender, SKPaintGLSurfaceEventArgs e)
        {
            InitWorld();
            generateLandscape(sender, e);
            int numOfSquares = (int)Math.Pow(2, (10 - worldsiseParam));
            for (int x = 0; x < numOfSquares; x++)
            {
                for (int y = 0; y < numOfSquares; y++)
                {
                    visualiseWorld((x*DIMENSION/numOfSquares), (y * DIMENSION / numOfSquares), sender, e);
                }
            }
            //visualiseWorld(X,Y,sender, e);
        }
        //Sets up the initial 4 points for the world corners
        private void InitWorld()
        {
            World = new Node[WORLDSIZE + 1, WORLDSIZE + 1];
            World[0, 0] = new Node(0, 0, 512.0f);
            World[0, WORLDSIZE] = new Node(0, WORLDSIZE, 512.0f);
            World[WORLDSIZE, 0] = new Node(WORLDSIZE, 0, 512.0f);
            World[WORLDSIZE, WORLDSIZE] = new Node(WORLDSIZE, WORLDSIZE, 512.0f);
        }
        //the main function
        private void generateLandscape(object sender, SKPaintGLSurfaceEventArgs e)
        {
            //i equals depth
            for (int i = 1; i <= worldsiseParam; i++)
            {
                //get the middles
                int step = WORLDSIZE;
                step = step / (int)Math.Pow(2, i);
                int numberOfSteps = WORLDSIZE / step;
                byte color = (byte)(200 * i / worldsiseParam);
                loopThroughCenters(numberOfSteps, step);
                loopThroughMiddles(numberOfSteps, step);
            }
        }
        private void loopThroughMiddles(int numberOfSteps, int step)
        {
            //Debug.WriteLine(boundInt(-1, numberOfSteps));
            //Debug.WriteLine(boundInt(5, numberOfSteps));
            //Debug.WriteLine(boundInt(1, numberOfSteps));
            for (int y = 0; y < numberOfSteps + 1; y++)
            {
                for (int x = 0; x < numberOfSteps + 1; x++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        float Point1Z = World[boundInt(x - 1, numberOfSteps) * step, (y) * step].height;
                        float Point2Z = World[boundInt(x + 1, numberOfSteps) * step, (y) * step].height;
                        float Point3Z = World[(x) * step, boundInt(y + 1, numberOfSteps) * step].height;
                        float Point4Z = World[(x) * step, boundInt(y - 1, numberOfSteps) * step].height;
                        World[x * step, y * step] = new Node
                        (x * step,
                        y * step,
                        (float)(((Point1Z + Point2Z +
                        Point3Z + Point4Z)
                        / 4) + ROUGHNESS * step * (Math.Pow(random.NextDouble(), 2) - 0.5))
                        );
                        /*World[x * step, y * step] = new Node
                        (x * step,
                        y * step,
                        (float)(((Point1Z + Point2Z +
                        Point3Z + Point4Z)
                        / 4) + ROUGHNESS * step * (Math.Pow(random.NextDouble(), 2) - 0.5))
                        );*/
                    }
                }
            }
            int boundInt(int x, int numberOfSteps_)
            {
                if (x < 0)
                {
                    return numberOfSteps_+x;
                }
                else if (x > numberOfSteps_)
                {
                    return x-numberOfSteps_;
                }
                else
                {
                    return x;
                }
            }
        }
        //generate centrs
        private void loopThroughCenters(int numberOfSteps, int step)
        {
            for (int y = 1; y < numberOfSteps; y += 2)
            {
                for (int x = 1; x < numberOfSteps; x += 2)
                {
                    World[x * step, y * step] = new Node
                        (x * step,
                        y * step,
                        (float)(((World[(x - 1) * step, (y - 1) * step].height + World[(x + 1) * step, (y + 1) * step].height +
                        World[(x - 1) * step, (y + 1) * step].height + World[(x + 1) * step, (y - 1) * step].height)
                        / 4) + ROUGHNESS * step * (Math.Pow(random.NextDouble(), 2) - 0.5))
                        );
                }
            }
        }
        private void visualiseWorld(int X, int Y, object sender, SKPaintGLSurfaceEventArgs e)
        {
            byte color = (byte)(0);
            for (int y = 1; y < WORLDSIZE; y++)
            {
                for (int x = 1; x < WORLDSIZE; x++)
                {
                    if (World[y, x].height < 202)
                    {
                        color = (byte)(Math.Max(255 * World[y, x].height / 1024, 0) + 100); ;
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x+X-1, y+Y-1),
                        new SKColor(0, 0, color));
                    }
                    else
                    {
                        color = (byte)(Math.Max(255 * World[y, x].height / 1024, 0));
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x+X-1, y+Y-1),
                        new SKColor(color, color, color));
                    }
                }
            }
        }
        private void Do1Iteration_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}
