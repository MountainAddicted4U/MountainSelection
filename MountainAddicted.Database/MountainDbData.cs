using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Database
{
    public class MountainDbData
    {
        public int Id { get; set; }

        public decimal NeLat { get; set; }
        public decimal NeLng { get; set; }
        public decimal SwLat { get; set; }
        public decimal SwLng { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PreviewData { get; set; }
        public string OriginalData { get; set; }
        public string GCodeData { get; set; }

        public GCodeConfig GCodeConfig { get; set; }
    }
}
