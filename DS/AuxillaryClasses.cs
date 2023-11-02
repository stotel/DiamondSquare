using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
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
        public bool isRendered = true;

        public PhysicalChunk(int PosX_, int PosY_)
        {
            posX = PosX_;
            posY = PosY_;
        }
        
        public void checkIfRendered()
        {
            isRendered = true;
            for (int i = 0;i<SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    if (chunk[i,j].height == 0)
                    {
                        isRendered = false; break;
                    } 
                }
                if (!isRendered)
                {
                    break;
                }
            }
        }
        public void DrawPhysycalChunk(object sender, SKPaintGLSurfaceEventArgs e, int ShiftX, int ShiftY)
        {
            byte color;
            for (int j = 0; j < PhysicalChunk.SIZE; j++)
            {
                for (int k = 0; k < PhysicalChunk.SIZE; k++)
                {
                    float pointHeight = chunk[k, j].height;
                    if (pointHeight != 0)
                    {
                        if (pointHeight < 0.0f)
                        {
                            color = (byte)(Math.Pow(pointHeight, 1) * 255 + 100);
                            e.Surface.Canvas.DrawPoint
                            (new SKPoint(posX * PhysicalChunk.SIZE + k + ShiftX, posY * PhysicalChunk.SIZE + j + ShiftY),
                            new SKColor(0, 0, color));
                        }
                        else
                        {
                            color = (byte)(Math.Pow(pointHeight, 1) * 255 + 50);
                            e.Surface.Canvas.DrawPoint
                            (new SKPoint(posX * PhysicalChunk.SIZE + k + ShiftX, posY * PhysicalChunk.SIZE + j + ShiftY),
                            new SKColor(color, color, color));
                        }
                    }
                }
            }
        }
        public void DrawPhysicalChunkBorder(object sender, SKPaintGLSurfaceEventArgs e, int ShiftX, int ShiftY)
        {
            SKPaint paint;
            if (isRendered)
            {
                paint = new SKPaint { Color = new SKColor(0, 255, 0) };
            }
            else
            {
                paint = new SKPaint { Color = new SKColor(0, 155, 0) };
            }
            e.Surface.Canvas.DrawRect(posX * PhysicalChunk.SIZE + ShiftX, posY * PhysicalChunk.SIZE + ShiftY, 1, PhysicalChunk.SIZE, paint);
            e.Surface.Canvas.DrawRect(posX * PhysicalChunk.SIZE + ShiftX, posY * PhysicalChunk.SIZE + PhysicalChunk.SIZE + ShiftY, PhysicalChunk.SIZE, 1, paint);
            e.Surface.Canvas.DrawRect(posX * PhysicalChunk.SIZE + ShiftX, posY * PhysicalChunk.SIZE + ShiftY, PhysicalChunk.SIZE, 1, paint);
            e.Surface.Canvas.DrawRect(posX * PhysicalChunk.SIZE + PhysicalChunk.SIZE + ShiftX, posY * PhysicalChunk.SIZE + ShiftY, 1, PhysicalChunk.SIZE, paint);
        }
    }
}