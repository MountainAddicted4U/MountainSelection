using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class MountainSplitter
    {
        public MountainSplittedOnLevels SplitMountainOnLevels(GCodeMountain mountain, MountainSplitOptions options)
        {
            var splittedMountain = new MountainSplittedOnLevels 
            { 
                OriginalMountain = mountain, 
                MountainSplitOptions = options 
            };

            var maxHeight = mountain.MaxHeight;
            var minHeight = mountain.MinHeight;
            var currentHeight = maxHeight - options.HeightPerLayerInGCode * mountain.HeightPerGCode;

            while (currentHeight >= minHeight)
            {
                var xDimensionLength = mountain.Heights.GetLength(0);
                var yDimensionLength = mountain.Heights.GetLength(1);

                var layer = new Layer
                {
                    SplittedMountain = splittedMountain,
                    Points = new MPoint[xDimensionLength, yDimensionLength]
                };

                for (int i = 0; i < xDimensionLength; i++)
                {
                    for (int j = 0; j < yDimensionLength; j++)
                    {
                        layer.Points[i, j] = new MPoint
                        {
                            Layer = layer,
                            ShouldBeProcessed = mountain.Heights[i, j] < currentHeight,
                            ProcessedStatus = ProcessedStatus.NotProcessed
                        };
                    }
                }
                splittedMountain.Layers.Add(layer);
                currentHeight -= options.HeightPerLayerInGCode * mountain.HeightPerGCode;
            }

            return splittedMountain;
        }
    }
}
