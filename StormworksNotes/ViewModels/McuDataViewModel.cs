using MVVMLibrary;

using StormworksNotes.Models;
using StormworksNotes.Models.Enums;

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StormworksNotes.ViewModels;

public class McuDataViewModel : ViewModel
{
   #region Local Props
   private SettingsModel _settings = null!;
   private ObservableCollection<ComponentModel> _mcus = [];
   #region Commands

   #endregion
   #endregion

   #region Constructors
   public McuDataViewModel() { }
   #endregion

   #region Methods
   public void OnStartup()
   {
      _settings = App.Settings;
      if (_settings is null) return;
      if (Directory.Exists(_settings.GameDataDir)) {
         var files = Directory.GetFiles(_settings.GameDataDir, "*.xml");
         if (files.Length != 0)
         {
            foreach (var file in files)
            {
               XmlDocument doc = new();
               doc.Load(file);
               var mcuNodes = doc.GetElementsByTagName("microprocessor");
               if (mcuNodes?.Count != 1) throw new Exception($"Failed to find MCU node in {Path.GetFileName(file)}.");
               var name = mcuNodes[0]!.Attributes?.GetNamedItem("name")?.Value;
               var desc = mcuNodes[0]!.Attributes?.GetNamedItem("description")?.Value;
               var mcuIONodes = mcuNodes[0]!.SelectSingleNode("nodes");
               var newMCU = new ComponentModel()
               {
                  Name = name,
                  Description = desc,
                  Type = ComponentType.MCU
               };
               if (mcuIONodes?.ChildNodes?.Count > 0)
               {
                  foreach (XmlNode ioNode in mcuIONodes.ChildNodes)
                  {
                     if (ioNode.Name == "n")
                     {
                        var innerNode = ioNode.SelectSingleNode("node");
                        if (innerNode != null)
                        {
                           var pinName = innerNode?.Attributes?.GetNamedItem("label")?.Value;
                           var pinDesc = innerNode?.Attributes?.GetNamedItem("description")?.Value;
                           var signalDir = (innerNode?.Attributes?.GetNamedItem("mode")?.Value == "1") ? SignalDirection.OUTPUT : SignalDirection.INPUT;
                           var pinChannel = innerNode?.Attributes?.GetNamedItem("id")?.Value;

                           switch (innerNode?.Attributes?.GetNamedItem("type")?.Value)
                           {
                              default:
                                 newMCU.BoolSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = SignalType.ON_OFF,
                                    Channel = pinChannel != null ? int.Parse(pinChannel) : 0,
                                 });
                                 break;
                              case "1":
                                 newMCU.NumberSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = SignalType.NUMBER,
                                    Channel = pinChannel != null ? int.Parse(pinChannel) : 0,
                                 });
                                 break;
                              case "5":
                                 newMCU.CompositeSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = (CompositeSignalDirection)signalDir,
                                 });
                                 break;
                              case "6":
                                 newMCU.VideoSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = SignalType.VIDEO,
                                 });
                                 break;
                              case "7":
                                 newMCU.AudioSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = SignalType.AUDIO,
                                 });
                                 break;
                           }
                        }
                     }
                  }
               }
            }
         }
      }
   }
   #region Events

   #endregion
   #endregion

   #region Full Props

   #endregion
}
