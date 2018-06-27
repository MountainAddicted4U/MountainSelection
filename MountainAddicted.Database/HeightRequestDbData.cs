using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Database
{
    public class HeightRequestDbData
    {
        public int Id { get; set; }
        public int MountainId { get; set; }

        public decimal NeLat { get; set; }
        public decimal NeLng { get; set; }
        public decimal SwLat { get; set; }
        public decimal SwLng { get; set; }

        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public int RequestNumber { get; set; } 
        public string Data { get; set; }
    }
}
