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

namespace StormworksNotes;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   private readonly MainViewModel VM = null!;
   private bool ReorderDataGrid { get; set; }
   public MainWindow()
   {
      VM = App.MainVM;
      DataContext = VM;
      InitializeComponent();
      Closing += VM.MainWindow_Closing;
   }

   private void ComponentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      //VM.SelectedComposite = null;
   }

   private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
   {
      if (sender is DataGrid dg)
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
                  VM.CompChannelEditChanged(list, item);
                  //dg.ItemsSource = list.OrderBy(x => x.Channel);
                  //ReorderDataGrid = true;
               }
            }
         }
      }
   }
}