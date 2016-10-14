namespace Framework.UI.TestHarness.ViewModels
{
    using Framework.ComponentModel;

    public sealed class MenuItemViewModel : NotifyPropertyChanges
    {
        private string property1;
        private string property2;
        private string property3;

        public string Property1
        {
            get { return this.property1; }
            set { this.SetProperty(ref this.property1, value); }
        }

        public string Property2
        {
            get { return this.property2; }
            set { this.SetProperty(ref this.property2, value); }
        }

        public string Property3
        {
            get { return this.property3; }
            set { this.SetProperty(ref this.property3, value); }
        }
    }
}
