using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class LevelSettings
    {
        public decimal LevelSizeOfMountain { get; set; }  // = 20; // in meters
        public decimal LevelSizeForGCode { get; set; }    // = 1;  // in mm

        public static LevelSettings operator / (LevelSettings settings, int coof)
        {
            return new LevelSettings
            {
                LevelSizeForGCode = settings.LevelSizeForGCode / coof,
                LevelSizeOfMountain = settings.LevelSizeOfMountain / coof
            };
        }
    }
}
