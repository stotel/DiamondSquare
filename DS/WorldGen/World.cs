using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace DS
{
    public class World
    {
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
            if (chunkInd == -1)
            {
                return 0;
            }
            try
            {
                return (PhysicalChunks[chunkInd].chunk[(X) % PhysicalChunk.SIZE, (Y) % PhysicalChunk.SIZE].height);
            }
            catch(IndexOutOfRangeException ex)
            {
                return (PhysicalChunks[chunkInd].chunk[(X) % PhysicalChunk.SIZE, (Y) % PhysicalChunk.SIZE].height);
                Console.WriteLine($"PhysicalChunks.Count() - 1 = {PhysicalChunks.Count() - 1} | chunkdInd = {chunkInd}");
                //Console.WriteLine($"chunk.Length - 1 = {PhysicalChunks[chunkInd].chunk.Length} | (X) % PhysicalChunk.SIZE, (Y) % PhysicalChunk.SIZE = {(X) % PhysicalChunk.SIZE, (Y) % PhysicalChunk.SIZE});
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
            byte color;
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
                int powOf2LogChunkSize = random.Next(5,7);
                int ChSideSize = (int)Math.Pow(2, powOf2LogChunkSize);
                LogicalChunk chunk = new LogicalChunk((i.posX+1)*PhysicalChunk.SIZE-ChSideSize, (i.posY+1)*PhysicalChunk.SIZE - ChSideSize, powOf2LogChunkSize, 2f, random.Next(2147483647));
                chunk.InitChunk(this);
                chunk.generateLandscape(this);
                LogicalChunks.Add(chunk);
            }
            Debug.WriteLine(LogicalChunks.Count);
        }
    }
}
