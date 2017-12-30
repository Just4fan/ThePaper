using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace The_Paper.Bases.Commonds
{
    public class DelegateCommond : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<object> ExecuteAction { get; set; }
        public Func<object, bool> CanExecuteFunc { get; set; }

        bool ICommand.CanExecute(object parameter)
        {
            if (CanExecuteFunc != null)
                return CanExecuteFunc(parameter);
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAction?.Invoke(parameter);
            return;
        }
    }
}
