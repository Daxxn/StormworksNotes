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
   private VehicleModel? _vehicle = null;
   private ComponentModel? _selectedComp = null;
   private ComponentModel? _selectedBlock = null;
   private ComponentModel? _selectedMcu = null;
   private ComponentModel? _selectedEditBlock = null;
   private ComponentModel? _selectedEditMcu = null;
   private SavedMCUs? _savedMCUs = new();
   private BlockCollection? _blocks = null;
   private int _newSignalTypeIndex = 0;
   private string? _blockSearchText = null;

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

   public Command ReadBlocksCmd { get; init; }
   public Command OpenBlocksCmd { get; init; }
   public Command SaveBlocksCmd { get; init; }
   public Command AddBlockToProjCmd { get; init; }
   public Command SearchBlocksCmd { get; init; }
   public Command SortBlocksCmd { get; init; }

   public Command ReadSavedMcusCmd { get; init; }
   public Command OpenSavedMcusCmd { get; init; }
   public Command SaveMcusCmd { get; init; }
   public Command AddMcuToProjCmd { get; init; }
   public Command AddToSavedMcusCmd { get; init; }
   public Command AppendSavedMcusCmd { get; init; }
   public Command RemoveMcuCmd { get; init; }
   public Command ReplaceMcuCmd { get; init; }
   public Command ReplaceCompCmd { get; init; }
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
      DeleteComponentCmd = new(DeleteComponent);
      AddSignalCmd = new(AddSignal);
      DeleteSignalCmd = new(DeleteSignal);
      DeleteCompositeCmd = new(DeleteComposite);

      ReadBlocksCmd = new(ReadBlocks);
      OpenBlocksCmd = new(OpenBlocks);
      SaveBlocksCmd = new(SaveBlocks);
      AddBlockToProjCmd = new(AddBlockToProject);
      SearchBlocksCmd = new(SearchBlocks);
      SortBlocksCmd = new(SortBlocks);

      ReadSavedMcusCmd = new(ReadSavedMcus);
      OpenSavedMcusCmd = new(OpenSavedMcus);
      SaveMcusCmd = new(SaveMcus);
      AddMcuToProjCmd = new(AddMcuToProj);
      AddToSavedMcusCmd = new(AddToSavedMcus);
      AppendSavedMcusCmd = new(AppendSavedMcus);
      RemoveMcuCmd = new(RemoveMcu);
      ReplaceMcuCmd = new(ReplaceMcu);
      ReplaceCompCmd = new(ReplaceComp);
   }
   #endregion

   #region Methods
   private void NewVehicle()
   {
      Vehicle = new VehicleModel();
      SelectedComponent = null;
      App.Settings.SavePath = null;
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
            InitialDirectory = App.Settings.VehicleSaveDir,
         };

         if (dialog.ShowDialog() == true)
         {
            Vehicle = JsonReader.OpenJsonFile<VehicleModel>(dialog.FileName);
            App.Settings.SavePath = dialog.FileName;
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
         if (File.Exists(App.Settings.SavePath))
         {
            JsonReader.SaveJsonFile(App.Settings.SavePath, Vehicle);
         }
         else
         {
            SaveFileDialog dialog = new()
            {
               DefaultExt = ".json",
               Filter = "Vehicle File|*.json|All Files|*.*",
               OverwritePrompt = true,
               InitialDirectory = App.Settings.VehicleSaveDir,
               FileName = Vehicle.Name
            };
            if (dialog.ShowDialog() == true)
            {
               JsonReader.SaveJsonFile(dialog.FileName, Vehicle);
               App.Settings.SavePath = dialog.FileName;
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
            InitialDirectory = App.Settings.VehicleSaveDir,
            FileName = Vehicle.Name
         };
         if (dialog.ShowDialog() == true)
         {
            JsonReader.SaveJsonFile(dialog.FileName, Vehicle);
            App.Settings.SavePath = dialog.FileName;
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
      newComp.Type = ComponentType.MCU;
      Vehicle.Components.Add(newComp);
      SelectedComponent = newComp;
   }

   private void DeleteComponent()
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;

      Vehicle.Components.Remove(SelectedComponent);
      SelectedComponent = null;
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

   private void ReadBlocks()
   {
      if (!Directory.Exists(App.Settings.BlockDataDir))
      {
         MessageBox.Show("Unable to find Stormworks data folder.");
      }
      if (BlockData != null)
      {
         if (MessageBox.Show("This will overwrite the current blocks with the info from the game.\nAll composite signals will lose thier channel names as the data files do not contain anything about them.", "You Sure?", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
         {
            return;
         }
      }

      BlockData = BlockCollection.ReadBlockData(App.Settings.BlockDataDir!);
   }

   private void OpenBlocks()
   {
      try
      {
         if (!File.Exists(App.Settings.ComponentBlocksPath))
         {
            MessageBox.Show("Unable to find blocks file.");
         }
         else
         {
            OpenFileDialog dialog = new()
            {
               DefaultExt = ".swblk",
               Filter = "Blocks File|*.swblk|All Files|*.*",
               InitialDirectory = App.Settings.VehicleSaveDir,
               FileName = "Blocks.swblk"
            };

            if (dialog.ShowDialog() == true)
            {
               App.Settings.ComponentBlocksPath = dialog.FileName;
               BlockData = BlockCollection.OpenBlocks(App.Settings.ComponentBlocksPath!);
            }
         }
      }
      catch (Exception e)
      {
         MessageBox.Show($"Failed to open blocks. {e.Message}", "Block File Open Error");
      }
   }

   private void SaveBlocks()
   {
      if (BlockData is null) return;
      try
      {
         if (File.Exists(App.Settings.ComponentBlocksPath))
         {
            BlockData.SaveBlocks(App.Settings.ComponentBlocksPath);
         }
         else
         {
            SaveFileDialog dialog = new()
            {
               DefaultExt = ".swblk",
               Filter = "Blocks File|*.swblk|All Files|*.*",
               OverwritePrompt = true,
               InitialDirectory = App.Settings.VehicleSaveDir,
               FileName = "Blocks.swblk"
            };

            if (dialog.ShowDialog() == true)
            {
               App.Settings.ComponentBlocksPath = dialog.FileName;
               BlockData.SaveBlocks(App.Settings.ComponentBlocksPath);
            }
         }
      }
      catch (Exception e)
      {
         MessageBox.Show($"Failed to save blocks. {e.Message}", "Blocks File Save Error");
      }
   }

   private void AddBlockToProject()
   {
      if (Vehicle is null) return;
      if (SelectedBlock is null) return;

      Vehicle.Components.Add(SelectedBlock.Copy());
   }

   private void SearchBlocks()
   {
      if (BlockData is null) return;
      if (string.IsNullOrEmpty(BlockSearchText)) return;

      List<ComponentModel> searchResults = [];
      foreach (var block in BlockData.Blocks)
      {
         if (block.Name?.StartsWith(BlockSearchText) == true)
         {
            searchResults.Add(block);
         }
      }
      if (searchResults.Count > 0)
      {
         SelectedBlock = searchResults[0];
      }
   }

   private void SortBlocks()
   {
      if (BlockData is null) return;
      BlockData.Blocks = new(BlockData.Blocks.OrderBy(b => b.Name));
   }

   private void ReadSavedMcus()
   {
      if (App.Settings.GameDataDir is null) return;
      SavedMCUs = SavedMCUs.ReadMcus();
   }

   private void OpenSavedMcus()
   {
      SavedMCUs.Open(App.Settings.SavedMCUFilePath);
   }

   private void SaveMcus()
   {
      if (SavedMCUs is null) return;
      if (File.Exists(App.Settings.SavedMCUFilePath))
      {
         SavedMCUs.Save(App.Settings.SavedMCUFilePath);
      }
      else
      {
         SaveFileDialog dialog = new()
         {
            Title = "Save MCUs",
            AddExtension = true,
            DefaultExt = ".swmcu",
            InitialDirectory = App.Settings.VehicleSaveDir,
         };
         if (dialog.ShowDialog() == true)
         {
            App.Settings.SavedMCUFilePath = dialog.FileName;
            SavedMCUs.Save(App.Settings.SavedMCUFilePath);
         }
      }
   }

   private void AddMcuToProj()
   {
      if (Vehicle is null) return;
      if (SelectedMcu is null) return;

      Vehicle.Components.Add(SelectedMcu.Copy());
   }

   private void AddToSavedMcus()
   {
      if (Vehicle is null) return;
      if (SelectedComponent is null) return;
      if (SavedMCUs is null) return;

      var newComp = SelectedComponent.Copy();
      foreach (var mcu in SavedMCUs.MCUs)
      {
         if (SelectedComponent.Name == mcu.Name)
         {
            mcu.Replace(newComp);
            SelectedMcu = newComp;
            return;
         }
      }

      SavedMCUs.Add(newComp);
      SelectedMcu = newComp;
   }

   private void AppendSavedMcus()
   {
      if (SavedMCUs is null) return;
      SavedMCUs.AppendNewMcus();
   }

   private void RemoveMcu()
   {
      if (SavedMCUs is null) return;
      if (SelectedEditMcu is null) return;

      SavedMCUs.Remove(SelectedEditMcu);
   }

   private void ReplaceMcu()
   {
      if (SavedMCUs is null) return;
      if (SelectedMcu is null) return;
      if (SelectedComponent is null) return;

      if (SelectedComponent.Type == ComponentType.BLOCK) return;

      string? tempName = SelectedComponent.Name;
      if (!string.IsNullOrEmpty(SelectedMcu.Name))
      {
         tempName = SelectedMcu.Name;
      }
      SelectedMcu.Replace(SelectedComponent);
      SelectedMcu.Name = tempName;
   }

   private void ReplaceComp()
   {
      if (SavedMCUs is null) return;
      if (SelectedMcu is null) return;
      if (SelectedComponent is null) return;

      if (SelectedComponent.Type == ComponentType.BLOCK) return;

      SelectedComponent.Replace(SelectedMcu);
   }

   #region Events
   public void OnStartup()
   {
      if (App.Settings.AutoOpen)
      {
         if (File.Exists(App.Settings.SavePath))
         {
            Vehicle = JsonReader.OpenJsonFile<VehicleModel>(App.Settings.SavePath);
            if (Vehicle is null)
            {
               MessageBox.Show($"Unable to read saved vehicle from {App.Settings.SavePath}.");
               NewVehicle();
            }
         }
         else
         {
            NewVehicle();
         }

         if (App.Settings.ComponentBlocksPath != null)
         {
            BlockData = BlockCollection.OpenBlocks(App.Settings.ComponentBlocksPath);
         }

         if (App.Settings.SavedMCUFilePath != null)
         {
            SavedMCUs = SavedMCUs.Open(App.Settings.SavedMCUFilePath);
            if (SavedMCUs is null)
            {
               SavedMCUs = SavedMCUs.ReadMcus();
            }
         }
      }
      else
      {
         NewVehicle();
      }
   }

   public void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
   {
      if (App.Settings.SaveOnClose)
      {
         if (Vehicle is null) return;

         if (File.Exists(App.Settings.SavePath))
         {
            Vehicle = JsonReader.OpenJsonFile<VehicleModel>(App.Settings.SavePath);
            if (Vehicle is null)
            {
               MessageBox.Show($"Unable to read saved vehicle from {App.Settings.SavePath}.");
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

         if (BlockData != null && App.Settings.ComponentBlocksPath != null)
         {
            BlockData.SaveBlocks(App.Settings.ComponentBlocksPath);
         }

         if (SavedMCUs != null && App.Settings.SavedMCUFilePath != null)
         {
            SavedMCUs.Save(App.Settings.SavedMCUFilePath);
         }
      }
   }

   public void CompChannelEditChanged(ComponentModel selectedComponent, IList<SignalModel> senderData, SignalModel item)
   {
      if (Vehicle is null) return;
      if (selectedComponent is null) return;
      if (selectedComponent.SelectedComposite is null) return;
      if (senderData == selectedComponent.SelectedComposite.BoolSignals)
      {
         SwapChannel(selectedComponent.SelectedComposite.BoolSignals, item);
      }
      else if (senderData == selectedComponent.SelectedComposite.NumberSignals)
      {
         SwapChannel(selectedComponent.SelectedComposite.NumberSignals, item);
      }
      selectedComponent.SelectedComposite.SortList(senderData);
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

   public ComponentModel? SelectedBlock
   {
      get => _selectedBlock;
      set
      {
         _selectedBlock = value;
         OnPropertyChanged();
      }
   }

   public ComponentModel? SelectedMcu
   {
      get => _selectedMcu;
      set
      {
         _selectedMcu = value;
         OnPropertyChanged();
      }
   }

   public ComponentModel? SelectedEditBlock
   {
      get => _selectedEditBlock;
      set
      {
         _selectedEditBlock = value;
         OnPropertyChanged();
      }
   }

   public ComponentModel? SelectedEditMcu
   {
      get => _selectedEditMcu;
      set
      {
         _selectedEditMcu = value;
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

   public SavedMCUs? SavedMCUs
   {
      get => _savedMCUs;
      set
      {
         _savedMCUs = value;
         OnPropertyChanged();
      }
   }

   public BlockCollection? BlockData
   {
      get => _blocks;
      set
      {
         _blocks = value;
         OnPropertyChanged();
      }
   }

   public string? BlockSearchText
   {
      get => _blockSearchText;
      set
      {
         _blockSearchText = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
