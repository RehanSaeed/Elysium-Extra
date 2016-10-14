namespace Framework.UI.TestHarness.Windows
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Elysium;

    /// <summary>
    /// Interaction logic for Elysium View XAML
    /// </summary>
    public partial class ElysiumView
    {
        private static readonly string Windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        private static readonly string SegoeUI = Windows + @"\Fonts\SegoeUI.ttf";
        private static readonly string Verdana = Windows + @"\Fonts\Verdana.ttf";

        /// <summary>
        /// Initialises a new instance of the <see cref="ElysiumView"/> class.
        /// </summary>
        public ElysiumView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The theme glyph initialized.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ThemeGlyphInitialized(object sender, EventArgs e)
        {
            ThemeGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        /// <summary>
        /// The accent glyph initialized.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AccentGlyphInitialized(object sender, EventArgs e)
        {
            AccentGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        /// <summary>
        /// The contrast glyph initialized.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void ContrastGlyphInitialized(object sender, EventArgs e)
        {
            ContrastGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        /// <summary>
        /// The light click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void LightClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(Theme.Light);
        }

        /// <summary>
        /// The dark click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void DarkClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(Theme.Dark);
        }

        /// <summary>
        /// The accent click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AccentClick(object sender, RoutedEventArgs e)
        {
            var item = e.Source as MenuItem;
            if (item != null)
            {
                var accentBrush = (SolidColorBrush)((Rectangle)item.Icon).Fill;
                Application.Current.Apply(accentBrush, null);
            }
        }

        /// <summary>
        /// The white click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void WhiteClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(null, Brushes.White);
        }

        /// <summary>
        /// The black click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void BlackClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(null, Brushes.Black);
        }

        /// <summary>
        /// The notification click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void NotificationClick(object sender, RoutedEventArgs e)
        {
            // NotificationManager.TryPushAsync("Message", "The quick brown fox jumps over the lazy dog");
        }

        /// <summary>
        /// The donate click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void DonateClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=KNYYZ7RM6LBCG");
        }

        /// <summary>
        /// The license click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void LicenseClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://elysium.codeplex.com/license");
        }

        /// <summary>
        /// The authors click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AuthorsClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://elysium.codeplex.com/team/view");
        }
    }
}
