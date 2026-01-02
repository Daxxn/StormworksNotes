using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

using StormworksNotes.Models.Enums;

namespace StormworksNotes.Models;
public class SignalModel : Model
{
   #region Local Props
   private string? _name = null;
   private int _channel = 0;
   private string? _desc = null;
   private RangeModel? _range = null;
   private SignalDirection _dir = SignalDirection.INPUT;
   private CompositeSignalType _compType = CompositeSignalType.NUMBER;
   private SignalType _type = SignalType.ON_OFF;
   #endregion

   #region Constructors
   public SignalModel() { }
   #endregion

   #region Methods
   public static SignalModel Create(int channel, CompositeSignalType type)
   {
      return new()
      {
         Name = $"",
         Channel = channel,
         CompType = type,
         NumberRange = type == CompositeSignalType.NUMBER ? new(-1, 1) : null,
      };
   }
   #endregion

   #region Full Props
   public string? Name
   {
      get => _name;
      set
      {
         _name = value;
         OnPropertyChanged();
      }
   }

   public int Channel
   {
      get => _channel;
      set
      {
         _channel = value;
         OnPropertyChanged();
      }
   }

   public SignalDirection Direction
   {
      get => _dir;
      set
      {
         _dir = value;
         OnPropertyChanged();
      }
   }

   public string? Description
   {
      get => _desc;
      set
      {
         _desc = value;
         OnPropertyChanged();
      }
   }

   public CompositeSignalType CompType
   {
      get => _compType;
      set
      {
         _compType = value;
         OnPropertyChanged();
      }
   }

   public SignalType Type
   {
      get => _type;
      set
      {
         _type = value;
         OnPropertyChanged();
      }
   }

   public RangeModel? NumberRange
   {
      get => _range;
      set
      {
         _range = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
