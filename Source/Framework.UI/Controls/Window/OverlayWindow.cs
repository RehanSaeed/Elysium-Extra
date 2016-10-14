namespace Framework.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    public class OverlayWindow : WindowBase
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsOverlayVisibleProperty = DependencyProperty.Register(
            "IsOverlayVisible",
            typeof(bool),
            typeof(OverlayWindow),
            new PropertyMetadata(true));

        public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register(
            "OverlayBackground",
            typeof(Brush),
            typeof(OverlayWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OverlayBorderBrushProperty = DependencyProperty.Register(
            "OverlayBorderBrush",
            typeof(Brush),
            typeof(OverlayWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OverlayBorderThicknessProperty = DependencyProperty.Register(
            "OverlayBorderThickness",
            typeof(Thickness),
            typeof(OverlayWindow),
            new PropertyMetadata(new Thickness()));

        public static readonly DependencyProperty OverlayMarginProperty = DependencyProperty.Register(
            "OverlayMargin",
            typeof(Thickness),
            typeof(OverlayWindow),
            new PropertyMetadata(new Thickness()));

        public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(
            "OverlayOpacity",
            typeof(double),
            typeof(OverlayWindow),
            new PropertyMetadata(0.8D));

        #endregion

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;

        #region Constructors

        static OverlayWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayWindow), new FrameworkPropertyMetadata(typeof(OverlayWindow)));
        }

        public OverlayWindow()
        {
            this.Style = (Style)this.FindResource(typeof(OverlayWindow));

            Elysium.Parameters.Window.SetMinimizeButtonToolTip(this, "Minimize");
            Elysium.Parameters.Window.SetMaximizeButtonToolTip(this, "Maximize");
            Elysium.Parameters.Window.SetRestoreButtonToolTip(this, "Restore");
            Elysium.Parameters.Window.SetCloseButtonToolTip(this, "Close");
        }

        #endregion

        #region Public Properties

        public bool IsOverlayVisible
        {
            get { return (bool)this.GetValue(IsOverlayVisibleProperty); }
            set { this.SetValue(IsOverlayVisibleProperty, value); }
        }

        public Brush OverlayBackground
        {
            get { return (Brush)this.GetValue(OverlayBackgroundProperty); }
            set { this.SetValue(OverlayBackgroundProperty, value); }
        }

        public Brush OverlayBorderBrush
        {
            get { return (Brush)this.GetValue(OverlayBorderBrushProperty); }
            set { this.SetValue(OverlayBorderBrushProperty, value); }
        }

        public Thickness OverlayBorderThickness
        {
            get { return (Thickness)this.GetValue(OverlayBorderThicknessProperty); }
            set { this.SetValue(OverlayBorderThicknessProperty, value); }
        }

        public Thickness OverlayMargin
        {
            get { return (Thickness)this.GetValue(OverlayMarginProperty); }
            set { this.SetValue(OverlayMarginProperty, value); }
        }

        public double OverlayOpacity
        {
            get { return (double)this.GetValue(OverlayOpacityProperty); }
            set { this.SetValue(OverlayOpacityProperty, value); }
        }

        public new System.Windows.Window Owner
        {
            get
            {
                return base.Owner;
            }

            set
            {
                base.Owner = value;
                this.UpdateOwnerBindings();
            }
        }

        #endregion

        #region Protected Methods

        private bool isClosing;

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!this.isClosing)
            {
                this.isClosing = true;
                e.Cancel = true;
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation()
                    {
                        Duration = TimeSpan.FromMilliseconds(500),
                        To = 0
                    };
                Storyboard.SetTarget(doubleAnimation, this);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));
                storyboard.Children.Add(doubleAnimation);
                EventHandler eventHandler = null;
                eventHandler = (sender, e2) =>
                     {
                         storyboard.Completed -= eventHandler;
                         this.Close();
                     };
                storyboard.Completed += eventHandler;
                storyboard.Begin();
            }
        }

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            base.OnSourceInitialized(e);
 
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource handle = HwndSource.FromHwnd(helper.Handle);
            handle.AddHook(WndProc);
        }

        #endregion

        #region Private Methods

        private void UpdateOwnerBindings()
        {
            if (this.Owner == null)
            {
                BindingOperations.ClearBinding(this, Window.LeftProperty);
                BindingOperations.ClearBinding(this, Window.TopProperty);
                BindingOperations.ClearBinding(this, Window.HeightProperty);
                BindingOperations.ClearBinding(this, Window.WidthProperty);
                this.WindowState = WindowState.Maximized;
                this.Margin = new Thickness();
                this.Topmost = true;
            }
            else
            {
                this.Topmost = false;
                this.SetBinding(
                    OverlayBorderThicknessProperty,
                    new Binding("BorderThickness")
                    {
                        Source = this.Owner
                    });
                this.SetBinding(
                    Window.MarginProperty,
                    new Binding("Margin")
                    {
                        Source = this.Owner
                    });
                this.SetBinding(
                    Window.LeftProperty,
                    new Binding("Left")
                    {
                        Source = this.Owner
                    });
                this.SetBinding(
                    Window.TopProperty,
                    new Binding("Top")
                    {
                        Source = this.Owner
                    });
                this.SetBinding(
                    Window.HeightProperty,
                    new Binding("Height")
                    {
                        Source = this.Owner
                    });
                this.SetBinding(
                    Window.WidthProperty,
                    new Binding("Width")
                    {
                        Source = this.Owner
                    });
                this.SetBinding(
                   Window.WindowStateProperty,
                   new Binding("WindowState")
                   {
                       Source = this.Owner
                   });
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion
    }
}
