using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using JsonReaderLibrary;

using Microsoft.Win32;

using MVVMLibrary;

using StormworksNotes.Models;
using StormworksNotes.Models.Enums;

namespace StormworksNotes.ViewModels;
public class MainViewModel : ViewModel
{
   #region Local Props
   private SettingsModel _settings = null!;
   private VehicleModel? _vehicle = null;
   private ComponentModel? _selectedComp = null;
   private int _newSignalTypeIndex = 0;

   #region Commands
   public Command NewVehicleCmd { get; init; }
   public Command OpenVehicleCmd { get; init; }
   public Command SaveVehicleCmd { get; init; }
   public Command SaveAsVehicleCmd { get; init; }

   public Command AddComponentCmd { get; init; }
   public Command DeleteComponentCmd { get; init; }
   public Command AddSignalCmd { get; init; }
   public Command DeleteSignalCmd { get; init; }
   public Command DeleteCompositeCmd { get; init; }

   #endregion
   #endregion

   #region Constructors
   public MainViewModel()
   {
      NewVehicleCmd = new(NewVehicle);
      OpenVehicleCmd = new(OpenVehicle);
      SaveVehicleCmd = new(SaveVehicle);
      SaveAsVehicleCmd = new(SaveAsVehicle);

      AddComponentCmd = new(AddComponent);
      DeleteComponentCmd = new(DeleteSignal);
      AddSignalCmd = new(AddSignal);
      DeleteSignalCmd = new(DeleteSignal);
      DeleteCompositeCmd = new(DeleteComposite);
   }
   #endregion

   #region Methods
   private void NewVehicle()
   {
      Vehicle = new VehicleModel();
      SelectedComponent = null;
      _settings.SavePath = null;
   }

   private void OpenVehicle()
   {
      try
      {
         OpenFileDialog dialog = new()
         {
            AddExtension = true,
            DefaultExt = ".json",
            CheckFileExists = true,
            Filter = "Vehicle File|*.json|All Files|*.*",
            Multiselect = false,
            InitialDirectory = _settings.VehicleSaveDir,
         };

         if (dialog.ShowDialog() == true)
         {
            Vehicle = JsonReader.OpenJsonFile<VehicleModel>(dialog.FileName);
            _settings.SavePath = dialog.FileName;
         }
      }
      catch (Exception e)
      {
         MessageBox.Show(e.Message, "Open Vehicle File Error");
      }
   }

   private void SaveVehicle()
   {
      try
      {
         if (Vehicle is null) return;
         if (File.Exists(_settings.SavePath))
         {
            JsonReader.SaveJsonFile(_settings.SavePath, Vehicle);
         }
         else
         {
            SaveFileDialog dialog = new()
            {
               DefaultExt = ".json",
               Filter = "Vehicle File|*.json|All Files|*.*",
               OverwritePrompt = true,
               InitialDirectory = _settings.VehicleSaveDir,
               FileName = Vehicle.Name
            };
            if (dialog.ShowDialog() == true)
            {
               JsonReader.SaveJsonFile(dialog.FileName, Vehicle);
               _settings.SavePath = dialog.FileName;
            }
         }
      }
      catch (Exception e)
      {
         MessageBox.Show(e.Message, "Save Vehicle File Error");
      }
   }

   private void SaveAsVehicle()
   {
      try
      {
         if (Vehicle is null) return;
         SaveFileDialog dialog = new()
         {
            DefaultExt = ".json",
            Filter = "Vehicle File|*.json|All Files|*.*",
            OverwritePrompt = true,
            InitialDirectory = _settings.VehicleSaveDir,
            FileName = Vehicle.Name
         };
         if (dialog.ShowDialog() == true)
         {
            JsonReader.SaveJsonFile(dialog.FileName, Vehicle);
            _settings.SavePath = dialog.FileName;
         }
      }
      catch (Exception e)
      {
         MessageBox.Show(e.Message, "Save Vehicle As File Error");
      }
   }

   private void AddComponent()
   {
      if (Vehicle is null) return;
      ComponentModel newComp = new();
      Vehicle.Components.Add(newComp);
      SelectedComponent = newComp;
   }

   private void DeleteComponent()
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;

