using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MountainAddicted.Library.Models;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class GCodeMountain
    {
        public Decimal[,] Heights { get; set; }

        private decimal? maxHeight;
        private decimal? minHeight;
        
        public GCodeMountain(MountainData mountainData, GCodeConfiguration config)
        {
            MountainWidthInGCode = config.FormSizeMMWidth;
            MountainLengthInGCode = config.FormSizeMMLength;
            MountainHeightInGCode = config.FormSizeMMHeight;

            var minorLength = mountainData.Points.Count;
            var majorLength = mountainData.Points[0].Count;
            Heights = new decimal[minorLength, majorLength];
            for(int i = 0; i < minorLength; i++)
            {
                for(int j = 0; j < majorLength; j++)
                {
                    Heights[i, j] = mountainData.Points[i][j].height;
                }
            }
        }

        public decimal MaxHeight
        {
            get
            {
                return !maxHeight.HasValue ? 
                    (maxHeight = Heights.Cast<decimal>().Max()).Value : 
                    maxHeight.Value;
            }
        }

        public decimal MinHeight
        {
            get
            {
                return !minHeight.HasValue ?
                    (minHeight = Heights.Cast<decimal>().Min()).Value :
                    minHeight.Value;
            }
        }

        public decimal MountainWidthInGCode { get; private set; }   // x
        public decimal MountainLengthInGCode { get; private set; }  // y
        public decimal MountainHeightInGCode { get; private set; }  // z
        
        public decimal HeightPerGCode
        {
            get
            {
                return (MaxHeight - MinHeight) / MountainHeightInGCode;
            }
        }
    }
}
