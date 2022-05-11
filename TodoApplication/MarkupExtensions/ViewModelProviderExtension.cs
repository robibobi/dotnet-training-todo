using Autofac;
using System;
using System.Windows.Markup;
using TodoApplication.ViewModels;

namespace TodoApplication.MarkupExtensions
{
    internal class ViewModelProviderExtension : MarkupExtension
    {
        public Type ViewModelType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if(ViewModelType == null)
            {
                return null;
            } else
            {
                if (ViewModelBase.IsRunningInDesigner())
                {
                    return null;
                } else
                {
                    return IoC.IoCContainer.Current?.Resolve(ViewModelType);
                }
            }
        }
    }
}
