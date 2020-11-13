using System;
using System.Windows.Input;

namespace APS_02.ViewModels
{
    public class Command : ICommand
    {
        public Command(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public Command(Action execute) : this(execute, () => true) { }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => canExecute();

        public void Execute(object parameter) => execute();

        private Action execute;
        private Func<bool> canExecute;
    }
}