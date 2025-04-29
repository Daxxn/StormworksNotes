using System.Configuration;
using System.Data;
using System.Windows;

using SettingsLibrary;

using StormworksNotes.Models;
using StormworksNotes.ViewModels;

namespace StormworksNotes;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
   public static SettingsModel Settings { get; private set; } = new();
   public static MainViewModel MainVM { get; private set; } = new();
   protected override void OnStartup(StartupEventArgs e)
   {
      Settings = SettingsManager.OnStartup<SettingsModel>(nameof(StormworksNotes));
      MainVM.OnStartup();
      base.OnStartup(e);
   }

   protected override void OnExit(ExitEventArgs e)
   {
      SettingsManager.OnExit(Settings, nameof(StormworksNotes));
      base.OnExit(e);
   }
}

