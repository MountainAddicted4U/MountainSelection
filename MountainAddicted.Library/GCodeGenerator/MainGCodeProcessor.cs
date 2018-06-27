using MountainAddicted.Library.Models;
using MountainAddicted.Library.GCodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class MainGCodeProcessor
    {
        public List<IGCodeMove> ProcessDirty(GCodeMountain mountain, GCodeConfiguration configuration)
        {
            var moves = new List<IGCodeMove>();

            var spin = new SpinOptions { SpinSize = configuration.DirtySpinSizeMM };

            var r1Moves = Process(mountain, configuration, configuration.DirtyR1OffsetHeightMM, configuration.XIncrementDirty, configuration.YIncrementDirty, spin);
            var r2Moves = Process(mountain, configuration, configuration.DirtyR2OffsetHeightMM, configuration.XIncrementDirty, configuration.YIncrementDirty, spin);
            var r3Moves = Process(mountain, configuration, configuration.DirtyR3OffsetHeightMM, configuration.XIncrementDirty, configuration.YIncrementDirty, spin);

            moves.AddRange(r1Moves);
            moves.AddRange(r2Moves);
            moves.AddRange(r3Moves);

            return moves;
        }

        public List<IGCodeMove> ProcessClean(GCodeMountain mountain, GCodeConfiguration configuration)
        {
            var spin = new SpinOptions { SpinSize = configuration.CleanSpinSizeMM };
            var moves = Process(
                mountain, 
                configuration, 
                0, 
                configuration.XIncrementClean, 
                configuration.YIncrementClean, 
                spin);

            return moves;
        }

        private List<IGCodeMove> Process(GCodeMountain mountain, GCodeConfiguration config, decimal zOffset, int xIncrement, int yIncrement, SpinOptions spin)
        {
            var moves = new List<IGCodeMove>();

            // go to zero point
            var currentZ = 0.0m;
            var x = 0;
            var y = 0;
            var previousZ = 0.0m;

            var fromTopToBottom = true;
            var numberOfPointsPerX = mountain.Heights.GetLength(0);
            var numberOfPointsPerY = mountain.Heights.GetLength(1);

            moves.Add(new GCodeChangeZ { Z = 0 });

            while (x < numberOfPointsPerX)
            {
                if (WithinRadius(mountain, x, y))
                {
                    previousZ = currentZ;
                    currentZ = Math.Round(GetCurrentZ(mountain, x, y, spin), 2);
                    currentZ = currentZ + zOffset;
                    currentZ = Math.Max(config.ZMin, currentZ);
                    currentZ = Math.Min(config.ZMax, currentZ);

                    var xGCode = mountain.MountainWidthInGCode * x / numberOfPointsPerX;
                    var yGCode = mountain.MountainLengthInGCode * y / numberOfPointsPerY;

                    if (currentZ > previousZ)
                    {
                        moves.Add(new GCodeLine { X = xGCode, Y = yGCode });
                        moves.Add(new GCodeChangeZ { Z = currentZ });
                    }
                    else
                    {
                        moves.Add(new GCodeChangeZ { Z = currentZ });
                        moves.Add(new GCodeLine { X = xGCode, Y = yGCode });
                    }
                }
                y += fromTopToBottom ? yIncrement : -yIncrement;
                if (y < 0)
                {
                    fromTopToBottom = true;
                    y = 0;
                    x += xIncrement;
                }
                else if (y >= numberOfPointsPerY)
                {
                    fromTopToBottom = false;
                    y = numberOfPointsPerY - 1;
                    x += xIncrement;
                }
            }

            return moves;
        }


        private decimal GetCurrentZ(GCodeMountain mountain, int x, int y, SpinOptions freeze)
        {
            var numberOfPointsPerX = mountain.Heights.GetLength(0);
            var numberOfPointsPerY = mountain.Heights.GetLength(1);

            var maxHeight = mountain.Heights[x, y];
            var xPointsOfInterestCount = (int)(freeze.SpinSize * numberOfPointsPerX / mountain.MountainWidthInGCode);
            var yPointsOfInterestCount = (int)(freeze.SpinSize * numberOfPointsPerY / mountain.MountainLengthInGCode);

            for (int i = 0; i <= xPointsOfInterestCount; i++)
            {
                for (int j = 0; j <= yPointsOfInterestCount; j++)
                {
                    var xCoord = x + i - xPointsOfInterestCount / 2;
                    var yCoord = y + j - yPointsOfInterestCount / 2;

                    if (xCoord >= 0 && xCoord < numberOfPointsPerX &&
                        yCoord >= 0 && yCoord < numberOfPointsPerY)
                    {
                        maxHeight = Math.Max(maxHeight, mountain.Heights[xCoord, yCoord]);
                    }
                }
            }
            return mountain.MountainHeightInGCode * (1 - (maxHeight - mountain.MinHeight) / (mountain.MaxHeight - mountain.MinHeight));
        }

        private bool WithinRadius(GCodeMountain mountain, int x, int y)
        {
            var width = mountain.Heights.GetLength(0);
            var height = mountain.Heights.GetLength(1);

            var x2 = Math.Pow(x - width / 2.0, 2);
            var y2 = Math.Pow(y - height / 2.0, 2);
            return (x2 + y2) <= (width / 2.0) * (height / 2.0) * 1.05;
        }
    }
}
