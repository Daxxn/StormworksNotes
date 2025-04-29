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
   OUTPUT,
   INPUT
}

public static class ProjConstants
{
   public const int COMP_SIGNAL_LEN = 32;
}
