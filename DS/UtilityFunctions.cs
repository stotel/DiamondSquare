using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    static class UtilityFunctions
    {
        public static int LoopInChunkCordsToTheCHUNKSIZERange(int C) { return (Math.Abs((C) % PhysicalChunk.SIZE)); }
    }
}
