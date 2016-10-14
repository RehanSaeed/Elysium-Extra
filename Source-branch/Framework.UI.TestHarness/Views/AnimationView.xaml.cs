namespace Framework.UI.TestHarness.Views
{
    using System;
    using System.Windows;
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for AnimationView.xaml
    /// </summary>
    public partial class AnimationView
    {
        public AnimationView()
        {
            InitializeComponent();
            this.DataContext = new ColoursViewModel();
        }

        /// <summary>
        /// The on animate in.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnAnimateIn(object sender, RoutedEventArgs e)
        {
            this.Behavior.AnimateIn();
        }

        /// <summary>
        /// The on animate out.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnAnimateOut(object sender, RoutedEventArgs e)
        {
            this.Behavior.AnimateOut();
        }

        /// <summary>
        /// The on show.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnShow(object sender, RoutedEventArgs e)
        {
            this.AnimatingItem.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The on collapse.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnCollapse(object sender, RoutedEventArgs e)
        {
            this.FadeBehavior.FadeOut();
            this.SlideBehavior.SlideOut();
        }

        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            this.AnimatingItem.Visibility = Visibility.Collapsed;
        }
    }
}
