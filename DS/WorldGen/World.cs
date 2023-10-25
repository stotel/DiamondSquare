using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
            return (PhysicalChunks[chunkInd].chunk[(X) % PhysicalChunk.SIZE, (Y) % PhysicalChunk.SIZE].height);
        }
        public void visualizePhysicalChunks(object sender, SKPaintGLSurfaceEventArgs e, int ShiftX, int ShiftY)
        {
            foreach(PhysicalChunk i in PhysicalChunks){
                SKPaint paint = new SKPaint { Color = new SKColor(0, 255, 0) };
                e.Surface.Canvas.DrawRect(i.posX * PhysicalChunk.SIZE + ShiftX, i.posY * PhysicalChunk.SIZE + ShiftY, 1, PhysicalChunk.SIZE, paint);
                e.Surface.Canvas.DrawRect(i.posX * PhysicalChunk.SIZE + ShiftX, i.posY * PhysicalChunk.SIZE + PhysicalChunk.SIZE + ShiftY, PhysicalChunk.SIZE, 1, paint);
                e.Surface.Canvas.DrawRect(i.posX * PhysicalChunk.SIZE + ShiftX, i.posY * PhysicalChunk.SIZE + ShiftY, PhysicalChunk.SIZE, 1, paint);
                e.Surface.Canvas.DrawRect(i.posX * PhysicalChunk.SIZE + PhysicalChunk.SIZE + ShiftX, i.posY * PhysicalChunk.SIZE + ShiftY, 1, PhysicalChunk.SIZE, paint);
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
                for (int j = 0; j < PhysicalChunk.SIZE; j++)
                {
                    for (int k = 0; k < PhysicalChunk.SIZE; k++)
                    {
                        float pointHeight = PhysicalChunks[i].chunk[k, j].height;
                        if (pointHeight != 0)
                        {
                            if (pointHeight < 0.3f)
                            {
                                color = (byte)(Math.Pow(pointHeight, 1) * 255 + 100);
                                e.Surface.Canvas.DrawPoint
                                (new SKPoint(PhysicalChunks[i].posX * PhysicalChunk.SIZE + k + ShiftX, PhysicalChunks[i].posY * PhysicalChunk.SIZE + j + ShiftY),
                                new SKColor(0, 0, color));
                            }
                            else
                            {
                                color = (byte)(Math.Pow(pointHeight, 1) * 255 + 50);
                                e.Surface.Canvas.DrawPoint
                                (new SKPoint(PhysicalChunks[i].posX * PhysicalChunk.SIZE + k + ShiftX, PhysicalChunks[i].posY * PhysicalChunk.SIZE + j + ShiftY),
                                new SKColor(color, color, color));
                            }
                        }
                    }
                }
            }
        }
    }
}
