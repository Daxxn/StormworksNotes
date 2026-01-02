using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

using SettingsLibrary.Models;

namespace StormworksNotes.Models;
public class SettingsModel : Model, ISettingsModel
{
   #region Local Props
   private string? _savePath = null;
   public string? LastSavePath { get; set; }
   private bool _autoOpen = true;
   private bool _saveOnClose = false;
   private string? _vehicleSaveDir = @"F:\Code\Lua\StormWorks\VehicleNotes";
   private string? _gameDataDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Stormworks\data";
   private string? _savedMCUFilePath = @"F:\Code\Lua\StormWorks\VehicleNotes\SavedMCUs.swmcu";
   private string? _compBlocksPath = @"F:\Code\Lua\StormWorks\VehicleNotes\ComponentTemplates.swblk";
   private string? _blockDataDir = @"E:\SteamLibrary\steamapps\common\Stormworks\rom\data\definitions";
   #endregion

   #region Constructors
   public SettingsModel() { }

   #endregion

   #region Methods
   public void Copy(SettingsModel current)
   {
      SavePath = current.SavePath;
      LastSavePath = current.LastSavePath;
      AutoOpen = current.AutoOpen;
      SaveOnClose = current.SaveOnClose;
      VehicleSaveDir = current.VehicleSaveDir;
      GameDataDir = current.GameDataDir;
      SavedMCUFilePath = current.SavedMCUFilePath;
      ComponentBlocksPath = current.ComponentBlocksPath;
      BlockDataDir = current.BlockDataDir;
   }
   #endregion

   #region Full Props
   public string? SavePath
   {
      get => _savePath;
      set
      {
         _savePath = value;
         OnPropertyChanged();
      }
   }

   public bool AutoOpen
   {
      get => _autoOpen;
      set
      {
         _autoOpen = value;
         OnPropertyChanged();
      }
   }

   public bool SaveOnClose
   {
      get => _saveOnClose;
      set
      {
         _saveOnClose = value;
         OnPropertyChanged();
      }
   }

   public string? VehicleSaveDir
   {
      get => _vehicleSaveDir;
      set
      {
         _vehicleSaveDir = value;
         OnPropertyChanged();
      }
   }

   public string? SavedMCUFilePath
   {
      get => _savedMCUFilePath;
      set
      {
         _savedMCUFilePath = value;
         OnPropertyChanged();
      }
   }

   public string? GameDataDir
   {
      get => _gameDataDir;
      set
      {
         _gameDataDir = value;
         OnPropertyChanged();
      }
   }

   public string? ComponentBlocksPath
   {
      get => _compBlocksPath;
      set
      {
         _compBlocksPath = value;
         OnPropertyChanged();
      }
   }

   public string? BlockDataDir
   {
      get => _blockDataDir;
      set
      {
         _blockDataDir = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
