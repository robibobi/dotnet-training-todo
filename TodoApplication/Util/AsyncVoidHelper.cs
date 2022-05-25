using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TodoApplication.Util
{
    static class AsyncVoidHelper
    {
        public static async void TryThrowOnDispatcher(Func<Task> func)
        {
            try
            {
                await func();
            } catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => throw ex);
            }
        }
    }
}
