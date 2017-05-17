using System;
using System.Windows.Input;

namespace DukeNukemProject.ViewModel.Commands
{
    public class ParameterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action<object[]> WhatToExecute;
        private Func<bool> WhenToExecute;

        public ParameterCommand(Action<object[]> what, Func<bool> when)
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
            var valuesArray = (object[])parameter;
            WhatToExecute(valuesArray);
        }
    }
}
