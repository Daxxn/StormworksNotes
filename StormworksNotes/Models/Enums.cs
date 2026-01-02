using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormworksNotes.Models.Enums;
public enum CompositeSignalType
{
   NUMBER,
   ON_OFF,
}

public enum SignalDirection
{
   OUTPUT = 0,
   INPUT = 1,
}

public enum CompositeSignalDirection
{
   OUTPUT = 0,
   INPUT = 1,
   DAISY = 2,
}

public enum SignalType
{
   ON_OFF = 0,
   NUMBER = 1,
   COMPOSITE = 5,
   VIDEO = 6,
   AUDIO = 7,
}

public enum ComponentType
{
   MCU,
   BLOCK,
}

public static class ProjConstants
{
   public const int COMP_SIGNAL_LEN = 32;
}
