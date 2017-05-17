using System;
using System.Windows.Input;

namespace DukeNukemProject.ViewModel.Commands
{
    class ParameterlessCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action WhatToExecute;
        private Func<bool> WhenToExecute;

        public ParameterlessCommand(Action what, Func<bool> when)
        {
            WhatToExecute = what;
            WhenToExecute = when;
        }

        public bool CanExecute(object parameter)
        {
            return WhenToExecute();
        }

        public void Execute(object parameter)
        {
            WhatToExecute();
        }
    }
}
