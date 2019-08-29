using System;
using System.Windows.Input;

namespace HlslCamera.Infrastructure
{
    public class RelayCommand : ICommand
    {
        private Action<object> executeMethod;
        private Func<object, bool> canExecuteMethod;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod = null)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            if (null == canExecuteMethod)
                return true;

            return canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }
    }
}
