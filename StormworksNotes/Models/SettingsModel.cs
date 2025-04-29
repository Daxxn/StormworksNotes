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
   #endregion

   #region Constructors
   public SettingsModel() { }

   #endregion

   #region Methods

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
   #endregion
}
