using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DukeNukemProject.Model;
using DukeNukemProject.ViewModel;
using DukeNukemProject.ViewModel.Commands;
using System.Windows.Input;
using DukeNukemProject.View;

namespace DukeNukemProject.ViewModel
{
    class WindowMenuViewModel
    {
        private ParameterlessCommand popupMenuManageCommand;
        public WindowMenuViewModel()
        {
            popupMenuManageCommand = new ParameterlessCommand(popupMenuManage, canPopupMenuManage);
            
        }

        public ICommand PopupMenuCommand
        {
            get { return popupMenuManageCommand; }

        }

        public void popupMenuManage()
        {
            MenuView mv = new MenuView();
            mv.Show();
        }
        public bool canPopupMenuManage()
        {
            return true;
        }

        
    }
}
