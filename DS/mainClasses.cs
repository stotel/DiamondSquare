using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
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
        public List<LogicalChunk[]> logicalChunksConnections = new List<LogicalChunk[]>();
        public List<PhysicalChunk> PhysicalChunks = new List<PhysicalChunk>();
        public World()
        {
        }
        public void initWorld(LogicalChunk Origin)
        {
            logicalChunks.Add(Origin);
            logicalChunksConnections.Add(new LogicalChunk[4]);
            logicalChunksConnections[0][0] = new LogicalChunk(Origin.posX + 1, Origin.posY, LogicalChunk.chunkSizeParam, Origin.ROUGHNESS, Origin.randInt);
            logicalChunksConnections[0][1] = new LogicalChunk(Origin.posX, Origin.posY + 1, LogicalChunk.chunkSizeParam, Origin.ROUGHNESS, Origin.randInt);
            logicalChunksConnections[0][2] = new LogicalChunk(Origin.posX - 1, Origin.posY, LogicalChunk.chunkSizeParam, Origin.ROUGHNESS, Origin.randInt);
            logicalChunksConnections[0][3] = new LogicalChunk(Origin.posX, Origin.posY - 1, LogicalChunk.chunkSizeParam, Origin.ROUGHNESS, Origin.randInt);
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
    public class LogicalChunk
    {
        public static int chunkSizeParam;
        private static int CHUNKSIZE;
        public float ROUGHNESS;
        public int posX; public int posY;
        public HeightNode[,] Chunk;
        public Random random;
        public int randInt = 0;
        public bool isGenerated = false;

        public LogicalChunk(int Left_x,int Top_y,int powerOf2ForChunkSize,float roughness,int random_ = 0)
        {
            chunkSizeParam = powerOf2ForChunkSize;
            CHUNKSIZE = (int)Math.Pow(2, chunkSizeParam);
            ROUGHNESS = roughness;
            posX = Left_x; posY = Top_y;
            Chunk = new HeightNode[CHUNKSIZE+1, CHUNKSIZE+1];
            randInt = random_;
            random = new Random(randInt);
        }
        public void generateChunk(object sender, SKPaintGLSurfaceEventArgs e)
        {
            InitChunk();
            generateLandscape();
            visualiseChunk(posX, posY, sender, e);
        }
        private void InitChunk()
        {
            //float a = (float)random.NextDouble();
            float a = 0.5f;
            Chunk[0, 0] = new HeightNode(a);
            Chunk[0, CHUNKSIZE] = new HeightNode(a);
            Chunk[CHUNKSIZE, 0] = new HeightNode(a);
            Chunk[CHUNKSIZE, CHUNKSIZE] = new HeightNode(a);
        }
        private void generateLandscape()
        {
            //i equals depth
            for (int i = 1; i <= chunkSizeParam; i++)
            {
                //get the middles
                int step = CHUNKSIZE;
                step = step / (int)Math.Pow(2, i);
                int numberOfSteps = CHUNKSIZE / step;
                loopThroughCenters(numberOfSteps, step);
                loopThroughMiddles(numberOfSteps, step);
                isGenerated = true;
            }
        }
        private void loopThroughMiddles(int numberOfSteps, int step)
        {
            for (int y = 0; y < numberOfSteps; y++)
            {
                for (int x = 0; x < numberOfSteps; x++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        float Point1Z = Chunk[boundInt(x - 1, numberOfSteps) * step, (y) * step].height;
                        float Point2Z = Chunk[boundInt(x + 1, numberOfSteps) * step, (y) * step].height;
                        float Point3Z = Chunk[(x) * step, boundInt(y + 1, numberOfSteps) * step].height;
                        float Point4Z = Chunk[(x) * step, boundInt(y - 1, numberOfSteps) * step].height;
                        float FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z) / 4) + ROUGHNESS * ((float)step/CHUNKSIZE) * (random.NextDouble() - 0.5));
                        Chunk[x * step, y * step] = new HeightNode(Math.Min(Math.Max(FinalPointZ, 0), 1));
                        if (x == 0)
                        {
                            Chunk[((numberOfSteps) * step), y * step] = new HeightNode(FinalPointZ);
                        }
                        if (y == 0)
                        {
                            Chunk[x * step, ((numberOfSteps) * step)] = new HeightNode(FinalPointZ);
                        }
                    }
                }
            }
            int boundInt(int x, int numberOfSteps_)
            {
                if (x < 0)
                {
                    return numberOfSteps_ + x;
                }
                else if (x > numberOfSteps_)
                {
                    return x - numberOfSteps_;
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
                    float Point1Z = Chunk[(x - 1) * step, (y - 1) * step].height;
                    float Point2Z = Chunk[(x + 1) * step, (y + 1) * step].height;
                    float Point3Z = Chunk[(x - 1) * step, (y + 1) * step].height;
                    float Point4Z = Chunk[(x + 1) * step, (y - 1) * step].height;
                    float FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z) / 4) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                    Chunk[x * step, y * step] = new HeightNode(Math.Min(Math.Max(FinalPointZ,0),1));
                }
            }
        }
        public void visualiseChunk(int X, int Y, object sender, SKPaintGLSurfaceEventArgs e)
        {
            byte color;
            for (int y = 1; y < CHUNKSIZE; y++)
            {
                for (int x = 1; x < CHUNKSIZE; x++)
                {
                    if (Chunk[y, x].height < 0.3f)
                    {
                        color = (byte)(Math.Pow(Chunk[y, x].height,2)*255+100);
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x + X - 1, y + Y - 1),
                        new SKColor(0, 0, color));
                    }
                    else
                    {
                        color = (byte)(Math.Pow(Chunk[y, x].height,2)*255);
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x + X - 1, y + Y - 1),
                        new SKColor(color, color, color));
                    }
                }
            }
        }
    }
    public class PhysicalChunk
    {
        public PhysicalChunk(float Left_x, float Top_y, List<List<HeightNode>> a)
        {
        }
    }
}
