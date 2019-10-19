using System;
using System.Windows.Input;

namespace testing {
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public bool _CanExecute { get; set; } = true;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) { 
            return this.canExecute == null || this.canExecute(parameter); 
        }

        public void Execute(object parameter) {
            this.execute(parameter); 
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
