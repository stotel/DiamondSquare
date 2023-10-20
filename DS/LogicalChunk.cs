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
	public class LogicalChunk
	{
		public static int chunkSizeParam;
		public static int CHUNKSIZE;
		public float ROUGHNESS;
		private static int maxSizeParam = 16;
		public int posX; public int posY;
		public HeightNode[,] Chunk;
		public Random random;
		public int randInt = 0;
		public bool isGenerated = false;

		public LogicalChunk(int Left_x, int Top_y, int powerOf2ForChunkSize, float roughness, int random_ = 0)
		{
			chunkSizeParam = powerOf2ForChunkSize;
			CHUNKSIZE = (int)Math.Pow(2, chunkSizeParam);
			ROUGHNESS = roughness;
			posX = Left_x; posY = Top_y;
			Chunk = new HeightNode[CHUNKSIZE + 1, CHUNKSIZE + 1];
			randInt = random_;
			random = new Random(randInt);
		}
		public void generateChunk(object sender, SKPaintGLSurfaceEventArgs e, World w)
		{
			InitChunk(w);
			generateLandscape(w);
		}
		public void InitChunk(World w)
		{
			Chunk[0, 0] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX, posY, w));
			TransferFromLogicalChunkToWorld(w, 0, 0);
			Chunk[0, CHUNKSIZE] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX, posY + CHUNKSIZE, w));
			TransferFromLogicalChunkToWorld(w, 0, CHUNKSIZE);
			Chunk[CHUNKSIZE, 0] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX + CHUNKSIZE, posY, w));
			TransferFromLogicalChunkToWorld(w, CHUNKSIZE, 0);
			Chunk[CHUNKSIZE, CHUNKSIZE] = new HeightNode(avarageOfSurraundingPointsPlusMinimalRandom(posX + CHUNKSIZE, posY + CHUNKSIZE, w));
			TransferFromLogicalChunkToWorld(w, CHUNKSIZE, CHUNKSIZE);
		}
		private float avarageOfSurraundingPointsPlusMinimalRandom(int X, int Y, World w)
		{
            int[] inChunkCords = worldCordsToInChunkCords(X, Y);
            int[] chunkCords = worldCordsToChunkCords(X, Y);
            int posInChunksList = w.ChunkWithCordsIndex(chunkCords[0], chunkCords[1]);
			if (posInChunksList != -1)
			{
				float height = w.PhysicalChunks[posInChunksList].chunk[inChunkCords[0], inChunkCords[1]].height;
                if (height != 0)
                {
                    return height;
                }
				height = (float)random.NextDouble() * chunkSizeParam / maxSizeParam;
                return height;
            }
			else
			{
                w.PhysicalChunks.Add(new PhysicalChunk(chunkCords[0], chunkCords[1]));
                w.PhysicalChunksDict.Add(( chunkCords[0], chunkCords[1]), w.PhysicalChunks.Count() - 1);
                float height =  w.PhysicalChunks[w.PhysicalChunks.Count() - 1].chunk[inChunkCords[0], inChunkCords[1]].height;
                if (height != 0)
                {
                    return height;
                }
                height = (float)random.NextDouble() * chunkSizeParam / maxSizeParam;
                return height;
            }
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
			for (int y = 0; y < numberOfSteps + 1; y++)
			{
				for (int x = 0; x < numberOfSteps + 1; x++)
				{
					if ((x + y) % 2 != 0)
					{
						if (heightFromBlockPos(w, x, y, step) == 0)
						{
                            float Point1Z = heightFromBlockPos(w, x - 1, y, step);
                            float Point2Z = heightFromBlockPos(w, x + 1, y, step);
                            float Point3Z = heightFromBlockPos(w, x, y - 1, step);
                            float Point4Z = heightFromBlockPos(w, x, y + 1, step);
							if (x == 0 && Point1Z == 0)
							{
								Point1Z = (float)(((Point2Z + Point4Z + Point3Z) / 3) + Math.Pow(addRandom(step), step * 2));
								AddToWorld(w, (x - 1) * step, (y) * step, Point1Z);
							}
							else if (y == 0 && Point3Z == 0)
							{
								Point3Z = (float)(((Point2Z + Point4Z + Point1Z) / 3) + Math.Pow(addRandom(step), step * 2));
                                AddToWorld(w, (x) * step, (y - 1) * step, Point3Z);
                            }
							else if (x == numberOfSteps && Point2Z == 0)
							{
								Point2Z = (float)(((Point1Z + Point4Z + Point3Z) / 3) + Math.Pow(addRandom(step), step * 2));
                                AddToWorld(w, (x + 1) * step, (y) * step, Point2Z);
                            }
							else if (y == numberOfSteps && Point4Z == 0)
							{
								Point4Z = (float)(((Point2Z + Point3Z + Point1Z) / 3) + Math.Pow(addRandom(step), step * 2));
                                AddToWorld(w, (x) * step, (y + 1) * step, Point4Z);
                            }
							float FinalPointZ;
							if (w.GetHeightByCords(posX + (x) * step, posY + (y) * step) == 0)
							{
								FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z) / 4) + addRandom(step) / 2);
							}
							else
							{
								FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z /*+ heightFromBlockPos(w, x, y, step) * 10*/) / 4) + addRandom(step) / 2);
							}
							Chunk[x * step, y * step] = new HeightNode(Math.Min(Math.Max(FinalPointZ, 0.01f), 0.99f));
							TransferFromLogicalChunkToWorld(w, x * step, y * step);
						}
					}
				}
			}
		}
		//generate centrs
		private void loopThroughCenters(int numberOfSteps, int step, World w)
		{
			for (int y = 1; y < numberOfSteps; y += 2)
			{
				for (int x = 1; x < numberOfSteps; x += 2)
				{

					float Point1Z = heightFromBlockPos(w, x - 1, y - 1, step);
					float Point2Z = heightFromBlockPos(w, x + 1, y + 1, step);
					float Point3Z = heightFromBlockPos(w, x - 1, y + 1, step);
					float Point4Z = heightFromBlockPos(w, x + 1, y - 1, step);
					float FinalPointZ;
					if (w.GetHeightByCords(posX + (x) * step, posY + (y) * step) == 0)
					{
						FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z) / 4) + addRandom(step) / 2);
					}
					else
					{
						FinalPointZ = (float)(((Point1Z + Point2Z + Point3Z + Point4Z + heightFromBlockPos(w, x, y, step) * (numberOfSteps - step)) / (numberOfSteps - step + 4)) + addRandom(step) / 2);
					}
					Chunk[x * step, y * step] = new HeightNode(Math.Min(Math.Max(FinalPointZ, 0.01f), 0.99f));
					TransferFromLogicalChunkToWorld(w, x * step, y * step);
				}
			}
		}
		private float heightFromBlockPos(World w, int X, int Y, int step)
		{
			return w.GetHeightByCords(posX + (X) * step, posY + (Y) * step);
		}
		private double addRandom(int step)
		{
			return (ROUGHNESS * ((float)step / CHUNKSIZE) * (random.NextDouble() - 0.5));
		}
		private void AddToWorld(World w, int x, int y, float height)
		{
            int[] inChunkCords = worldCordsToInChunkCords(posX + x, posY + y);
            int[] chunkCords = worldCordsToChunkCords(posX + x, posY + y);
            int posInChunksList = w.ChunkWithCordsIndex(chunkCords[0], chunkCords[1]);

            if (posInChunksList != -1)
            {
                w.PhysicalChunks[posInChunksList].chunk[inChunkCords[0], inChunkCords[1]].height = height;
            }
            else
            {
                w.PhysicalChunks.Add(new PhysicalChunk(chunkCords[0], chunkCords[1]));
                w.PhysicalChunksDict.Add((chunkCords[0], chunkCords[1]), w.PhysicalChunks.Count() - 1);
                w.PhysicalChunks[w.PhysicalChunks.Count() - 1].chunk[inChunkCords[0], inChunkCords[1]].height = height;
            }
        }
		private void TransferFromLogicalChunkToWorld(World w, int x, int y)
		{
            int[] inChunkCords = worldCordsToInChunkCords(posX + x, posY + y);
            int[] chunkCords = worldCordsToChunkCords(posX + x, posY + y);
            int posInChunksList = w.ChunkWithCordsIndex(chunkCords[0], chunkCords[1]);
            if (posInChunksList != -1)
			{
                w.PhysicalChunks[posInChunksList].chunk[inChunkCords[0], inChunkCords[1]] = Chunk[x, y];
            }
			else
			{
                w.PhysicalChunks.Add(new PhysicalChunk(chunkCords[0], chunkCords[1]));
				w.PhysicalChunksDict.Add((chunkCords[0], chunkCords[1]), w.PhysicalChunks.Count() - 1);
                w.PhysicalChunks[w.PhysicalChunks.Count() - 1].chunk[inChunkCords[0], inChunkCords[1]] = Chunk[x, y];
            }
		}
        private int[] worldCordsToChunkCords(int x, int y)
        {
            return (new int[] { (x) / PhysicalChunk.SIZE, (y) / PhysicalChunk.SIZE });
        }
        private int[] worldCordsToInChunkCords(int x, int y)
		{
			return (new int[] { (x) % PhysicalChunk.SIZE, (y) % PhysicalChunk.SIZE });
		}
	}
}
