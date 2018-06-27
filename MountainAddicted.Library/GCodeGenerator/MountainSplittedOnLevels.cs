using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class MountainSplittedOnLevels
    {
        public GCodeMountain OriginalMountain { get; set; }
        public List<Layer> Layers { get; set; }
        public MountainSplitOptions MountainSplitOptions { get; set; }

        public MountainSplittedOnLevels()
        {
            this.Layers = new List<Layer>();
        }
    }
}
