namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Interop;

    public sealed class Screen
    {
        private readonly System.Windows.Forms.Screen screen;

        private Screen(System.Windows.Forms.Screen screen)
        {
            this.screen = screen;
        }

        public Rect DeviceBounds
        {
            get { return this.GetRect(this.screen.Bounds); }
        }

        public Rect WorkingArea
        {
            get { return this.GetRect(this.screen.WorkingArea); }
        }

        private Rect GetRect(System.Drawing.Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect()
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }

        public static Screen Primary
        {
            get { return new Screen(System.Windows.Forms.Screen.PrimaryScreen); }
        }

        public string DeviceName
        {
            get { return this.screen.DeviceName; }
        }

        public bool IsPrimary
        {
            get { return this.screen.Primary; }
        }

        public static IEnumerable<Screen> AllScreens()
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new Screen(screen);
            }
        }

        public static Screen GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(
                windowInteropHelper.Handle);
            Screen wpfScreen = new Screen(screen);
            return wpfScreen;
        }

        public static Screen GetScreenFrom(Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromPoint(drawingPoint);
            Screen wpfScreen = new Screen(screen);

            return wpfScreen;
        }
    }
}
