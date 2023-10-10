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
        private float ROUGHNESS = 1.5f;
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
            InitWorld();
            generateLandscape(sender,e);
            visualiseWorld(sender,e);
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
            //i equals depth
            for (int i = 1; i <= worldsize_param; i++)
            {   
                //get the middles
                int step = WORLDSIZE;
                step = step / (int)Math.Pow(2, i);
                int numberOfSteps = WORLDSIZE/ step;
                byte color = (byte)(200*i/worldsize_param);
                for (int y = 0; y < numberOfSteps+1; y++)
                {
                    for (int x = 0; x < numberOfSteps+1; x++) 
                    {   
                        if ((x+y)%2 != 0)
                        {
                            if (y%2 == 0)
                                World[x*step, y*step] = new Node
                                    (
                                    x * step,
                                    y * step,
                                    (float)(((World[(x-1) * step, y * step].height+ World[(x + 1) * step, y * step].height)
                                    /2) + ROUGHNESS * step * (Math.Pow(random.NextDouble(), 2) - 0.5))
                                    );
                            else
                            {
                                World[x * step, y * step] = new Node
                                    (
                                    x * step,
                                    y * step,
                                    (float)(((World[x * step, (y-1) * step].height + World[x * step, (y+1) * step].height)
                                    / 2) + ROUGHNESS * step * (Math.Pow(random.NextDouble(), 2) - 0.5))
                                    );
                            }
                        }
                    }
                }
                //get the centr
                for (int y = 1; y < numberOfSteps; y+=2)
                {
                    for (int x = 1; x < numberOfSteps; x+=2)
                    {
                        World[x * step, y * step] = new Node
                            (x * step,
                            y * step,
                            (float)(((World[x * step, (y - 1) * step].height + World[x * step, (y + 1) * step].height +
                            World[(x - 1) * step, y * step].height + World[(x + 1) * step, y * step].height)
                            / 4) + ROUGHNESS * step * (Math.Pow(random.NextDouble(),2) - 0.5))
                            );
                    }
                }

            }
        }
        private void visualiseWorld(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            byte color = (byte)(0);
            for (int y = 1;y < WORLDSIZE;y++)
            {
                for (int x = 1; x < WORLDSIZE; x++)
                {   
                    if(World[y, x].height < 202)
                    {
                        color = (byte)(Math.Max(255 * World[y, x].height / 1024, 0)+100); ;
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x, y),
                        new SKColor(0, 0, color));
                    }
                    else
                    {
                        color = (byte)(Math.Max(255 * World[y, x].height / 1024, 0));
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x, y),
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
