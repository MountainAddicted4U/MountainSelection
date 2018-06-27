using MountainAddicted.Library.GCodeGenerator;
using MountainAddicted.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MapConverterToGCode.Controllers
{
    public class GCodeProcessor
    {
        public GCodeData GetMountainGCode(MountainData mountainData, GCodeConfiguration configuration)
        {
            var gCodeData = new GCodeData();

            // TODO: convert mountainData to mountain
            var mountain = new GCodeMountain(mountainData, configuration);

            var preparationProcessor = new PreparationProcessor();
            gCodeData.PreparationMoves = preparationProcessor.Process(mountain, configuration);

            var mainGCodeProcessor = new MainGCodeProcessor();
            gCodeData.DirtyMoves = mainGCodeProcessor.ProcessDirty(mountain, configuration);
            gCodeData.CleanMoves = mainGCodeProcessor.ProcessClean(mountain, configuration);

            return gCodeData;
        }
        
        //public void ExportGCodeMovesToFile(List<IGCodeMove> gCodeMoves, string destination)
        //{
        //    using (var file = new StreamWriter(File.Create(destination)))
        //    {
        //        foreach (var move in gCodeMoves)
        //        {
        //            file.WriteLine(move.GCodeMoveAsString);
        //        }
        //    }
        //}
    }
}
