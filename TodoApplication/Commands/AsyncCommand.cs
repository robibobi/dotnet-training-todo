using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TodoApplication.Util;

namespace TodoApplication.Commands
{
    internal class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExceute;

        public event EventHandler CanExecuteChanged;


        public AsyncCommand(Func<Task> execute, Func<bool> canExceute)
        {
            _execute = execute;
            _canExceute = canExceute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExceute();
        }

        public void Execute(object parameter)
        {
            AsyncVoidHelper.TryThrowOnDispatcher(_execute);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }


    public class AsyncCommand<T> : ICommand where T : class
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExceute;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExceute)
        {
            _execute = execute;
            _canExceute = canExceute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExceute(parameter as T);
        }

        public void Execute(object parameter)
        {
            AsyncVoidHelper.TryThrowOnDispatcher(() => _execute(parameter as T));
        }
    }

}
