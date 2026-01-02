using Microsoft.Win32;

using MVVMLibrary;

using StormworksNotes.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormworksNotes.ViewModels;

public class SettingsViewModel : ViewModel
{
   #region Local Props
   private SettingsModel _settings;
   #region Commands
   public Command BrowseFolderCmd { get; init; }
   public Command BrowseFileCmd { get; init; }
   #endregion
   #endregion

   #region Constructors
   public SettingsViewModel(SettingsModel currentSettings)
   {
      _settings = new();
      _settings.Copy(currentSettings);

      BrowseFolderCmd = new Command(BrowseFolder);
      BrowseFileCmd = new Command(BrowseFile);
   }
   #endregion

   #region Methods
   private void BrowseFolder(object param)
   {
      if (param is string p)
      {
         OpenFolderDialog dialog = new()
         {
            Title = $"Select {p} Folder",
            Multiselect = false,
         };

         if (dialog.ShowDialog() == true)
         {
            switch (p)
            {
               case "Vehicles Folder":
                  Settings.VehicleSaveDir = dialog.FolderName;
                  break;
               case "Game Data":
                  Settings.GameDataDir = dialog.FolderName;
                  break;
               case "Block Data":
                  Settings.BlockDataDir = dialog.FolderName;
                  break;
               default:
                  break;
            }
         }
      }
   }

   private void BrowseFile(object param)
   {
      if (param is string p)
      {
         OpenFileDialog dialog = new()
         {
            Title = $"Select {p} File",
            Multiselect = false,
         };

         if (dialog.ShowDialog() == true)
         {
            switch (p)
            {
               case "Saved MCUs":
                  Settings.SavedMCUFilePath = dialog.FileName;
                  break;
               case "Blocks":
                  Settings.ComponentBlocksPath = dialog.FileName;
                  break;
               default:
                  break;
            }
         }
      }
   }
   #region Events

   #endregion
   #endregion

   #region Full Props
   public SettingsModel Settings
   {
      get => _settings;
      set
      {
         _settings = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
