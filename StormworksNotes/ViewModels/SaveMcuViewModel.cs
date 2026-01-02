using MVVMLibrary;

using StormworksNotes.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormworksNotes.ViewModels;

public class SaveMcuViewModel : ViewModel
{
   #region Local Props
   private readonly ComponentModel _selectedComponent;
   #region Commands

   #endregion
   #endregion

   #region Constructors
   public SaveMcuViewModel(ComponentModel selectedComponent)
   {
      _selectedComponent = selectedComponent;
   }
   #endregion

   #region Methods

   #region Events

   #endregion
   #endregion

   #region Full Props
   public ComponentModel SelectedComponent
   {
      get => _selectedComponent;
   }
   #endregion
}
