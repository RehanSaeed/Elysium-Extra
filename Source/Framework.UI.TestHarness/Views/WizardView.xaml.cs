namespace Framework.UI.TestHarness.Views
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Framework.UI.Controls;
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for Wizard View XAML
    /// </summary>
    public partial class WizardView
    {
        private WizardItem wizardItem;

        /// <summary>
        /// Initialises a new instance of the <see cref="WizardView"/> class.
        /// </summary>
        public WizardView()
        {
            InitializeComponent();
            this.wizardItem = new WizardItem()
            {
                Content = "My View Model Bound Here",
                ContentTemplate = null,
                Background = (ImageBrush)this.Resources["AbstractImageBrush"],
                Description = "Managers Description",
                Icon = this.FindResource("User1Geometry"),
                Id = "NewManager",
                ParentId = "Managers",
                Title = "New Manager"
            };

            this.DataContext = new WizardViewModel();
        }

        /// <summary>
        /// The on skip managers wizard item click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnSkipManagersWizardItemClick(object sender, RoutedEventArgs e)
        {
            this.SimonSmithWizardItem.ParentId = "Home";
        }

        /// <summary>
        /// The on un-skip managers wizard item click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnUnskipManagersWizardItemClick(object sender, RoutedEventArgs e)
        {
            this.SimonSmithWizardItem.ParentId = "Managers";
        }

        /// <summary>
        /// The on add wizard item click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        private void OnAddWizardItemClick(object sender, RoutedEventArgs e)
        {
            if (!this.Wizard.Items.Contains(this.wizardItem))
            {
                this.Wizard.Items.Add(this.wizardItem);
            }
        }

        /// <summary>
        /// The on remove wizard item click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnRemoveWizardItemClick(object sender, RoutedEventArgs e)
        {
            if (this.Wizard.Items.Contains(this.wizardItem))
            {
                this.Wizard.Items.Remove(this.wizardItem);
            }
        }

        /// <summary>
        /// The on collapse wizard item click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnCollapseWizardItemClick(object sender, RoutedEventArgs e)
        {
            this.ManagersWizardItem.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// The on un collapse wizard item click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnUnCollapseWizardItemClick(object sender, RoutedEventArgs e)
        {
            this.ManagersWizardItem.Visibility = Visibility.Visible;
        }
    }
}
