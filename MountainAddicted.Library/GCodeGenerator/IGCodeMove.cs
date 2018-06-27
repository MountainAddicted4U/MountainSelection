using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MountainAddicted.Library.GCodeGenerator
{
    public interface IGCodeMove
    {
        string GCodeMoveAsString { get; }
    }
}
