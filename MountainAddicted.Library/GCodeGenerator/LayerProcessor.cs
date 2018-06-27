using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class LayerProcessor
    {
        public List<IGCodeMove> ProcessLayer(Layer layer, SpinOptions freezeOptions, decimal currentZ)
        {
            var commandList = new List<IGCodeMove>();
            
            var numberOfRowsToProcess = layer.Points.GetLength(0) - freezeOptions.SpinSize + 1;
            var numberOfColsToProcess = layer.Points.GetLength(1) - freezeOptions.SpinSize + 1;
            var freezePoints = new MPoint[numberOfRowsToProcess, numberOfColsToProcess];
            for (int r = 0; r < numberOfRowsToProcess; r++)
            {
                for (int c = 0; c < numberOfColsToProcess; c++)
                {
                    var freezeRegion = new MPoint[freezeOptions.SpinSize, freezeOptions.SpinSize];
                    for(int freezeRegionRow = 0; freezeRegionRow < freezeOptions.SpinSize; freezeRegionRow++)
                    {
                        for(int freezeRegionCol = 0; freezeRegionCol < freezeOptions.SpinSize; freezeRegionCol++)
                        {
                            freezeRegion[freezeRegionRow, freezeRegionCol] = layer.Points[r + freezeRegionRow, c + freezeRegionCol];
                        }
                    }
                    freezePoints[r, c] = new MPoint
                    {
                        ShouldBeProcessed = !freezeRegion.Cast<MPoint>().Any(i => !i.ShouldBeProcessed) // maybe some other algorithm will be used soon
                    };
                }
            }

            for (int fr = 0; fr < numberOfRowsToProcess; fr++)
            {
                var currentRow = fr + freezeOptions.SpinSize / 2.0m;
                // move to freeze zero point
                commandList.Add(new GCodeChangeZ { Z = 0 });

                var inverse = (fr % 2) == 1;
                var currentFirstColumn = !inverse ? 0 : numberOfColsToProcess - 1;
                var currentLastColumn = !inverse ? numberOfColsToProcess - 1 : 0;
                if (freezePoints[fr, currentFirstColumn].ShouldBeProcessed)
                {
                    commandList.Add(new GCodeMove { X = currentRow, Y = currentFirstColumn + freezeOptions.SpinSize / 2.0m });
                    commandList.Add(new GCodeChangeZ { Z = currentZ });
                }
                for (int fc = 1; fc < numberOfColsToProcess; fc++)
                {
                    var currentColumn = !inverse ? fc : numberOfColsToProcess - 1 - fc;
                    var previousColumn = !inverse ? currentColumn - 1 : currentColumn + 1;
                    var shouldPreviousPointBeProcessed = freezePoints[fr, previousColumn].ShouldBeProcessed;
                    var shouldCurrentPointBeProcessed = freezePoints[fr, currentColumn].ShouldBeProcessed;
                    if (shouldCurrentPointBeProcessed != shouldPreviousPointBeProcessed)
                    {
                        if (shouldPreviousPointBeProcessed)
                        {
                            commandList.Add(new GCodeLine { X = currentRow, Y = previousColumn + freezeOptions.SpinSize / 2.0m });
                            commandList.Add(new GCodeChangeZ { Z = 0 });
                        }
                        else
                        {
                            commandList.Add(new GCodeMove { X = currentRow, Y = currentColumn + freezeOptions.SpinSize / 2.0m });
                            commandList.Add(new GCodeChangeZ { Z = currentZ });
                        }
                    }
                }
                if (freezePoints[fr, currentLastColumn].ShouldBeProcessed)
                {
                    commandList.Add(new GCodeLine { X = currentRow, Y = currentLastColumn + freezeOptions.SpinSize / 2.0m });
                }
            }
            return commandList;
        }
    }
}
