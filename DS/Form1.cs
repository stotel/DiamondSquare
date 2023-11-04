﻿//Rublevsky O O
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
        //private static int DIMENSION = 1024;
        //private float ROUGHNESS = 1f;
        public static Random random = new Random();
        public const int randInt = 0;
        public List<LogicalChunk> logicalChunks = new List<LogicalChunk>();
        World w = new World();
        public Form1()
        {
            InitializeComponent();
        }

        private void display(object sender, SKPaintGLSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            w.visualiseWorld(sender,e,Moover.PosX,Moover.PosY);
            //w.visualizePhysicalChunks(sender, e, Moover.PosX, Moover.PosY);
            //w.visualizeLogicalChunks(sender, e, Moover.PosX, Moover.PosY);
        }
        private void ChunkTest()
        {
            //w = new World();
            /*for (int i = 1; i < 2; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 256 - 128, j * 256 - 128, 8, 4f, random.Next(2147483647));
                    chunk.InitChunk(w);
                    chunk.generateLandscape(w);
                    w.LogicalChunks.Add(chunk);
                }
            }
            for (int i = 4; i < 6; i++)
            {
                for (int j = 2; j < 6; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 128 - 128, j * 128 - 128, 7, 2f, random.Next(2147483647));
                    chunk.InitChunk(w);
                    chunk.generateLandscape(w);
                    w.LogicalChunks.Add(chunk);
                }
            }
            for (int i = 12; i < 15; i++)
            {
                for (int j = 4; j < 12; j++)
                {
                    LogicalChunk chunk = new LogicalChunk(i * 64 - 128, j * 64 - 128, 6, 1f, random.Next(2147483647));
                    chunk.InitChunk(w);
                    chunk.generateLandscape(w);
                    w.LogicalChunks.Add(chunk);
                }
            }*/
            LogicalChunk chunk = new LogicalChunk(512 - 128, 512 - 128, 8, 4f, random.Next(2147483647));
            chunk.InitChunk(w);
            chunk.generateLandscape(w);
            w.LogicalChunks.Add(chunk);
            /*LogicalChunk chunk1 = new LogicalChunk(512, 512-128, 8, 4f, random.Next(2147483647));
            chunk1.InitChunk(w);
            chunk1.generateLandscape(w);
            w.LogicalChunks.Add(chunk1);*/
        }
        private void skglControl1_KeyDown(object sender, KeyEventArgs e)
        {
            Moover.Moove(e);
            skglControl1.Invalidate();
        }

        private void generate_Click(object sender, EventArgs e)
        {
            ChunkTest();
            w.findRenderedPhysicalChunks();
            skglControl1.Invalidate();
        }

        private void expandClick(object sender, EventArgs e)
        {
            w.expandWorldInAllDirections(random);
            w.findRenderedPhysicalChunks();
            skglControl1.Invalidate();
        }
    }
}
