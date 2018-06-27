using MountainAddicted.Library.Models;
using MountainAddicted.Library.GCodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class PreparationProcessor
    {
        public List<IGCodeMove> Process(GCodeMountain mountain, GCodeConfiguration configuration)
        {
            var mountainSplitter = new MountainSplitter();

            
            var mountainSplitOptions = new MountainSplitOptions { HeightPerLayerInGCode = configuration.PreparationHeightMMPerLayer };
            var roughSplittedMountain = mountainSplitter.SplitMountainOnLevels(mountain, mountainSplitOptions);

            var moves = new List<IGCodeMove>();
            var layerProcessor = new CubeLayerProcessor();
            var targetZ = 0.0m;
            foreach (var layer in roughSplittedMountain.Layers)
            {
                targetZ += roughSplittedMountain.MountainSplitOptions.HeightPerLayerInGCode;
                moves.AddRange(layerProcessor.ProcessLayer(layer, configuration, targetZ));
            }
            return moves;
        }
    }
}
