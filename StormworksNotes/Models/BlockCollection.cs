using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using JsonReaderLibrary;

using MVVMLibrary;

using StormworksNotes.Models.Enums;

namespace StormworksNotes.Models;

public class BlockCollection : Model
{
   #region Local Props
   private ObservableCollection<ComponentModel> _blocks = [];
   private static Regex CleanerRegex = new("<voxels>.*</voxels>", RegexOptions.Singleline);
   #endregion

   #region Constructors
   public BlockCollection() { }
   #endregion

   #region Methods
   public static BlockCollection? OpenBlocks(string path)
   {
      if (File.Exists(path))
      {
         return JsonReader.OpenJsonFile<BlockCollection>(path);
      }
      return null;
   }

   public void SaveBlocks(string path)
   {
      JsonReader.SaveJsonFile(path, this);
   }

   public static BlockCollection? ReadBlockData(string path)
   {
      if (Directory.Exists(path))
      {
         var files = Directory.GetFiles(path, "*.xml");

         if (files.Length != 0)
         {
            BlockCollection blocks = new();
            foreach (var file in files)
            {
               if (char.IsDigit(Path.GetFileName(file)[0])) continue;
               var fileData = CleanXmlData(file);
               if (string.IsNullOrEmpty(fileData)) continue;
               XmlDocument doc = new();
               doc.LoadXml(fileData);
               var defNode = doc.GetElementsByTagName("definition");
               if (defNode?.Count != 1) continue;

               XmlNodeList logicNodes = doc.GetElementsByTagName("logic_nodes");
               if (logicNodes?.Count != 1) continue;
               if (logicNodes[0]?.HasChildNodes == true)
               {
                  var compName = defNode[0]?.Attributes?.GetNamedItem("name")?.Value;
                  var compDesc = defNode[0]?.SelectSingleNode("tooltip_properties")?.Attributes?.GetNamedItem("short_description")?.Value;
                  ComponentModel component = new()
                  {
                     Name = compName,
                     Description = compDesc,
                     Type = ComponentType.BLOCK,
                  };
                  foreach (XmlNode logic in logicNodes[0]!.ChildNodes)
                  {
                     int modeId = 0;
                     var name = logic.Attributes?.GetNamedItem("label")?.Value;
                     var desc = logic.Attributes?.GetNamedItem("description")?.Value;
                     var mode = logic.Attributes?.GetNamedItem("mode")?.Value;
                     var type = logic.Attributes?.GetNamedItem("type")?.Value;
                     if (int.TryParse(type, out int logicType))
                     {
                        if (!int.TryParse(mode, out modeId))
                        {
                           modeId = 0;
                        }
                        switch (logicType)
                        {
                           default:
                              break;
                           case 0:
                              component.BoolSignals.Add(new()
                              {
                                 Name = name,
                                 Description = desc,
                                 Direction = (SignalDirection)modeId,
                                 Type = SignalType.ON_OFF,
                              });
                              break;
                           case 1:
                              component.NumberSignals.Add(new()
                              {
                                 Name = name,
                                 Description = desc,
                                 Direction = (SignalDirection)modeId,
                                 Type = SignalType.NUMBER,
                              });
                              break;
                           case 5:
                              component.CompositeSignals.Add(new()
                              {
                                 Name = name,
                                 Description = desc,
                                 Direction = (SignalDirection)modeId,
                              });
                              break;
                           case 6:
                              component.VideoSignals.Add(new()
                              {
                                 Name = name,
                                 Description = desc,
                                 Direction = (SignalDirection)modeId,
                                 Type = SignalType.VIDEO,
                              });
                              break;
                           case 7:
                              component.AudioSignals.Add(new()
                              {
                                 Name = name,
                                 Description = desc,
                                 Direction = (SignalDirection)modeId,
                                 Type = SignalType.AUDIO,
                              });
                              break;
                        }
                     }
                  }

                  if (component.AudioSignals.Count == 0 && component.VideoSignals.Count == 0 && component.BoolSignals.Count == 0 && component.NumberSignals.Count == 0 && component.CompositeSignals.Count == 0)
                     continue;

                  blocks.Blocks.Add(component);
               }
            }

            return blocks;
         }
      }
      return null;
   }

   private static string? CleanXmlData(string filePath)
   {
      using StreamReader reader = new StreamReader(filePath);
      string xml = reader.ReadToEnd();
      return CleanerRegex.Replace(xml, "");
   }
   #endregion

   #region Full Props
   public ObservableCollection<ComponentModel> Blocks
   {
      get => _blocks;
      set
      {
         _blocks = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
