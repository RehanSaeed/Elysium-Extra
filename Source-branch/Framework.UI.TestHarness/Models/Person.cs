namespace Framework.UI.TestHarness.Models
{
    using Framework.ComponentModel;

    /// <summary>
    /// The person.
    /// </summary>
    public sealed class Person : NotifyPropertyChanges
    {
        private bool isSelected;

        /// <summary>
        /// Gets or sets the eye colour.
        /// </summary>
        public string EyeColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the nationality.
        /// </summary>
        public string Nationality
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the profession.
        /// </summary>
        /// <value>
        /// The profession.
        /// </value>
        public string Profession
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.SetProperty(ref this.isSelected, value); }
        }
    }
}
