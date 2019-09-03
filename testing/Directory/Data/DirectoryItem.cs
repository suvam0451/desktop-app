namespace testing
{
    // Information about a directory item (file/ folder)...
    public class DirectoryItem
    {
        /// The type of this item
        public DirectoryItemType Type { get; set; }
        /// The absolute path to this item
        public string FullPath { get; set; }
        // Name of this directory item...
        public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath); } }
    }
}