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
}
