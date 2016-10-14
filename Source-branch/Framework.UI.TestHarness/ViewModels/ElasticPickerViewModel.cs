namespace Framework.UI.TestHarness.ViewModels
{
    using Framework.UI.TestHarness.Models;

    public sealed class ElasticPickerViewModel
    {
        private readonly PersonCollection people;

        public ElasticPickerViewModel()
        {
            this.people = new PersonCollection();
        }
        public PersonCollection People
        {
            get { return this.people; }
        }
    }
}
