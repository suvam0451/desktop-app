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
        #region Private Members
        /// The action to run
        private Action mAction;
        #endregion

        #region Public Events
        /// Event that is fired when CanExecute value has changed
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        #endregion

        #region Constructor
        /// Default constructor
        public RelayCommand(Action action)
        {
            mAction = action;
        }
        #endregion

        #region Command methods
        /// A relay command cqan always execute...
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// execute the command action
        public void Execute(object parameter)
        {
            mAction();
        }
        #endregion
    }
}
