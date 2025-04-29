using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

using StormworksNotes.Models.Enums;

namespace StormworksNotes.Models;
public class CompositeModel : Model
{
   #region Local Props
   private string? _name = "Composite Signal";
   private string? _desc = null;
   private SignalDirection _direction = SignalDirection.INPUT;
   private SignalModel[] _boolSignals = new SignalModel[ProjConstants.COMP_SIGNAL_LEN];
   private SignalModel[] _numberSignals = new SignalModel[ProjConstants.COMP_SIGNAL_LEN];
   #endregion

   #region Constructors
   public CompositeModel()
   {
      GenerateSignals();
   }
   #endregion

   #region Methods
   private void GenerateSignals()
   {
      for (int i = 0; i < ProjConstants.COMP_SIGNAL_LEN; i++)
      {
         BoolSignals[i] = SignalModel.Create(i + 1, CompositeSignalType.ON_OFF);
         NumberSignals[i] = SignalModel.Create(i + 1, CompositeSignalType.NUMBER);
      }
   }

   public void SortList(IList<SignalModel> senderData)
   {
      if (senderData == BoolSignals)
      {
         BoolSignals = BoolSignals.OrderBy(x => x.Channel).ToArray();
         OnPropertyChanged(nameof(BoolSignals));
      }
      else if (senderData == NumberSignals)
      {
         NumberSignals = NumberSignals.OrderBy(x => x.Channel).ToArray();
         OnPropertyChanged(nameof(NumberSignals));
      }
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

   public string? Description
   {
      get => _desc;
      set
      {
         _desc = value;
         OnPropertyChanged();
      }
   }

   public SignalModel[] BoolSignals
   {
      get => _boolSignals;
      set
      {
         _boolSignals = value;
         OnPropertyChanged();
      }
   }

   public SignalModel[] NumberSignals
   {
      get => _numberSignals;
      set
      {
         _numberSignals = value;
         OnPropertyChanged();
      }
   }

   public SignalDirection Direction
   {
      get => _direction;
      set
      {
         _direction = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
