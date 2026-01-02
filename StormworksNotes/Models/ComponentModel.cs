using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

using Newtonsoft.Json;

using StormworksNotes.Models.Enums;

namespace StormworksNotes.Models;
public class ComponentModel : Model
{
   #region Local Props
   private string? _name = "New MCU";
   private string? _desc = null;
   private ComponentType _type = ComponentType.MCU;
   private ObservableCollection<SignalModel> _boolSignals = [];
   private ObservableCollection<SignalModel> _numberSignals = [];
   private ObservableCollection<SignalModel> _videoSignals = [];
   private ObservableCollection<SignalModel> _audioSignals = [];

   private ObservableCollection<CompositeModel> _compSignals = [];

   private SignalModel? _selectedSignal = null;
   private CompositeModel? _selectedComposite = null;
   #endregion

   #region Constructors
   public ComponentModel() { }
   #endregion

   #region Methods
   public ComponentModel Copy()
   {
      return new()
      {
         Name = Name,
         Description = Description,
         Type = Type,
         BoolSignals = new(BoolSignals),
         NumberSignals = new(NumberSignals),
         VideoSignals = new(VideoSignals),
         AudioSignals = new(AudioSignals),
         CompositeSignals = new(CompositeSignals)
      };
   }

   public void Replace(ComponentModel comp)
   {
      Name = comp.Name;
      Description = comp.Description;
      Type = comp.Type;
      BoolSignals = new(comp.BoolSignals);
      NumberSignals = new(comp.NumberSignals);
      VideoSignals = new(comp.VideoSignals);
      AudioSignals = new(comp.AudioSignals);
      CompositeSignals = new(comp.CompositeSignals);
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

   public ObservableCollection<SignalModel> VideoSignals
   {
      get => _videoSignals;
      set
      {
         _videoSignals = value;
         OnPropertyChanged();
      }
   }

   public ObservableCollection<SignalModel> AudioSignals
   {
      get => _audioSignals;
      set
      {
         _audioSignals = value;
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

   public ComponentType Type
   {
      get => _type;
      set
      {
         _type = value;
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
