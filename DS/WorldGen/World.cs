using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace DS
{
    public class World
    {
        public static int WorldStaticRandomInt = 100;
        private static Random random = new Random(WorldStaticRandomInt);
        public List<LogicalChunk> LogicalChunks = new List<LogicalChunk>();
        public List<PhysicalChunk> PhysicalChunks = new List<PhysicalChunk>();
        public Dictionary<(int, int), int> PhysicalChunksDict = new Dictionary<(int, int), int>();

        public World()
        {
        }
        public int ChunkWithCordsIndex(int X, int Y)
        {
            return PhysicalChunksDict.TryGetValue((X, Y), out var result) ? result : -1;
        }
        public float GetHeightByCords(int X, int Y)
        { 
            int chunkInd = ChunkWithCordsIndex(X / PhysicalChunk.SIZE, Y / PhysicalChunk.SIZE);
            int inChunkX = UtilityFunctions.LoopInChunkCordsToTheCHUNKSIZERange(X);
            int inChunkY = UtilityFunctions.LoopInChunkCordsToTheCHUNKSIZERange(Y);
            if (chunkInd == -1)
            {
                return 0;
            }
            try
            {
                return (PhysicalChunks[chunkInd].chunk[inChunkX,inChunkY].height);
            }
            catch(IndexOutOfRangeException ex)
            {
                Debug.WriteLine($"PhysicalChunks.Count() - 1 = {PhysicalChunks.Count() - 1} | chunkdInd = {chunkInd}");
                Debug.WriteLine($"chunk.Length - 1 = {PhysicalChunks[chunkInd].chunk.Length} | (X) % PhysicalChunk.SIZE = {inChunkX}, (Y) % PhysicalChunk.SIZE = {inChunkY}");
                throw new Exception("Ті лох", ex);
            }
        }
        public void findRenderedPhysicalChunks()
        {
            foreach (PhysicalChunk i in PhysicalChunks)
            {
                i.checkIfRendered();
            }
        }
        public void visualizePhysicalChunks(object sender, SKPaintGLSurfaceEventArgs e, int ShiftX, int ShiftY)
        {
            foreach(PhysicalChunk i in PhysicalChunks){
                i.DrawPhysicalChunkBorder(sender, e, ShiftX, ShiftY);
            }
        }
        public void visualizeLogicalChunks(object sender, SKPaintGLSurfaceEventArgs e, int ShiftX, int ShiftY)
        {
            foreach(LogicalChunk i in LogicalChunks)
            {
                SKPaint paint = new SKPaint { Color = new SKColor(255, 0, 0) };
                e.Surface.Canvas.DrawRect(i.posX + ShiftX, i.posY + ShiftY, 1, i.CHUNKSIZE, paint);
                e.Surface.Canvas.DrawRect(i.posX + ShiftX, i.posY + +i.CHUNKSIZE + ShiftY, i.CHUNKSIZE, 1, paint);
                e.Surface.Canvas.DrawRect(i.posX + ShiftX, i.posY + ShiftY, i.CHUNKSIZE, 1, paint);
                e.Surface.Canvas.DrawRect(i.posX + i.CHUNKSIZE + ShiftX, i.posY + ShiftY, 1, i.CHUNKSIZE, paint);
            }
        }
        public void visualiseWorld(object sender, SKPaintGLSurfaceEventArgs e, int ShiftX, int ShiftY)
        {
            for (int i = 0; i < PhysicalChunks.Count; i++)
            {
                PhysicalChunks[i].DrawPhysycalChunk(sender, e, ShiftX, ShiftY);
            }
        }

        public void expandWorldInAllDirections(Random random)
        {
            List<PhysicalChunk> CurrentUnrenderedPhysicalChunks = new List<PhysicalChunk>() ;
            Debug.WriteLine(LogicalChunks.Count);
            foreach (PhysicalChunk i in PhysicalChunks)
            {
                if (i.isRendered == false)
                {
                    CurrentUnrenderedPhysicalChunks.Add(i);
                }
            }
            foreach(PhysicalChunk i in CurrentUnrenderedPhysicalChunks)
            {
                if (i.isPotentForGeneration)
                {
                    int powOf2LogChunkSize = random.Next(5, 8);
                    int SpawnRandomDist = (int)Math.Pow(2, powOf2LogChunkSize)/16 - 1;
                    LogicalChunk chunk = new LogicalChunk(((i.posX) * PhysicalChunk.SIZE) - (random.Next(SpawnRandomDist) * PhysicalChunk.SIZE), ((i.posY) * PhysicalChunk.SIZE) - (random.Next(SpawnRandomDist) * PhysicalChunk.SIZE), powOf2LogChunkSize, 4f, random.Next(2147483647));
                    chunk.InitChunk(this);
                    chunk.generateLandscape(this);
                    LogicalChunks.Add(chunk);
                }
            }
            Debug.WriteLine(LogicalChunks.Count);
        }
    }
}
