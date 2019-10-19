using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace testing
{
    public class DirectoryItemViewModel : BaseViewModel
    {
        #region Public Properties
        /// The type of this item
        public DirectoryItemType Type { get; set; }
        /// Image used for the item
        public string ImageName => Type == DirectoryItemType.Drive ? "drive" : (Type == DirectoryItemType.File ? "file" : (IsExpanded ? "folder-open" : "folder-closed"));
        /// The absolute path to this item
        public string FullPath { get; set; }
        /// Name of this directory item...
        public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath); } }
        /// A list of all children contained inside this item
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }
        /// Indicates if this item can be expanded
        public bool CanExpand { get { return this.Type != DirectoryItemType.File; } }
        /// Indicates if the current item is expanded or not
        public bool IsExpanded {
            get {
                // Any children not null
                return this.Children?.Count(f => f != null) > 0;
            }
            set {
                // If UI tells us to expand...
                if (value == true)
                    Expand();
                else
                    this.ClearChildren();
            }
        }
        #endregion

        #region Public Commands
        /// The command to expand this item
        public ICommand ExpandCommand { get; set; }
        #endregion

        #region Constructor
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            // Pass expand as the command to be executed by ICommand...
            this.ExpandCommand = new RelayCommand( o => { Expand(); }, o => true);

            // Set path and type
            this.FullPath = fullPath;
            this.Type = type;

            // Setup the children as needed
            this.ClearChildren();
        }
        #endregion

        #region Helper Methods
        ///  Expands this directory and finds all children
        private void Expand()
            {
                // File can't expand
                if (this.Type == DirectoryItemType.File)
                    return;

                // Find all children
                var children = DirectoryStructure.GetDirectoryContents(this.FullPath);
                this.Children = new ObservableCollection<DirectoryItemViewModel>(
                                    children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type)));
            }

        private void ClearChildren()
        {
            // Clear items
            this.Children = new ObservableCollection<DirectoryItemViewModel>();
        }
        #endregion
    }
}
