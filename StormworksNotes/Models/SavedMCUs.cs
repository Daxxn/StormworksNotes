using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using JsonReaderLibrary;
using MVVMLibrary;

using StormworksNotes.Models.Enums;

namespace StormworksNotes.Models;

public class SavedMCUs : Model
{
   #region Local Props
   private ObservableCollection<ComponentModel> _mcus = [];
   private static Regex CleanerRegex = new("<v.*/>");
   #endregion

   #region Constructors
   public SavedMCUs() { }
   #endregion

   #region Methods
   public static SavedMCUs? ReadMcus()
   {
      var _settings = App.Settings;
      if (_settings is null) return null;
      if (Directory.Exists(_settings.GameDataDir))
      {
         var mcuFolder = Path.Combine(_settings.GameDataDir, "microprocessors");
         var files = Directory.GetFiles(mcuFolder, "*.xml");
         if (files.Any())
         {
            var mcus = new SavedMCUs();
            foreach (var file in files)
            {
               var fileData = CleanXml(file);
               if (string.IsNullOrEmpty(fileData)) continue;
               XmlDocument doc = new();
               doc.LoadXml(fileData);
               var mcuNodes = doc.GetElementsByTagName("microprocessor");
               if (mcuNodes?.Count != 1) continue;
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
                           SignalType signalType = SignalType.ON_OFF;

                           switch (innerNode?.Attributes?.GetNamedItem("type")?.Value)
                           {
                              default:
                                 signalType = SignalType.ON_OFF;
                                 newMCU.BoolSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = signalType,
                                    Channel = pinChannel != null ? int.Parse(pinChannel) : 0,
                                 });
                                 break;
                              case "1":
                                 signalType = SignalType.NUMBER;
                                 newMCU.NumberSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = signalType,
                                    Channel = pinChannel != null ? int.Parse(pinChannel) : 0,
                                 });
                                 break;
                              case "5":
                                 signalType = SignalType.COMPOSITE;
                                 newMCU.CompositeSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = (CompositeSignalDirection)signalDir,
                                 });
                                 break;
                              case "6":
                                 signalType = SignalType.VIDEO;
                                 newMCU.VideoSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = signalType,
                                 });
                                 break;
                              case "7":
                                 signalType = SignalType.AUDIO;
                                 newMCU.AudioSignals.Add(new()
                                 {
                                    Name = pinName,
                                    Description = pinDesc,
                                    Direction = signalDir,
                                    Type = signalType,
                                 });
                                 break;
                           }
                        }
                     }
                  }
               }

               mcus.MCUs.Add(newMCU);
            }

            return mcus;
         }
      }

      return null;
   }

   private static string? CleanXml(string filePath)
   {
      using StreamReader reader = new(filePath);
      var fileData = reader.ReadToEnd();
      return CleanerRegex.Replace(fileData, "");
   }

   public void AppendNewMcus()
   {
      var savedMCUs = ReadMcus();
      if (savedMCUs is null) return;
      foreach (var mcu in savedMCUs.MCUs)
      {
         var foundMcu = MCUs.FirstOrDefault(m => m.Name == mcu.Name);
         if (foundMcu != null)
         {
            mcu.Replace(foundMcu);
         }
      }
   }

   public void Save(string path)
   {
      JsonReader.SaveJsonFile(path, this);
   }

   public static SavedMCUs? Open(string? path)
   {
      if (File.Exists(path))
      {
         return JsonReader.OpenJsonFile<SavedMCUs>(path);
      }
      return null;
   }

   public void Add(ComponentModel newMcu)
   {
      MCUs.Add(newMcu.Copy());
   }

   public bool Remove(ComponentModel mcu)
   {
      return MCUs.Remove(mcu);
   }
   #endregion

   #region Full Props
   public ObservableCollection<ComponentModel> MCUs
   {
      get => _mcus;
      set
      {
         _mcus = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
