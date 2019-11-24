using System;
using System.Windows.Input;

namespace testing.Interfaces {

    public interface ITab { 
        String Name { get; set; }
        ICommand CloseCommand { get; }
        event EventHandler CloseRequested;
    }

    public abstract class Tab : ITab { 
        public string Name { get; set; }
        public ICommand CloseCommand { get; }
        public event EventHandler CloseRequested;

        public Tab() {
            CloseCommand = new RelayCommand(o => CloseRequested?.Invoke(this, EventArgs.Empty));
        }
    }
}