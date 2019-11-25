using System;
using System.Windows.Input;
using testing.DataModels;

namespace testing.Interfaces {

    /* Interafce for all tab Controls */
    public interface ITab { 
        String Name { get; set; }
        ICommand CloseCommand { get; }
        event EventHandler CloseRequested; // To be raised
    }

    // Base class for code re-use
    public abstract class Tabs : ITab {
        public string Name { get; set; }
        public ICommand CloseCommand { get; }
        public event EventHandler CloseRequested;

        public Tabs() {
            CloseCommand = new RelayCommand(o => CloseRequested?.Invoke(this, EventArgs.Empty));
        }
    }

    public class MainPageTabs : Tabs
    {
        public EPageList Content { get; set; } = EPageList.None;

        public MainPageTabs(String _Header, EPageList _Content) {
            Name = _Header;
            Content = _Content;
        }
    } 
}