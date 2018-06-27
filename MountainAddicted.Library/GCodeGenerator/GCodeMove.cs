using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class GCodeMove : IGCodeMove
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public string GCodeMoveAsString
        {
            get { return string.Format("G00 X{0} Y{1}", X, Y); }
        }

        public override string ToString()
        {
            return GCodeMoveAsString;
        }
    }
}
