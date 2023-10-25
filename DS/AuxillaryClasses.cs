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
        
        //public bool Validate
    }
}