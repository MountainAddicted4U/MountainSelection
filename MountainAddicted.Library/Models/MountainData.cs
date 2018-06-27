using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.Models
{
    public class MountainData
    {
        public List<List<MountainPoint>> Points { get; set; }
        public SplitRange SplitRange { get; set;}
    }
}
