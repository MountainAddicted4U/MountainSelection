using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class GCodeChangeZ : IGCodeMove
    {
        public decimal Z { get; set; }

        public string GCodeMoveAsString
        {
            get { return string.Format("G01 Z{0}", Z); }
        }

        public override string ToString()
        {
            return GCodeMoveAsString;
        }
    }
}
