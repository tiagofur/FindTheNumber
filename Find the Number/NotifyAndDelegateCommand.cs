using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Find_the_Number
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> _target;

        public DelegateCommand(Action<T> target)
        {
            this._target = target;
        }

        public bool CanExecute(object parameter)
        {
            return this._target != null;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this._target((T)parameter);
        }
    }

    public class ViewModelBase : INotifyPropertyChanged, IServiceProvider
    {
        #region IServiceProvider 

        public IServiceProvider ServiceProvider { get; set; }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                return null;

            if (serviceType.IsAssignableFrom(this.GetType()))
                return this;

            if (this.ServiceProvider != null)
                return this.ServiceProvider.GetService(serviceType);

            return null;
        }

        public T GetService<T>()
        {
            return ((T)this.GetService(typeof(T)));
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