      Vehicle.Components.Remove(SelectedComponent);
   }

   private void AddSignal(object param)
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;

      if (param is string name)
      {
         switch (name)
         {
            case "bool":
               SignalModel newBool = SignalModel.Create(SelectedComponent.BoolSignals.Count + 1, CompositeSignalType.ON_OFF);
               SelectedComponent.BoolSignals.Add(newBool);
               SelectedComponent.SelectedSignal = newBool;
               break;
            case "number":
               SignalModel newNum = SignalModel.Create(SelectedComponent.NumberSignals.Count + 1, CompositeSignalType.NUMBER);
               SelectedComponent.NumberSignals.Add(newNum);
               SelectedComponent.SelectedSignal = newNum;
               break;
            case "composite":
               CompositeModel newComp = new();
               SelectedComponent.CompositeSignals.Add(newComp);
               SelectedComponent.SelectedComposite = newComp;
               break;
            default:
               break;
         }
      }
   }

   private void DeleteSignal()
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;
      if (SelectedComponent.SelectedSignal is null) return;

      if (SelectedComponent.BoolSignals.Remove(SelectedComponent.SelectedSignal))
      {
         SelectedComponent.SelectedSignal = null;
         return;
      }
      SelectedComponent.NumberSignals.Remove(SelectedComponent.SelectedSignal);
      SelectedComponent.SelectedSignal = null;
   }

   private void DeleteComposite()
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;
      if (SelectedComponent.SelectedComposite is null) return;

      SelectedComponent.CompositeSignals.Remove(SelectedComponent.SelectedComposite);
      SelectedComponent.SelectedComposite = null;
   }

   #region Events
   public void OnStartup()
   {
      _settings = App.Settings;
      if (_settings.AutoOpen)
      {
         if (File.Exists(_settings.SavePath))
         {
            Vehicle = JsonReader.OpenJsonFile<VehicleModel>(_settings.SavePath);
            if (Vehicle is null)
            {
               MessageBox.Show($"Unable to read saved vehicle from {_settings.SavePath}.");
               NewVehicle();
            }
         }
         else
         {
            NewVehicle();
         }
      }
      else
      {
         NewVehicle();
      }
   }

   public void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
   {
      if (_settings.SaveOnClose)
      {
         if (Vehicle is null) return;

         if (File.Exists(_settings.SavePath))
         {
            Vehicle = JsonReader.OpenJsonFile<VehicleModel>(_settings.SavePath);
            if (Vehicle is null)
            {
               MessageBox.Show($"Unable to read saved vehicle from {_settings.SavePath}.");
               NewVehicle();
            }
         }
         else
         {
            var result = MessageBox.Show("Vehicle not saved. Save the vehicle before closing?", "WOAH!", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
               SaveAsVehicle();
            }
            else if (result == MessageBoxResult.No)
            {
               return;
            }
            else if (result == MessageBoxResult.Cancel)
            {
               e.Cancel = true;
               return;
            }
         }
      }
   }

   public void CompChannelEditChanged(IList<SignalModel> senderData, SignalModel item)
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;
      if (SelectedComponent.SelectedComposite is null) return;
      if (senderData == SelectedComponent.SelectedComposite.BoolSignals)
      {
         SwapChannel(SelectedComponent.SelectedComposite.BoolSignals, item);
      }
      else if (senderData == SelectedComponent.SelectedComposite.NumberSignals)
      {
         SwapChannel(SelectedComponent.SelectedComposite.NumberSignals, item);
      }
      SelectedComponent.SelectedComposite.SortList(senderData);
   }
   #endregion

   #region Util Methods
   private void SwapChannel(SignalModel[] listData, SignalModel signal)
   {
      int missingCh = 0;
      SignalModel? found = null;
      for (int i = 0; i < ProjConstants.COMP_SIGNAL_LEN; i++)
      {
         if (listData[i] == signal)
         {
            missingCh = i + 1;
         }
         if (listData[i] != signal && listData[i].Channel == signal.Channel)
         {
            found = listData[i];
         }
      }
      if (missingCh != 0 && found != null)
      {
         found.Channel = missingCh;
      }
      //listData = listData.OrderBy(x => x.Channel).ToArray();
   }
   #endregion
   #endregion

   #region Full Props
   public VehicleModel? Vehicle
   {
      get => _vehicle;
      set
      {
         _vehicle = value;
         OnPropertyChanged();
      }
   }

   public ComponentModel? SelectedComponent
   {
      get => _selectedComp;
      set
      {
         _selectedComp = value;
         OnPropertyChanged();
      }
   }

   public int NewSignalTypeIndex
   {
      get => _newSignalTypeIndex;
      set
      {
         _newSignalTypeIndex = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
