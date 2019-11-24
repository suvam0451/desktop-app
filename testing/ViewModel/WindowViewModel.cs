using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testing.DataModels;
using testing.Models;
using testing.Interfaces;

namespace testing
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private Member
        /// The window this view model controls
        private Window mWindow;
        /// The margin around the window to allow for a drop shadow
        private Thickness mOuterMarginSize = new Thickness(5);
        /// The radius of the edges of the window
        private int mWindowRadius = 4;
        /// The last known dock position
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        private bool[] InstanceActive { get; set; }
        #endregion

        #region Public Properties

        public ICollection<ITab> Tabs { get; }


        /// The smallest width the window can go to
        public double WindowMinimumWidth { get; set; } = 1080;
        public String Yeet { get; set; } = "Yeeto";
        /// The smallest height the window can go to
        public double WindowMinimumHeight { get; set; } = 500;
        public String AppStatus { get; set; } = "Idle";
        /// True if the window is currently being moved/dragged
        public bool BeingMoved { get; set; }

        /// True if the window should be borderless because it is docked or maximized
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

        public float OuterMarginSizeThickness { get; set; } = 0;
        /// The size of the resize border around the window
        public int ResizeBorder {
            get { return (mWindow.WindowState == WindowState.Maximized) ? 0 : 4; }
        }
        public ObservableCollection<PageTabModel> TabBinding { get; set; }
        public int SelectedTabIndex { get; set; }
        public Dictionary<EPageList, bool> TabTracking { get; set; }

        // public PageTabModel CurrentTabBinding { get; set; }
        // This will be our current sidebar (Value converted)...
        public EPageList Sidebar_Home_Content { get; set; } = EPageList.Sidebar;
        public Application_Workload Workspace_Home_Content { get; set; } = Application_Workload.Default;

        /// The size of the resize border around the window, taking into account the outer margin
        public Thickness ResizeBorderThickness => new Thickness(OuterMarginSize.Left + ResizeBorder,
                                                        OuterMarginSize.Top + ResizeBorder,
                                                        OuterMarginSize.Right + ResizeBorder,
                                                        OuterMarginSize.Bottom + ResizeBorder);

        /// The padding of the inner content of the main window
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// The margin around the window to allow for a drop shadow
        public Thickness OuterMarginSize {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? new Thickness(0) : mOuterMarginSize;
            }
            set => mOuterMarginSize = value;
        }

        /// The radius of the edges of the window
        public int WindowRadius {
            get => (mWindow.WindowState == WindowState.Maximized) ? 0 : mWindowRadius;
            set => mWindowRadius = value;
        }

        /// The rectangle border around the window when docked
        public int FlatBorderThickness => Borderless && mWindow.WindowState != WindowState.Maximized ? 1 : 0;

        /// The radius of the edges of the window
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// The height of the title bar / caption of the window
        public int TitleHeight { get; set; } = 32;

        /// The height of the title bar / caption of the window
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        /// True if we should have a dimmed overlay on the window
        /// such as when a popup is visible or the window is not focused
        public bool DimmableOverlayVisible { get; set; }

        // Current sidebar
        public Pagename CurrentPage { get; set; } = Pagename.Default;
        #endregion

        #region Commands

        public ICommand MenuCommand { get; set; }
        public ICommand AppendPage { get; set; }

        #endregion

        #region Constructor

        /// Default constructor
        public WindowViewModel(Window window)
        // public WindowViewModel()
        {
            mWindow = window;

            mWindow.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            MenuCommand = new RelayCommand(
                o => { SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()); }, 
                o => true );
            AppendPage = new RelayCommand(
                o => { AppendPage_Impl(o); },
                o => true );

            // Initialize Dictionary
            TabTracking = new Dictionary<EPageList, bool>();


            TabBinding = new ObservableCollection<PageTabModel>();
            TabBinding.Add(new PageTabModel("Combine Textures", EPageList.CombineTexture));
            TabTracking.Add(TabBinding[0].Content, true);

            // TabBinding[0].Content
            // 
            // CurrentTabBinding = TabBinding[0];
            SelectedTabIndex = 0;


            InstanceActive = new bool[6];

            Tabs = new ObservableCollection<ITab>();
        }

        private void AppendPage_Impl(object parameter) {
            int PageID = Convert.ToInt32(parameter);
            switch (PageID) {
                case 2: { 
                        if (TabTracking.ContainsKey(EPageList.CombineTexture))
                        {
                            AppStatus = "Workspace for Texture Assembler already exists.";
                        }
                        else {
                            TabBinding.Add(new PageTabModel("Combine textures", EPageList.CombineTexture));
                            AppStatus = "Created workspace for Combining Textures";
                        }
                    break;
                }
                case 3: { 
                    TabBinding.Add(new PageTabModel("OpenCV analysis", EPageList.PlayVideo));
                    AppStatus = "Created workspace for OpenCV CPU demo";
                    break;
                }
                case 4: { 
                    TabBinding.Add(new PageTabModel("Welcome", EPageList.HomePage));
                    AppStatus = "Created workspace for OpenCV CPU demo";
                    break;
                }
                case 6: { 
                    TabBinding.Add(new PageTabModel("Traffic Analysis", EPageList.TrafficAnalysis));
                    AppStatus = "Created workspace for Traffic Analysis";
                    break; 
                }
                case 7: {
                    TabBinding.Add(new PageTabModel("Traffic Analysis", EPageList.Settings));
                    AppStatus = "Created workspace for Settings Page";
                    break;
                }
                default: break;
            }
        }

        #endregion

        #region Private Helpers

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        // Gets the mouse position (using user32.dll)
        private Point GetMousePosition() {
            var w32Mouse = Mouse.GetPosition(mWindow);
            // Window position added
            return new Point(w32Mouse.X + mWindow.Left, w32Mouse.Y + mWindow.Top);
        }
        #endregion
    }
}