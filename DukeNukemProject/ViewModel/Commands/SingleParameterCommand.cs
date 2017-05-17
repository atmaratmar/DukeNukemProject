using System;
using System.Windows.Input;

namespace DukeNukemProject.ViewModel.Commands
{
    public class SingleParameterCommand : ICommand
    {
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action<object> WhatToExecute;
        private Func<bool> WhenToExecute;

        public SingleParameterCommand(Action<object> what, Func<bool> when)
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
            WhatToExecute(parameter);
        }
        
    }
}
