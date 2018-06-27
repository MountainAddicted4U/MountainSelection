using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class MPoint
    {
        public bool ShouldBeProcessed { get; set; }
        public Layer Layer { get; set; }
        public ProcessedStatus ProcessedStatus { get; set; }
    }
}
