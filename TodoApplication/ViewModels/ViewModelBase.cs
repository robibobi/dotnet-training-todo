using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace TodoApplication.ViewModels
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static bool IsRunningInDesigner()
        {
            return (bool)(DesignerProperties.IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject)).DefaultValue);
        }


        public virtual Task OnAttachedAsync()
        {
            return Task.CompletedTask;
        }
    }
}
