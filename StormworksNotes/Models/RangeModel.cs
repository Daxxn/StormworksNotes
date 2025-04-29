using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

namespace StormworksNotes.Models;
public class RangeModel : Model
{
   #region Local Props
   private double _min = -1;
   private double _max = 1;
   #endregion

   #region Constructors
   public RangeModel() { }
   public RangeModel(double min, double max)
   {
      Min = min; Max = max;
   }
   #endregion

   #region Methods

   #endregion

   #region Full Props
   public double Min
   {
      get => _min;
      set
      {
         _min = value;
         OnPropertyChanged();
      }
   }

   public double Max
   {
      get => _max;
      set
      {
         _max = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
