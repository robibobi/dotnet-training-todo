using Autofac;
using TodoApplication.Respositories;
using TodoApplication.Services;
using TodoApplication.ViewModels;

namespace TodoApplication.IoC
{
    internal class IoCContainer
    {
        public static IContainer Current { get; }

        static IoCContainer()
        {

            var builder = new ContainerBuilder();

            // Repossitories
            builder.RegisterType<TagRepository>()
                .As<ITagRepository>()
                .SingleInstance();

            builder.RegisterType<TodoItemRespository>()
                .As<ITodoItemRepository>();

            // Services
            builder.RegisterType<DialogService>()
                .As<IDialogService>();

            builder.RegisterType<AppConfigService>()
                .As<IAppConfigService>()
                .SingleInstance();

            // ViewModels
            builder.RegisterType<MainWindowViewModel>();

            Current = builder.Build();
        }
    }
}
