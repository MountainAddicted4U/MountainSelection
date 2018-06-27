using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library.Models
{
    public class GCodeConfiguration
    {
        // размер заготовки
        public decimal FormSizeMMWidth { get; set; }
        public decimal FormSizeMMLength { get; set; }
        public decimal FormSizeMMHeight { get; set; }

        // подготовительный раунд: высота обработки
        public decimal PreparationMaxHeightMM { get; set; } = 30;

        // подготовительный раунд: высота обработки за раунд 
        public decimal PreparationHeightMMPerRound { get; set; } = 6;

        // подготовительный раунд: высота обработки за слой (или проход)
        public decimal PreparationHeightMMPerLayer { get; set; } = 2;

        public decimal DirtyR1OffsetHeightMM { get; set; } = -10;
        public decimal DirtyR2OffsetHeightMM { get; set; } = -5;
        public decimal DirtyR3OffsetHeightMM { get; set; } = -1;

        public FormFigure FormFigure { get; set; }

        // ширина фрезы, int for easier calculation
        public int PreparationSpinSizeMM { get; set; } = 6;
        public int DirtySpinSizeMM { get; set; } = 6;
        public int CleanSpinSizeMM { get; set; } = 3;

        // ширина между проходами по x,y
        public int XIncrementDirty { get; set; } = 5;
        public int YIncrementDirty { get; set; } = 5;

        public int XIncrementClean { get; set; } = 1;
        public int YIncrementClean { get; set; } = 1;

        // минимум / макс для Z.
        // макс не знаю зачем
        public decimal ZMin { get; set; } = 0;
        public decimal ZMax { get; set; } = 50;

        // мин размеры для разбивки в preparation step
        public int MinLength { get; set; } = 30;
        public int MinWidth { get; set; } = 30;

        // preparation step - additional points for overflow
        public int AdditionalPointsToOverflowCubesByWidth { get; set; } = 2;
        public int AdditionalPointsToOverflowCubesByLength { get; set; } = 2;
    }
}
