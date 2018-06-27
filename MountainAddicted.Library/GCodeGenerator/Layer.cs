using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class Layer
    {
        public MPoint[,] Points { get; set; }
        public MountainSplittedOnLevels SplittedMountain { get; set; }
    }
}
