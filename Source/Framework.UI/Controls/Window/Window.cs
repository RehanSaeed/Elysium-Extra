namespace Framework.UI.Controls
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Imaging;
    using System.Windows.Shell;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// The normal window.
    /// </summary>
    public class Window : WindowBase
    {
        #region Dependency Properties

        public static readonly DependencyProperty EffectFocusedProperty = DependencyProperty.Register(
            "EffectFocused", 
            typeof(Effect), 
            typeof(Window), 
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(
            "IsFullScreen",
            typeof(bool),
            typeof(Window),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsFullScreenShortcutsEnabledProperty = DependencyProperty.Register(
            "IsFullScreenShortcutsEnabled",
            typeof(bool),
            typeof(Window),
            new PropertyMetadata(true));

        public static readonly DependencyProperty TaskbarDescriptionProperty = DependencyProperty.Register(
            "TaskbarDescription",
            typeof(string),
            typeof(Window),
            new PropertyMetadata(null, OnTaskbarDescriptionPropertyChanged));

        public static readonly DependencyProperty TaskbarIsBusyProperty = DependencyProperty.Register(
            "TaskbarIsBusy",
            typeof(bool),
            typeof(Window),
            new PropertyMetadata(false, OnTaskbarIsBusyPropertyChanged));

        public static readonly DependencyProperty TaskbarOverlayProperty = DependencyProperty.Register(
            "TaskbarOverlay",
            typeof(object),
            typeof(Window),
            new PropertyMetadata(null, OnTaskbarOverlayPropertyChanged));

        public static readonly DependencyProperty TaskbarOverlayTemplateProperty = DependencyProperty.Register(
            "TaskbarOverlayTemplate",
            typeof(DataTemplate),
            typeof(Window),
            new PropertyMetadata(null, OnTaskbarOverlayPropertyChanged));

        public static readonly DependencyProperty TaskbarProgressStateProperty = DependencyProperty.Register(
            "TaskbarProgressState",
            typeof(TaskbarItemProgressState),
            typeof(Window),
            new PropertyMetadata(TaskbarItemProgressState.None, OnTaskbarProgressStatePropertyChanged));

        public static readonly DependencyProperty TaskbarProgressValueProperty = DependencyProperty.Register(
            "TaskbarProgressValue",
            typeof(double),
            typeof(Window),
            new PropertyMetadata(0D, OnTaskbarProgressValuePropertyChanged));

        #endregion

        private const int OVERLAY_ICON_WIDTH = 16;
        private const int OVERLAY_ICON_HEIGHT = 16;

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="Window"/> class.
        /// </summary>
        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));

            MarginProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMarginPropertyChanged)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
            this.KeyUp += this.OnKeyUp;

            this.Style = (Style)this.FindResource(typeof(Window));

            Elysium.Parameters.Window.SetMinimizeButtonToolTip(this, "Minimize");
            Elysium.Parameters.Window.SetMaximizeButtonToolTip(this, "Maximize");
            Elysium.Parameters.Window.SetRestoreButtonToolTip(this, "Restore");
            Elysium.Parameters.Window.SetCloseButtonToolTip(this, "Close");

            this.TaskbarItemInfo = new TaskbarItemInfo();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the effect applied to the window when focused.
        /// </summary>
        /// <value>
        /// The focused effect.
        /// </value>
        public Effect EffectFocused
        {
            get { return (Effect)this.GetValue(EffectFocusedProperty); }
            set { this.SetValue(EffectFocusedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is full screen.
        /// </summary>
        public bool IsFullScreen
        {
            get { return (bool)this.GetValue(IsFullScreenProperty); }
            set { this.SetValue(IsFullScreenProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is full screen shortcuts enabled.
        /// </summary>
        public bool IsFullScreenShortcutsEnabled
        {
            get { return (bool)this.GetValue(IsFullScreenShortcutsEnabledProperty); }
            set { this.SetValue(IsFullScreenShortcutsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the taskbar description.
        /// </summary>
        /// <value>
        /// The taskbar description.
        /// </value>
        public string TaskbarDescription
        {
            get { return (string)this.GetValue(TaskbarDescriptionProperty); }
            set { this.SetValue(TaskbarDescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the taskbar progress bar should be shown as indeterminate.
        /// </summary>
        /// <value>
        /// <c>true</c> if the taskbar is busy; otherwise, <c>false</c>.
        /// </value>
        public bool TaskbarIsBusy
        {
            get { return (bool)this.GetValue(TaskbarIsBusyProperty); }
            set { this.SetValue(TaskbarIsBusyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the taskbar icon overlayed on top of the default icon.
        /// </summary>
        /// <value>
        /// The taskbar overlay.
        /// </value>
        public object TaskbarOverlay
        {
            get { return (object)this.GetValue(TaskbarOverlayProperty); }
            set { this.SetValue(TaskbarOverlayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the taskbar overlay template.
        /// </summary>
        /// <value>
        /// The taskbar overlay template.
        /// </value>
        public DataTemplate TaskbarOverlayTemplate
        {
            get { return (DataTemplate)this.GetValue(TaskbarOverlayTemplateProperty); }
            set { this.SetValue(TaskbarOverlayTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the state of the taskbar progress.
        /// </summary>
        /// <value>
        /// The state of the taskbar progress.
        /// </value>
        public TaskbarItemProgressState TaskbarProgressState
        {
            get { return (TaskbarItemProgressState)this.GetValue(TaskbarProgressStateProperty); }
            set { this.SetValue(TaskbarProgressStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the taskbar progress value.
        /// </summary>
        /// <value>
        /// The taskbar progress value.
        /// </value>
        public double TaskbarProgressValue
        {
            get { return (double)this.GetValue(TaskbarProgressValueProperty); }
            set { this.SetValue(TaskbarProgressValueProperty, value); }
        }

        #endregion

        #region Protected Methods

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            this.UpdateResizeBorderThickness();
        }

        #endregion

        #region Private Static Methods

        private static void OnMarginPropertyChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)dependencyObject;
            window.UpdateResizeBorderThickness();
        }

        /// <summary>
        /// Called when the task bar description property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTaskbarDescriptionPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)dependencyObject;
            window.GetTaskbarItemInfoSafely().Description = window.TaskbarDescription;
        }

        /// <summary>
        /// Called when the taskbar is busy property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTaskbarIsBusyPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)dependencyObject;

            if (window.TaskbarIsBusy)
            {
                window.GetTaskbarItemInfoSafely().ProgressState = TaskbarItemProgressState.Indeterminate;
            }
            else
            {
                window.GetTaskbarItemInfoSafely().ProgressState = TaskbarItemProgressState.None;
            }
        }

        /// <summary>
        /// Called when the task bar overlay property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTaskbarOverlayPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)dependencyObject;

            if ((window.TaskbarOverlay == null) && (window.TaskbarOverlayTemplate == null))
            {
                window.GetTaskbarItemInfoSafely().Overlay = null;
            }
            else if ((window.TaskbarOverlay != null) && (window.TaskbarOverlay is ImageSource) && (window.TaskbarOverlayTemplate == null))
            {
                window.GetTaskbarItemInfoSafely().Overlay = (ImageSource)window.TaskbarOverlay;
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                OVERLAY_ICON_WIDTH, 
                OVERLAY_ICON_HEIGHT, 
                96, 
                96, 
                PixelFormats.Default);
            ContentControl contentControl = new ContentControl()
            {
                Content = window.TaskbarOverlay,
                ContentTemplate = window.TaskbarOverlayTemplate
            };
            contentControl.Arrange(new Rect(0, 0, OVERLAY_ICON_WIDTH, OVERLAY_ICON_HEIGHT));
            renderTargetBitmap.Render(contentControl);

            window.GetTaskbarItemInfoSafely().Overlay = renderTargetBitmap;
        }

        /// <summary>
        /// Called when the task bar progress state property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTaskbarProgressStatePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)dependencyObject;
            window.GetTaskbarItemInfoSafely().ProgressState = window.TaskbarProgressState;
        }

        /// <summary>
        /// Called when the task bar progress value property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTaskbarProgressValuePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)dependencyObject;
            window.GetTaskbarItemInfoSafely().ProgressValue = window.TaskbarProgressValue;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the taskbar item information safely.
        /// </summary>
        /// <returns></returns>
        private TaskbarItemInfo GetTaskbarItemInfoSafely()
        {
            if (this.TaskbarItemInfo == null)
            {
                this.TaskbarItemInfo = new TaskbarItemInfo();
            }

            if (this.TaskbarItemInfo.IsFrozen)
            {
                this.TaskbarItemInfo = (TaskbarItemInfo)this.TaskbarItemInfo.Clone();
            }

            return this.TaskbarItemInfo;
        }

        /// <summary>
        /// Called when the key up event is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (this.IsFullScreenShortcutsEnabled)
            {
                if (e.Key == Key.F12)
                {
                    this.IsFullScreen = true;
                }
                else if (e.Key == Key.Escape)
                {
                    this.IsFullScreen = false;
                }
            }
        }

        private void UpdateResizeBorderThickness()
        {
            if (this.chromeField != null)
            {
                WindowChrome chrome = (WindowChrome)this.chromeField.GetValue(this);
                if (this.WindowState == WindowState.Maximized)
                {
                    chrome = new WindowChrome
                    {
                        CaptionHeight = chrome.CaptionHeight,
                        CornerRadius = chrome.CornerRadius,
                        GlassFrameThickness = chrome.GlassFrameThickness,
                        NonClientFrameEdges = chrome.NonClientFrameEdges,
                        ResizeBorderThickness = new Thickness(0D),
                        UseAeroCaptionButtons = chrome.UseAeroCaptionButtons
                    };
                }
                else
                {
                    chrome = new WindowChrome
                    {
                        CaptionHeight = chrome.CaptionHeight,
                        CornerRadius = chrome.CornerRadius,
                        GlassFrameThickness = chrome.GlassFrameThickness,
                        NonClientFrameEdges = chrome.NonClientFrameEdges,
                        ResizeBorderThickness = new Thickness(
                            this.Margin.Left < 3D ? 3D : this.Margin.Left,
                            this.Margin.Top < 3D ? 3D : this.Margin.Top,
                            this.Margin.Right < 3D ? 3D : this.Margin.Right,
                            this.Margin.Bottom < 3D ? 3D : this.Margin.Bottom),
                        UseAeroCaptionButtons = chrome.UseAeroCaptionButtons
                    };
                }
                chrome.Freeze();
                WindowChrome.SetWindowChrome(this, chrome);
            }
        }

        #endregion

        #region Drop Shadow

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        /// <summary>
        /// The actual method that makes API calls to drop the shadow to the window
        /// </summary>
        /// <param name="window">Window to which the shadow will be applied</param>
        /// <returns>True if the method succeeded, false if not</returns>
        private static bool DropShadow(Window window)
        {
            try
            {
                WindowInteropHelper helper = new WindowInteropHelper(window);
                int val = 2;
                int ret1 = DwmSetWindowAttribute(helper.Handle, 2, ref val, 4);

                if (ret1 == 0)
                {
                    Margins m = new Margins { Bottom = 1, Left = 1, Right = 1, Top = 1 };
                    int ret2 = DwmExtendFrameIntoClientArea(helper.Handle, ref m);
                    return ret2 == 0;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                // Probably dwmapi.dll not found (incompatible OS)
                return false;
            }
        }

        #endregion

        #region HACK TO ALLOW RESIZING BIGGER THAN PRIMARY SCREEN RESOLUTION. REMOVE UPON UPGRADE OF ELYSIUM

        private Assembly assembly = Assembly.GetAssembly(typeof(Elysium.Theme));
        private BindingFlags bindingFlags =
            BindingFlags.CreateInstance |
            BindingFlags.Instance |
            BindingFlags.InvokeMethod |
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.GetField |
            BindingFlags.FlattenHierarchy;
        private FieldInfo chromeField;
        private IntPtr handle;
        private ConstructorInfo monitorConstructor;
        private PropertyInfo monitorBoundsProperty;
        private MethodInfo monitorInvalidateMethod;
        private PropertyInfo monitorWorkAreaProperty;
        private MethodInfo taskbarInvalidateMethod;
        private PropertyInfo taskbarPositionProperty;
        private PropertyInfo taskbarAutoHideProperty;
        private FieldInfo boundsLeftField;
        private FieldInfo boundsTopField;
        private FieldInfo workAreaTopField;
        private FieldInfo workAreaBottomField;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.SourceInitialized" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (this.HasDropShadow && !this.AllowsTransparency)
            {
                DropShadow(this);
            }

            this.handle = new WindowInteropHelper(this).Handle;
            this.assembly = Assembly.GetAssembly(typeof(Elysium.Theme));

            Type monitorType = this.assembly.GetType("Elysium.Native.Monitor");
            this.monitorConstructor = monitorType.GetConstructors(this.bindingFlags).First();
            this.monitorInvalidateMethod = monitorType.GetMethod("Invalidate", this.bindingFlags);
            this.monitorBoundsProperty = monitorType.GetProperty("Bounds", this.bindingFlags);
            this.monitorWorkAreaProperty = monitorType.GetProperty("WorkArea", this.bindingFlags);

            Type taskbarType = this.assembly.GetType("Elysium.Native.Taskbar");
            this.taskbarInvalidateMethod = taskbarType.GetMethod("Invalidate", this.bindingFlags);
            this.taskbarPositionProperty = taskbarType.GetProperty("Position", this.bindingFlags);
            this.taskbarAutoHideProperty = taskbarType.GetProperty("AutoHide", this.bindingFlags);

            this.chromeField = typeof(Elysium.Controls.Window).GetField("_chrome", this.bindingFlags);

            this.UpdateResizeBorderThickness();

            // Use WindowProc as the callback method
            // to process all native window messages.
            HwndSource.FromHwnd(handle).AddHook(MaximizedSizeFixWindowProc);
        }

        /// <summary>
        /// Window procedure callback.
        /// Hooked to a WPF maximized window works around a WPF bug:
        /// https://connect.microsoft.com/VisualStudio/feedback/details/363288/maximised-wpf-window-not-covering-full-screen?wa=wsignin1.0#tabs
        /// possibly also:
        /// https://connect.microsoft.com/VisualStudio/feedback/details/540394/maximized-window-does-not-cover-working-area-after-screen-setup-change?wa=wsignin1.0
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The window message.</param>
        /// <param name="wParam">The wParam (word parameter).</param>
        /// <param name="lParam">The lParam (long parameter).</param>
        /// <param name="handled">
        /// if set to <c>true</c> - the message is handled
        /// and should not be processed by other callbacks.
        /// </param>
        /// <returns></returns>
        private IntPtr MaximizedSizeFixWindowProc(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_GETMINMAXINFO:
                    // Handle the message and mark it as handled,
                    // so other callbacks do not touch it
                    //WindowChrome chrome = (WindowChrome)this.chromeField.GetValue(this);
                    //if (object.Equals(WindowChrome.GetWindowChrome(this), chrome))
                    //{
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    //}
                    break;
            }

            return (IntPtr)0;
        }

        /// <summary>
        /// Creates and populates the MINMAXINFO structure for a maximized window.
        /// Puts the structure into memory address given by lParam.
        /// Only used to process a WM_GETMINMAXINFO message.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="lParam">The lParam.</param>
        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var structure = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(NativeMethods.MINMAXINFO));

            // var monitor = new Monitor(_handle);
            // monitor.Invalidate();
            // Taskbar.Invalidate();
            object monitor = this.monitorConstructor.Invoke(new object[] { this.handle });
            this.monitorInvalidateMethod.Invoke(monitor, new object[] { });
            this.taskbarInvalidateMethod.Invoke(null, new object[] { });

            // var bounds = monitor.Bounds;
            // var workArea = monitor.WorkArea;
            object bounds = this.monitorBoundsProperty.GetValue(monitor);
            object workArea = this.monitorWorkAreaProperty.GetValue(monitor);

            if (this.boundsLeftField == null)
            {
                Type boundsType = bounds.GetType();
                this.boundsLeftField = boundsType.GetField("left", this.bindingFlags);
                this.boundsTopField = boundsType.GetField("top", this.bindingFlags);
                Type workAreaType = workArea.GetType();
                this.workAreaTopField = workAreaType.GetField("top", this.bindingFlags);
                this.workAreaBottomField = workAreaType.GetField("bottom", this.bindingFlags);
            }

            int boundsLeft = (int)this.boundsLeftField.GetValue(bounds);
            int boundsTop = (int)this.boundsTopField.GetValue(bounds);

            int workAreaLeft;
            int workAreaRight;

            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            NativeMethods.GetWindowPlacement(this.handle, out placement);
            //placement.showCmd == ShowWindowCommands
            if (placement.showCmd == 3)
            {
                workAreaLeft = (int)workArea.GetType().GetField("left", bindingFlags).GetValue(workArea);
                workAreaRight = (int)workArea.GetType().GetField("right", bindingFlags).GetValue(workArea);
            }
            else
            {
                // Allow the screen to be resized accross multiple monitors.
                // This does not work when the user double clicks the title bar as the WindowState shows as Normal.
                workAreaLeft = 0;
                workAreaRight = (int)SystemParameters.VirtualScreenWidth;
            }

            int workAreaTop = (int)this.workAreaTopField.GetValue(workArea);
            int workAreaBottom = (int)this.workAreaBottomField.GetValue(workArea);

            int taskbarPosition = (int)this.taskbarPositionProperty.GetValue(null);
            bool taskbarAutoHide = (bool)this.taskbarAutoHideProperty.GetValue(null);

            structure.MaxPosition.X = Math.Abs(boundsLeft) + taskbarPosition == 0 && taskbarAutoHide ? 1 : 0;
            structure.MaxPosition.Y = Math.Abs(boundsTop) + taskbarPosition == 1 && taskbarAutoHide ? 1 : 0;
            // WARNING: MAGIC NUMBER - Why Do I Need to take away 3!!!
            structure.MaxSize.X = structure.MaxTrackSize.X = Math.Abs(workAreaRight - workAreaLeft) - (taskbarPosition == 2 && taskbarAutoHide ? 1 : 0) - 3;
            structure.MaxSize.Y = structure.MaxTrackSize.Y = Math.Abs(workAreaBottom - workAreaTop) - (taskbarPosition == 3 && taskbarAutoHide ? 1 : 0);

            var source = PresentationSource.FromVisual(this);
            if (source != null && source.CompositionTarget != null)
            {
                if (IsNonNegative(MinWidth))
                {
                    structure.MinTrackSize.X = (int)Math.Ceiling(MinWidth * source.CompositionTarget.TransformFromDevice.M11);
                }
                if (IsNonNegative(MinHeight))
                {
                    structure.MinTrackSize.Y = (int)Math.Ceiling(MinHeight * source.CompositionTarget.TransformFromDevice.M22);
                }
                if (IsNonNegative(MaxWidth))
                {
                    structure.MaxSize.X = structure.MaxTrackSize.X = Math.Min(structure.MaxSize.X, (int)Math.Ceiling(MaxWidth * source.CompositionTarget.TransformFromDevice.M11));
                }
                if (IsNonNegative(MaxHeight))
                {
                    structure.MaxSize.Y = structure.MaxTrackSize.Y = Math.Min(structure.MaxSize.Y, (int)Math.Ceiling(MaxHeight * source.CompositionTarget.TransformFromDevice.M22));
                }
            }

            Marshal.StructureToPtr(structure, lParam, true);
        }

        private static bool IsNonNegative(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;
        }

        /// <summary>
        /// Native Windows API methods and interfaces.
        /// </summary>
        private static class NativeMethods
        {
            #region Constants
            // The WM_GETMINMAXINFO message is sent to a window when the size or
            // position of the window is about to change.
            // An application can use this message to override the window's
            // default maximized size and position, or its default minimum or
            // maximum tracking size.
            public const int WM_GETMINMAXINFO = 0x0024;

            public const int SW_SHOWNORMAL = 1;
            public const int SW_SHOWMINIMIZED = 2;

            public const int WM_MDIMAXIMIZE = 0x0225;

            public const int WM_ENTERSIZEMOVE = 0x231;

            public const int WM_EXITSIZEMOVE = 0x232;

            public const int WM_NCACTIVATE = 0x86;


            // Constants used with MonitorFromWindow()
            // Returns NULL.
            private const int MONITOR_DEFAULTTONULL = 0;

            // Returns a handle to the primary display monitor.
            private const int MONITOR_DEFAULTTOPRIMARY = 1;

            // Returns a handle to the display monitor that is nearest to the window.
            private const int MONITOR_DEFAULTTONEAREST = 2;
            #endregion

            #region Structs
            /// <summary>
            /// Native Windows API-compatible POINT struct
            /// </summary>
            [Serializable, StructLayout(LayoutKind.Sequential)]
            public struct POINT
            {
                public int X;
                public int Y;
            }
            /// <summary>
            /// The RECT structure defines the coordinates of the upper-left
            /// and lower-right corners of a rectangle.
            /// </summary>
            /// <see cref="http://msdn.microsoft.com/en-us/library/dd162897%28VS.85%29.aspx"/>
            /// <remarks>
            /// By convention, the right and bottom edges of the rectangle
            /// are normally considered exclusive.
            /// In other words, the pixel whose coordinates are ( right, bottom )
            /// lies immediately outside of the the rectangle.
            /// For example, when RECT is passed to the FillRect function, the rectangle
            /// is filled up to, but not including,
            /// the right column and bottom row of pixels. This structure is identical
            /// to the RECTL structure.
            /// </remarks>
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                /// <summary>
                /// The x-coordinate of the upper-left corner of the rectangle.
                /// </summary>
                public int Left;

                /// <summary>
                /// The y-coordinate of the upper-left corner of the rectangle.
                /// </summary>
                public int Top;

                /// <summary>
                /// The x-coordinate of the lower-right corner of the rectangle.
                /// </summary>
                public int Right;

                /// <summary>
                /// The y-coordinate of the lower-right corner of the rectangle.
                /// </summary>
                public int Bottom;
            }

            /// <summary>
            /// The MINMAXINFO structure contains information about a window's
            /// maximized size and position and its minimum and maximum tracking size.
            /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms632605%28VS.85%29.aspx"/>
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct MINMAXINFO
            {
                /// <summary>
                /// Reserved; do not use.
                /// </summary>
                public POINT Reserved;

                /// <summary>
                /// Specifies the maximized width (POINT.x)
                /// and the maximized height (POINT.y) of the window.
                /// For top-level windows, this value
                /// is based on the width of the primary monitor.
                /// </summary>
                public POINT MaxSize;

                /// <summary>
                /// Specifies the position of the left side
                /// of the maximized window (POINT.x)
                /// and the position of the top
                /// of the maximized window (POINT.y).
                /// For top-level windows, this value is based
                /// on the position of the primary monitor.
                /// </summary>
                public POINT MaxPosition;

                /// <summary>
                /// Specifies the minimum tracking width (POINT.x)
                /// and the minimum tracking height (POINT.y) of the window.
                /// This value can be obtained programmatically
                /// from the system metrics SM_CXMINTRACK and SM_CYMINTRACK.
                /// </summary>
                public POINT MinTrackSize;

                /// <summary>
                /// Specifies the maximum tracking width (POINT.x)
                /// and the maximum tracking height (POINT.y) of the window.
                /// This value is based on the size of the virtual screen
                /// and can be obtained programmatically
                /// from the system metrics SM_CXMAXTRACK and SM_CYMAXTRACK.
                /// </summary>
                public POINT MaxTrackSize;
            }

            /// <summary>
            /// The WINDOWINFO structure contains window information.
            /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms632610%28VS.85%29.aspx"/>
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct WINDOWINFO
            {
                /// <summary>
                /// The size of the structure, in bytes.
                /// The caller must set this to sizeof(WINDOWINFO).
                /// </summary>
                public uint Size;

                /// <summary>
                /// Pointer to a RECT structure
                /// that specifies the coordinates of the window.
                /// </summary>
                public RECT Window;

                /// <summary>
                /// Pointer to a RECT structure
                /// that specifies the coordinates of the client area.
                /// </summary>
                public RECT Client;

                /// <summary>
                /// The window styles. For a table of window styles,
                /// <see cref="http://msdn.microsoft.com/en-us/library/ms632680%28VS.85%29.aspx">
                /// CreateWindowEx
                /// </see>.
                /// </summary>
                public uint Style;

                /// <summary>
                /// The extended window styles. For a table of extended window styles,
                /// see CreateWindowEx.
                /// </summary>
                public uint ExStyle;

                /// <summary>
                /// The window status. If this member is WS_ACTIVECAPTION,
                /// the window is active. Otherwise, this member is zero.
                /// </summary>
                public uint WindowStatus;

                /// <summary>
                /// The width of the window border, in pixels.
                /// </summary>
                public uint WindowBordersWidth;

                /// <summary>
                /// The height of the window border, in pixels.
                /// </summary>
                public uint WindowBordersHeight;

                /// <summary>
                /// The window class atom (see
                /// <see cref="http://msdn.microsoft.com/en-us/library/ms633586%28VS.85%29.aspx">
                /// RegisterClass
                /// </see>).
                /// </summary>
                public ushort WindowType;

                /// <summary>
                /// The Windows version of the application that created the window.
                /// </summary>
                public ushort CreatorVersion;
            }

            /// <summary>
            /// The MONITORINFO structure contains information about a display monitor.
            /// The GetMonitorInfo function stores information in a MONITORINFO structure.
            /// <seealso cref="http://msdn.microsoft.com/en-us/library/dd145065%28VS.85%29.aspx"/>
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct MONITORINFO
            {
                /// <summary>
                /// The size, in bytes, of the structure. Set this member
                /// to sizeof(MONITORINFO) (40) before calling the GetMonitorInfo function.
                /// Doing so lets the function determine
                /// the type of structure you are passing to it.
                /// </summary>
                public int Size;

                /// <summary>
                /// A RECT structure that specifies the display monitor rectangle,
                /// expressed in virtual-screen coordinates.
                /// Note that if the monitor is not the primary display monitor,
                /// some of the rectangle's coordinates may be negative values.
                /// </summary>
                public RECT Monitor;

                /// <summary>
                /// A RECT structure that specifies the work area rectangle
                /// of the display monitor that can be used by applications,
                /// expressed in virtual-screen coordinates.
                /// Windows uses this rectangle to maximize an application on the monitor.
                /// The rest of the area in rcMonitor contains system windows
                /// such as the task bar and side bars.
                /// Note that if the monitor is not the primary display monitor,
                /// some of the rectangle's coordinates may be negative values.
                /// </summary>
                public RECT WorkArea;

                /// <summary>
                /// The attributes of the display monitor.
                ///
                /// This member can be the following value:
                /// 1 : MONITORINFOF_PRIMARY
                /// </summary>
                public uint Flags;
            }
            #endregion

            #region Imported methods

            [DllImport("user32.dll")]
            public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

            [DllImport("user32.dll")]
            public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

            /// <summary>
            /// The GetWindowInfo function retrieves information about the specified window.
            /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms633516%28VS.85%29.aspx"/>
            /// </summary>
            /// <param name="hwnd">The window handle.</param>
            /// <param name="pwi">The reference to WINDOWINFO structure.</param>
            /// <returns>true on success</returns>
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

            /// <summary>
            /// The MonitorFromWindow function retrieves a handle to the display monitor
            /// that has the largest area of intersection with the bounding rectangle
            /// of a specified window.
            /// <seealso cref="http://msdn.microsoft.com/en-us/library/dd145064%28VS.85%29.aspx"/>
            /// </summary>
            /// <param name="hwnd">The window handle.</param>
            /// <param name="dwFlags">Determines the function's return value
            /// if the window does not intersect any display monitor.</param>
            /// <returns>
            /// Monitor HMONITOR handle on success or based on dwFlags for failure
            /// </returns>
            [DllImport("user32.dll")]
            private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

            /// <summary>
            /// The GetMonitorInfo function retrieves information about a display monitor
            /// <seealso cref="http://msdn.microsoft.com/en-us/library/dd144901%28VS.85%29.aspx"/>
            /// </summary>
            /// <param name="hMonitor">A handle to the display monitor of interest.</param>
            /// <param name="lpmi">
            /// A pointer to a MONITORINFO structure that receives information
            /// about the specified display monitor.
            /// </param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

            #endregion
        }

        #endregion

        #region Window Placement

        // RECT structure required by WINDOWPLACEMENT structure
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        // POINT structure required by WINDOWPLACEMENT structure
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        // WINDOWPLACEMENT stores the position, size, and state of a window
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT minPosition;
            public POINT maxPosition;
            public RECT normalPosition;
        }

        public static class WindowPlacementHelper
        {
            private static Encoding encoding = new UTF8Encoding();
            private static XmlSerializer serializer = new XmlSerializer(typeof(WINDOWPLACEMENT));

            public static void SetPlacement(IntPtr windowHandle, string placementXml)
            {
                if (string.IsNullOrEmpty(placementXml))
                {
                    return;
                }

                WINDOWPLACEMENT placement;
                byte[] xmlBytes = encoding.GetBytes(placementXml);

                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(xmlBytes))
                    {
                        placement = (WINDOWPLACEMENT)serializer.Deserialize(memoryStream);
                    }

                    placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                    placement.flags = 0;
                    placement.showCmd = (placement.showCmd == NativeMethods.SW_SHOWMINIMIZED ? NativeMethods.SW_SHOWNORMAL : placement.showCmd);
                    NativeMethods.SetWindowPlacement(windowHandle, ref placement);
                }
                catch (InvalidOperationException)
                {
                    // Parsing placement XML failed. Fail silently.
                }
            }

            public static string GetPlacement(IntPtr windowHandle)
            {
                WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                NativeMethods.GetWindowPlacement(windowHandle, out placement);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                    {
                        serializer.Serialize(xmlTextWriter, placement);
                        byte[] xmlBytes = memoryStream.ToArray();
                        return encoding.GetString(xmlBytes);
                    }
                }
            }
        }

        #endregion
    }
}
