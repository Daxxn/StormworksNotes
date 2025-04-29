using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

using Newtonsoft.Json;

namespace StormworksNotes.Models;
public class ComponentModel : Model
{
   #region Local Props
   private string? _name = "Component";
   private string? _desc = null;
   private ObservableCollection<SignalModel> _boolSignals = [];
   private ObservableCollection<SignalModel> _numberSignals = [];

   private ObservableCollection<CompositeModel> _compSignals = [];

   private SignalModel? _selectedSignal = null;
   private CompositeModel? _selectedComposite = null;
   #endregion

   #region Constructors
   public ComponentModel() { }
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

   public ObservableCollection<SignalModel> BoolSignals
   {
      get => _boolSignals;
      set
      {
         _boolSignals = value;
         OnPropertyChanged();
      }
   }

   public ObservableCollection<SignalModel> NumberSignals
   {
      get => _numberSignals;
      set
      {
         _numberSignals = value;
         OnPropertyChanged();
      }
   }

   public ObservableCollection<CompositeModel> CompositeSignals
   {
      get => _compSignals;
      set
      {
         _compSignals = value;
         OnPropertyChanged();
      }
   }

   [JsonIgnore]
   public SignalModel? SelectedSignal
   {
      get => _selectedSignal;
      set
      {
         _selectedSignal = value;
         OnPropertyChanged();
      }
   }

   [JsonIgnore]
   public CompositeModel? SelectedComposite
   {
      get => _selectedComposite;
      set
      {
         _selectedComposite = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
