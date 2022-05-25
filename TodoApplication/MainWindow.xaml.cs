using System;
using System.Windows;
using TodoApplication.Util;
using TodoApplication.ViewModels;

namespace TodoApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;  
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ViewModelBase vm)
            {
                AsyncVoidHelper.TryThrowOnDispatcher(vm.OnAttachedAsync);
            }
        }
    }
}
