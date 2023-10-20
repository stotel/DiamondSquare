using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class World
    {
        public List<LogicalChunk> logicalChunks = new List<LogicalChunk>();
        public List<PhysicalChunk> PhysicalChunks = new List<PhysicalChunk>();
        public Dictionary<(int,int),int> PhysicalChunksDict = new Dictionary<(int, int), int>();

        public World()
        {
        }
        public int ChunkWithCordsIndex(int X,int Y)
        {
            return PhysicalChunksDict.TryGetValue((X, Y), out var result) ? result : -1;
        }
        public float GetHeightByCords(int X,int Y)
        {
            int chunkInd = ChunkWithCordsIndex(X/ PhysicalChunk.SIZE, Y/ PhysicalChunk.SIZE);
            if (chunkInd == -1)
            {
                return 0;
            }
            return (PhysicalChunks[chunkInd].chunk[(X) % PhysicalChunk.SIZE, (Y) % PhysicalChunk.SIZE].height);
        }
        public void visualiseWorld(object sender, SKPaintGLSurfaceEventArgs e)
        {
            byte color;
            for (int i = 0; i < PhysicalChunks.Count; i++)
            {
                for (int j = 0; j < PhysicalChunk.SIZE; j++)
                {
                    for(int k = 0; k < PhysicalChunk.SIZE; k++)
                    {
                        if (PhysicalChunks[i].chunk[k, j].height < 0.3f)
                        {
                            color = (byte)(Math.Pow(PhysicalChunks[i].chunk[k, j].height, 1) * 255 + 100);
                            e.Surface.Canvas.DrawPoint
                            (new SKPoint(PhysicalChunks[i].posX * PhysicalChunk.SIZE + k, PhysicalChunks[i].posY * PhysicalChunk.SIZE + j),
                            new SKColor(0, 0, color));
                        }
                        else
                        {
                            color = (byte)(Math.Pow(PhysicalChunks[i].chunk[k, j].height, 1) * 255 + 50);
                            e.Surface.Canvas.DrawPoint
                            (new SKPoint(PhysicalChunks[i].posX * PhysicalChunk.SIZE + k, PhysicalChunks[i].posY * PhysicalChunk.SIZE + j),
                            new SKColor(color, color, color));
                        }
                    }
                }
            }
        }
    }
    public struct HeightNode
    {
        public float height;
        public HeightNode(float _height)
        {
            height = _height;
        }
    
    }
    public class PhysicalChunk
    {
        public int posX;
        public int posY;
        public const int SIZE = 16;
        public HeightNode[,] chunk  = new HeightNode[SIZE, SIZE];

        public PhysicalChunk(int PosX_, int PosY_)
        {
            posX = PosX_;
            posY = PosY_;
        }
    }
}