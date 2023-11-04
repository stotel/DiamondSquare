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
        public bool isPotentForGeneration = true;

        public PhysicalChunk(int PosX_, int PosY_,bool isPotentForGeneration_ = true)
        {
            posX = PosX_;
            posY = PosY_;
            isPotentForGeneration = isPotentForGeneration_;
        }
        
        public void checkIfRendered()
        {
            isRendered = true;
            for (int i = 1;i<SIZE; i++)
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
            for (int j = 0; j < SIZE; j++)
            {
                for (int k = 0; k < SIZE; k++)
                {
                    float pointHeight = chunk[k, j].height;
                    if (pointHeight != 0)
                    {
                        if (pointHeight < 0.0f)
                        {
                            color = (byte)(Math.Pow(pointHeight, 1) * 255 + 100);
                            e.Surface.Canvas.DrawPoint
                            (new SKPoint(posX * SIZE + k + ShiftX, posY * SIZE + j + ShiftY),
                            new SKColor(0, 0, color));
                        }
                        else
                        {
                            color = (byte)(Math.Pow(pointHeight, 1) * 255/* + 50*/);
                            e.Surface.Canvas.DrawPoint
                            (new SKPoint(posX * SIZE + k + ShiftX, posY * SIZE + j + ShiftY),
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
            if(isPotentForGeneration)
            {
                paint.Color = paint.Color.WithRed(255);
            }
            e.Surface.Canvas.DrawRect(posX * SIZE + ShiftX, posY * SIZE + ShiftY, 1, SIZE, paint);
            e.Surface.Canvas.DrawRect(posX * SIZE + ShiftX, posY * SIZE + SIZE + ShiftY, SIZE, 1, paint);
            e.Surface.Canvas.DrawRect(posX * SIZE + ShiftX, posY * SIZE + ShiftY, SIZE, 1, paint);
            e.Surface.Canvas.DrawRect(posX * SIZE + SIZE + ShiftX, posY * SIZE + ShiftY, 1, SIZE, paint);
        }
    }
}