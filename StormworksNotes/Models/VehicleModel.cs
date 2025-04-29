using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

namespace StormworksNotes.Models;
public class VehicleModel : Model
{
   #region Local Props
   private string? _name = null;
   private string? _desc = null;
   private ObservableCollection<ComponentModel> _components = [];
   #endregion

   #region Constructors
   public VehicleModel() { }
   #endregion

   #region Methods

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

   public ObservableCollection<ComponentModel> Components
   {
      get => _components;
      set
      {
         _components = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
