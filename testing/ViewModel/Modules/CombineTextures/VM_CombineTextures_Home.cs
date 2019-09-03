using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testing.DataModels;

namespace testing
{
    public class VM_TextureCombine_Home : BaseViewModel
    {
        #region Private Member
        /// The window this view model controls
        private Window mWindow;
        /// The window resizer helper that keeps the window size correct in various states
        private WindowResizer mWindowResizer;
        /// The margin around the window to allow for a drop shadow
        private Thickness mOuterMarginSize = new Thickness(5);
        /// The radius of the edges of the window
        private int mWindowRadius = 4;
        /// The last known dock position
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;
        #endregion

        #region Public Properties
        /// The smallest width the window can go to
        public double WindowMinimumWidth { get; set; } = 800;

        /// The smallest height the window can go to
        public double WindowMinimumHeight { get; set; } = 500;

        /// True if the window is currently being moved/dragged
        public bool BeingMoved { get; set; }

        // Whether sub-modules are being rendered...
        bool[] mine = { false, false };

        /// True if the window should be borderless because it is docked or maximized
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

        /// The size of the resize border around the window
        public int ResizeBorder
        {
            get { return (mWindow.WindowState == WindowState.Maximized) ? 0 : 4; }
        }

        // This will be our current sidebar (Value converted)...
        public Application_Sidebar Sidebar_Home_Content { get; set; } = Application_Sidebar.HomePage;
        public Application_Workload Workspace_Home_Content { get; set; } = Application_Workload.Default;

        /// The size of the resize border around the window, taking into account the outer margin
        public Thickness ResizeBorderThickness => new Thickness(OuterMarginSize.Left + ResizeBorder,
                                                        OuterMarginSize.Top + ResizeBorder,
                                                        OuterMarginSize.Right + ResizeBorder,
                                                        OuterMarginSize.Bottom + ResizeBorder);

        /// The padding of the inner content of the main window
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// The margin around the window to allow for a drop shadow
        public Thickness OuterMarginSize
        {
            // If it is maximized or docked, no border
            //get => mWindow.WindowState == WindowState.Maximized ? mWindowResizer.CurrentMonitorMargin : (Borderless ? new Thickness(0) : mOuterMarginSize);
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? new Thickness(0) : mOuterMarginSize;
            }
            set => mOuterMarginSize = value;
        }

        /// The radius of the edges of the window
        public int WindowRadius
        {
            // If it is maximized or docked, no border
            //get => Borderless ? 0 : mWindowRadius;

            // If app is maximized, return zero radius
            get => (mWindow.WindowState == WindowState.Maximized) ? 0 : mWindowRadius;
            set => mWindowRadius = value;
        }

        /// The rectangle border around the window when docked
        public int FlatBorderThickness => Borderless && mWindow.WindowState != WindowState.Maximized ? 1 : 0;

        /// The radius of the edges of the window
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// The height of the title bar / caption of the window
        public int TitleHeight { get; set; } = 28;

        /// The height of the title bar / caption of the window
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        /// True if we should have a dimmed overlay on the window
        /// such as when a popup is visible or the window is not focused
        public bool DimmableOverlayVisible { get; set; }

        // Current sidebar
        public Application_Sidebar CurrentPage { get; set; } = Application_Sidebar.Default;
        #endregion

        #region Commands
        /// The command to minimize the window
        public ICommand MinimizeCommand { get; set; }

        /// The command to maximize the window
        public ICommand MaximizeCommand { get; set; }

        /// The command to close the window
        public ICommand CloseCommand { get; set; }

        /// The command to show the system menu of the window
        public ICommand MenuCommand { get; set; }
        #endregion

        #region Constructor

        /// Default constructor
        public VM_TextureCombine_Home(Window window)
        {
            mWindow = window;

            mWindow.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                //OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            // Create commands
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue...
            var resizer = new WindowResizer(mWindow);
            /*
            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            /// Fix window resize issue
            mWindowResizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            mWindowResizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            // On window being moved/dragged
            mWindowResizer.WindowStartedMove += () =>
            {
                // Update being moved flag
                BeingMoved = true;
            };

            // Fix dropping an undocked window at top which should be positioned at the
            // very top of screen
            mWindowResizer.WindowFinishedMove += () =>
            {
                // Update being moved flag
                BeingMoved = false;

                // Check for moved to top of window and not at an edge
                if (mDockPosition == WindowDockPosition.Undocked && mWindow.Top == mWindowResizer.CurrentScreenSize.Top)
                    // If so, move it to the true top (the border size)
                    mWindow.Top = -OuterMarginSize.Top;
            };*/
        }

        #endregion

        #region Private Helpers

        /// Gets the current mouse position on the screen

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
        private Point GetMousePosition()
        {
            //Win32Point w32Mouse = new Win32Point();
            //GetCursorPos(ref w32Mouse);

            var w32Mouse = Mouse.GetPosition(mWindow);
            // Window position added
            return new Point(w32Mouse.X + mWindow.Left, w32Mouse.Y + mWindow.Top);
        }

        /*private Point GetMousePosition()
            {
                return mWindowResizer.GetCursorPosition();
            }*/

        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values


        /*private void WindowResized()
            {
                // Fire off events for all properties that are affected by a resize
                OnPropertyChanged(nameof(Borderless));
                OnPropertyChanged(nameof(FlatBorderThickness));
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            }*/

        #endregion
    }
}
