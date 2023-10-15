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
        public List<PhysicalChunk> PhysicalChunks = new List<PhysicalChunk>();
        public HeightNode[,] world = new HeightNode[1025,1025];

        public World()
        {
        }
        public void initWorld(LogicalChunk Origin)
        {
            logicalChunks.Add(Origin);
            
        }
        public void visualiseWorld(object sender, SKPaintGLSurfaceEventArgs e)
        {
            byte color;
            for (int y = 1; y < 1025; y++)
            {
                for (int x = 1; x < 1025; x++)
                {
                    if (world[y, x].height < 0.3f)
                    {
                        color = (byte)(Math.Pow(world[y, x].height, 2) * 255 + 100);
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x, y),
                        new SKColor(0, 0, color));
                    }
                    else
                    {
                        color = (byte)(Math.Pow(world[y, x].height, 2) * 255);
                        e.Surface.Canvas.DrawPoint
                        (new SKPoint(x, y),
                        new SKColor(color, color, color));
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
    public class LogicalChunk
    {
        public static int chunkSizeParam;
        public static int CHUNKSIZE;
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
        public void generateChunk(object sender, SKPaintGLSurfaceEventArgs e, World w)
        {
            InitChunk(w);
            generateLandscape(w);
            visualiseChunk(posX, posY, sender, e);
        }
        public void InitChunk(World w)
        {   
            //float a = (float)random.NextDouble();
            //float a = 0.5f;
            Chunk[0, 0] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX,posY,w));
            addToWorld(w, 0, 0);
            Chunk[0, CHUNKSIZE] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX, posY + CHUNKSIZE, w));
            addToWorld(w, 0, CHUNKSIZE);
            Chunk[CHUNKSIZE, 0] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX + CHUNKSIZE, posY, w));
            addToWorld(w, CHUNKSIZE, 0);
            Chunk[CHUNKSIZE, CHUNKSIZE] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX + CHUNKSIZE, posY + CHUNKSIZE, w));
            addToWorld(w, CHUNKSIZE, CHUNKSIZE);
        }
        private float avarageOfSurraundingPointsPlusMinimalRandom(int X,int Y,World w)
        {
            List<float> knownCornerValues = new List<float>();
            if (w.world[X - 1, Y].height != 0)
            {
                knownCornerValues.Add(w.world[X - 1, Y].height);
            }
            if (w.world[X + 1, Y].height != 0)
            {
                knownCornerValues.Add(w.world[X + 1, Y].height);
            }
            if (w.world[X, Y - 1].height != 0)
            {
                knownCornerValues.Add(w.world[X, Y - 1].height);
            }
            if (w.world[X, Y + 1].height != 0)
            {
                knownCornerValues.Add(w.world[X, Y + 1].height);
            }
            if(knownCornerValues.Count == 0)
            {
                return ((float)random.NextDouble());
            }
            return (knownCornerValues.Average() + ((float)random.NextDouble() * 2) / CHUNKSIZE);
        }
        public void generateLandscape(World w)
        {
            //i equals depth
            for (int i = 1; i <= chunkSizeParam; i++)
            {
                //get the middles
                int step = CHUNKSIZE;
                step = step / (int)Math.Pow(2, i);
                int numberOfSteps = CHUNKSIZE / step;
                loopThroughCenters(numberOfSteps, step, w);
                loopThroughMiddles(numberOfSteps, step, w);
                isGenerated = true;
            }
        }
        private void loopThroughMiddles(int numberOfSteps, int step, World w)
        {
            for (int y = 0; y < numberOfSteps +1; y++)
            {
                for (int x = 0; x < numberOfSteps +1; x++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        if (x == 0 && w.world[posX + (x - 1) * step, posY + (y) * step].height == 0)
                        {
                            w.world[posX + (x - 1) * step, posY + (y) * step].height = (float)(((w.world[posX + (x + 1) * step, posY + (y) * step].height + w.world[posX + (x) * step, posY + (y + 1) * step].height + w.world[posX + (x) * step, posY + (y - 1) * step].height) / 3) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                        }
                        else if (y == 0 && w.world[posX + (x) * step, posY + (y - 1) * step].height == 0)
                        {
                            w.world[posX + (x) * step, posY + (y - 1) * step].height = (float)(((w.world[posX + (x + 1) * step, posY + (y) * step].height + w.world[posX + (x) * step, posY + (y + 1) * step].height + w.world[posX + (x - 1) * step, posY + (y) * step].height) / 3) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                        }
                        else if (x == numberOfSteps && w.world[posX + (x + 1) * step, posY + (y) * step].height == 0)
                        {
                            w.world[posX + (x + 1) * step, posY + (y) * step].height = (float)(((w.world[posX + (x - 1) * step, posY + (y) * step].height + w.world[posX + (x) * step, posY + (y + 1) * step].height + w.world[posX + (x) * step, posY + (y - 1) * step].height) / 3) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                        }
                        else if (y == numberOfSteps && w.world[posX + (x) * step, posY + (y + 1) * step].height == 0)
                        {
                            w.world[posX + (x) * step, posY + (y + 1) * step].height = (float)(((w.world[posX + (x + 1) * step, posY + (y) * step].height + w.world[posX + (x) * step, posY + (y - 1) * step].height + w.world[posX + (x - 1) * step, posY + (y) * step].height) / 3) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                        }
                        float Point1Z = w.world[posX + (x - 1) * step, posY + (y) * step].height;
                        float Point2Z = w.world[posX + (x + 1) * step, posY + (y) * step].height;
                        float Point3Z = w.world[posX + (x) * step, posY + (y + 1) * step].height;
                        float Point4Z = w.world[posX + (x) * step, posY + (y - 1) * step].height;
                        float FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z) / 4) + ROUGHNESS * ((float)step/CHUNKSIZE) * (random.NextDouble() - 0.5));
                        Chunk[x * step, y * step] = new HeightNode(Math.Min(Math.Max(FinalPointZ, 0.01f), 0.99f));
                        addToWorld(w, x * step, y * step);
                    }
                }
            }
            /*int boundInt(int x, int numberOfSteps_)
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
            }*/
        }
        //generate centrs
        private void loopThroughCenters(int numberOfSteps, int step, World w)
        {
            for (int y = 1; y < numberOfSteps; y += 2)
            {
                for (int x = 1; x < numberOfSteps; x += 2)
                {

                    float Point1Z = w.world[posX + (x - 1) * step, posY + (y - 1) * step].height;
                    float Point2Z = w.world[posX + (x + 1) * step, posY + (y + 1) * step].height;
                    float Point3Z = w.world[posX + (x - 1) * step, posY + (y + 1) * step].height;
                    float Point4Z = w.world[posX + (x + 1) * step, posY + (y - 1) * step].height;
                    float FinalPointZ;
                    if(w.world[posX + (x) * step, posY + (y) * step].height == 0)
                    {
                        FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z) / 4) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                    }
                    else
                    {
                        FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z + w.world[posX + (x) * step, posY + (y) * step].height*10 )/ 14) + ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
                    }
                    Chunk[x * step, y * step] = new HeightNode(Math.Min(Math.Max(FinalPointZ,0.01f),0.99f));
                    addToWorld(w, x * step, y * step);
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
                    if (Chunk[y, x].height < 0f)
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
        private void addToWorld(World w,int x, int y)
        {
            w.world[posX+x,posY+y] = Chunk[x,y];
        }
    }
    public class PhysicalChunk
    {
        public PhysicalChunk(float Left_x, float Top_y, List<List<HeightNode>> a)
        {
        }
    }
}
