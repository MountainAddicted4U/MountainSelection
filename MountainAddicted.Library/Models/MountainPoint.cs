using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.Models
{
    [DebuggerDisplay("{height} ({lat}, {lng})")]
    public class MountainPoint
    {
        public decimal lng { get; set; }
        public decimal lat { get; set; }
        public decimal height { get; set; }
    }
}
