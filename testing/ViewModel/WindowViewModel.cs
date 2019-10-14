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
        #endregion

        #region Public Properties
        /// The smallest width the window can go to
        public double WindowMinimumWidth { get; set; } = 1080;
        public String Yeet { get; set; } = "Yeeto";
        /// The smallest height the window can go to
        public double WindowMinimumHeight { get; set; } = 500;

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

        // public ICommand MinimizeCommand { get; set; }
        // public ICommand MaximizeCommand { get; set; }
        // public ICommand CloseCommand { get; set; }
        public ICommand MenuCommand { get; set; }
        public ICommand TrafficAnalysis { get; set; }

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

            // Create commands
            // MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            // MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            // CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));
            TrafficAnalysis = new RelayCommand(StartTrafficAnalysis);

            // Fix window resize issue...
            // var resizer = new WindowResizer(mWindow);

            TabBinding = new ObservableCollection<PageTabModel>();
            TabBinding.Add(new PageTabModel("Combine Textures", EPageList.CombineTexture));
        }

        private void StartTrafficAnalysis() {
            TabBinding.Add(new PageTabModel("Traffic Analysis", EPageList.TrafficAnalysis));
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
            //Win32Point w32Mouse = new Win32Point();
            //GetCursorPos(ref w32Mouse);

            var w32Mouse = Mouse.GetPosition(mWindow);
            // Window position added
            return new Point(w32Mouse.X + mWindow.Left, w32Mouse.Y + mWindow.Top);
        }
        #endregion
    }
}
