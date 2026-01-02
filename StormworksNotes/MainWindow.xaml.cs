using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using StormworksNotes.Models;
using StormworksNotes.ViewModels;
using StormworksNotes.Views;

namespace StormworksNotes;

public partial class MainWindow : Window
{
   private readonly MainViewModel VM = null!;
   public MainWindow()
   {
      VM = App.MainVM;
      DataContext = VM;
      InitializeComponent();
      Closing += VM.MainWindow_Closing;
   }

   private void ComponentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
   }

   private void VehicleDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
   {
      if (sender is DataGrid dg)
      {
         if (VM.SelectedComponent != null)
         {
            if ((string)e.Column.Header == "Ch" && e.EditAction == DataGridEditAction.Commit)
            {
               if (e.Row.Item is SignalModel item)
               {
                  if (item.Channel >= 32 || item.Channel <= 0)
                  {
                     e.Cancel = true;
                     return;
                  }
                  if (dg.ItemsSource is IList<SignalModel> list)
                  {
                     VM.CompChannelEditChanged(VM.SelectedComponent, list, item);
                  }
               }
            }
         }
      }
   }
   private void BlocksDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
   {
      if (sender is DataGrid dg)
      {
         if (VM.SelectedEditBlock != null)
         {
            if ((string)e.Column.Header == "Ch" && e.EditAction == DataGridEditAction.Commit)
            {
               if (e.Row.Item is SignalModel item)
               {
                  if (item.Channel >= 32 || item.Channel <= 0)
                  {
                     e.Cancel = true;
                     return;
                  }
                  if (dg.ItemsSource is IList<SignalModel> list)
                  {
                     VM.CompChannelEditChanged(VM.SelectedEditBlock, list, item);
                  }
               }
            }
         }
      }
   }
   private void McusDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
   {
      if (sender is DataGrid dg)
      {
         if (VM.SelectedEditMcu != null)
         {
            if ((string)e.Column.Header == "Ch" && e.EditAction == DataGridEditAction.Commit)
            {
               if (e.Row.Item is SignalModel item)
               {
                  if (item.Channel >= 32 || item.Channel <= 0)
                  {
                     e.Cancel = true;
                     return;
                  }
                  if (dg.ItemsSource is IList<SignalModel> list)
                  {
                     VM.CompChannelEditChanged(VM.SelectedEditMcu, list, item);
                  }
               }
            }
         }
      }
   }

   private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
   {
      var settingsVM = new SettingsViewModel(App.Settings);
      var settingsDialog = new SettingsDialog(settingsVM);
      if (settingsDialog.ShowDialog() == true)
      {
         App.SaveSettings(settingsVM.Settings);
      }
   }
}