using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using StormworksNotes.Models;
using StormworksNotes.ViewModels;

namespace StormworksNotes.Views;

public partial class SettingsDialog : Window
{
   private readonly SettingsViewModel VM;
   public SettingsDialog(SettingsViewModel vm)
   {
      VM = vm;
      DataContext = VM;
      InitializeComponent();
   }

   private void Cancel_Click(object sender, RoutedEventArgs e)
   {
      DialogResult = false;
      Close();
   }

   private void Save_Click(object sender, RoutedEventArgs e)
   {
      DialogResult = true;
      Close();
   }
}
