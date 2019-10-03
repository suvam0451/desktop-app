using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace testing
{
    class RelayCommand : ICommand
    {
        private Action mAction;

        public bool _CanExecute { get; set; } = true;

        #region ctor
        public RelayCommand(Action action)
        {
            mAction = action;
        }
        #endregion

        // public bool CanExecute(object parameter) { return _CanExecute; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter) { mAction(); }

        /// Event that is fired when CanExecute value has changed
        public event EventHandler CanExecuteChanged = (sender, e) => { };
    }
}
