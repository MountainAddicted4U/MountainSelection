using MountainAddicted.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MountainAddicted.Library.Models
{
    [DebuggerDisplay("{title}: ({neLat}, {neLng}) - ({swLat}, {swLng})")]
    public class MountainViewModel
    {
        public int id { get; set; }

        public decimal neLat { get; set; }
        public decimal neLng { get; set; }
        public decimal swLat { get; set; }
        public decimal swLng { get; set; }

        public MountainData MountainData { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public SplitRange resolution { get; set; }
        public SplitRange preview { get; set; }
        public List<List<MountainPoint>> previewHeights { get; set; }
        public List<List<MountainPoint>> detailsHeights { get; set; }

        public MountainViewModel()
        {
            previewHeights = new List<List<MountainPoint>>();
            detailsHeights = new List<List<MountainPoint>>();
        }

        public MountainViewModel(MountainDbData dbData)
            :this()
        {
            id = dbData.Id;
            neLat = dbData.NeLat;
            neLng = dbData.NeLng;
            swLat = dbData.SwLat;
            swLng = dbData.SwLng;
            title = dbData.Title;
            description = dbData.Description;

        }
    }
}