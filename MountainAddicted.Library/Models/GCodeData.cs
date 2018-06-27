using MountainAddicted.Library.GCodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.Models
{
    public class GCodeData
    {
        public List<IGCodeMove> PreparationMoves { get; set; }
        public List<IGCodeMove> DirtyMoves { get; set; }
        public List<IGCodeMove> CleanMoves { get; set; }
    }
}
