using Autofac;
using System.Windows;
using TodoApplication.Dialogs;
using TodoApplication.IoC;
using TodoApplication.Respositories;
using TodoApplication.Services;
using TodoApplication.ViewModels;

namespace TodoApplication
{
    public partial class MainWindow : Window
    {
       public MainWindow()
       {
            Title = "Title from C#";
            InitializeComponent();
            DataContext = IoCContainer.Current.Resolve<MainWindowViewModel>();

            //var tagRepository = new TagRepository();

            //DataContext = new MainWindowViewModel(
            //    new TodoItemRespository(),
            //    tagRepository,
            //    new DialogService(tagRepository),
            //    null);
        }
    }
}
