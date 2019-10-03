using PropertyChanged;
using System.ComponentModel;

namespace testing
{
    //[ImplementPropertyChanged]
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// The event that is fired when any child property changes its value
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// Call this to fire a "PropertyChanged" event
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}