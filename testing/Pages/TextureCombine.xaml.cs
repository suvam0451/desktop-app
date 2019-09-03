using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.IO;

namespace testing
{
    /// <summary>
    /// Interaction logic for TextureCombine.xaml
    /// </summary>
    public partial class TextureCombine : Page
    {
        // Constructor...
        public TextureCombine()
        {
            InitializeComponent();
            texture_info_table.IsReadOnly = true;
            texture_info_table.Focusable = false;
            

            TChannel Roughness = new TChannel();
            Roughness.Name = "Roughness";
            Roughness.Occupancy = 1;
            Roughness.location = "None";
            Roughness.size = 1024;

            texture_info_table.Items.Add(Roughness);

            TChannel Metallic = new TChannel("Metallic", 1, "path", 1024);
            texture_info_table.Items.Add(Metallic);

            this.DataContext = new DirectoryStructureViewModel();
        }
        /*
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem() {
                    Header = drive,
                    Tag = drive
                };
                item.Items.Add(null);

                // Add event for expanded
                item.Expanded += Tree_Expanded;

                tree_view.Items.Add(item);
            }
        }
        private void Tree_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            // If item contains dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            // Clear dummy data
            item.Items.Clear();

            var fullpath = (string)item.Tag;
            var directories = new List<string>();
            try
            {
                var dirs = Directory.GetDirectories(fullpath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

            directories.ForEach(directoryPath =>
            {
                // Create directory item...
                var subItem = new TreeViewItem()
                {
                    // Header as folder name, tag as full path...
                    // Path.GetDirectoryName doesnt work
                    Header = GetFileFolderName(directoryPath),
                    Tag = directoryPath
                };

                // Add dummy item to added tree comp
                subItem.Items.Add(null);

                // Recursive hadling...
                subItem.Expanded += Tree_Expanded;

                item.Items.Add(subItem);
            });
        }*/

        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // make all slashes to backslashes
            var normalizedPath = path.Replace('/', '\\');

            var lastIndex = normalizedPath.LastIndexOf('\\');

            if (lastIndex <= 0)
                return path;
            return path.Substring(lastIndex + 1);        
        }

        public class TChannel {
            public TChannel() { }
            public TChannel(string _name, int _occupancy, string _location, int _size) {
                Name = _name;
                Occupancy = _occupancy;
                location = _location;
                size = _size;
            }

            public string Name { get; set; }
            public int Occupancy { get; set; }
            public string location { get; set; }
            public int size { get; set; }
        }
    }
}