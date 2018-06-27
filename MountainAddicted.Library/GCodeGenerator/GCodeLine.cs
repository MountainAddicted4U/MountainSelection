using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class GCodeLine : IGCodeMove
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public string GCodeMoveAsString
        {
            get { return string.Format("G01 X{0} Y{1} F{2}", X, Y, 500); }
        }

        public override string ToString()
        {
            return GCodeMoveAsString;
        }
    }
}
