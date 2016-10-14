namespace Framework.UI
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using Elysium;

    public class ElysiumApplication : Application, INotifyPropertyChanged
    {
        #region Fields

        private Color accentColor;
        private Color contrastColor;
        private Color semitransparentContrastColor;
        private Theme theme = Theme.Light;
        private bool isLoaded;

        #endregion

        #region Events

        private event PropertyChangedEventHandler propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion

        #region Public Properties

        public Color AccentColor
        {
            get 
            { 
                return this.accentColor; 
            }

            set 
            {
                if (this.SetProperty(ref this.accentColor, value) && this.isLoaded)
                {
                    ThemeManager.UpdateAccentColor(this.Resources, value);
                }
            }
        }

        public Color ContrastColor
        {
            get
            {
                return this.contrastColor;
            }

            set
            {
                if (this.SetProperty(ref this.contrastColor, value) && this.isLoaded)
                {
                    ThemeManager.UpdateContrastColor(this.Resources, value);
                }
            }
        }

        public Color SemitransparentContrastColor
        {
            get
            {
                return this.semitransparentContrastColor;
            }

            set
            {
                if (this.SetProperty(ref this.semitransparentContrastColor, value) && this.isLoaded)
                {
                    ThemeManager.UpdateSemitransparentContrastColor(this.Resources, value);
                }
            }
        }

        public Theme Theme
        {
            get
            {
                return this.theme;
            }

            set
            {
                if (this.SetProperty(ref this.theme, value) && this.isLoaded)
                {
                    ThemeManager.UpdateTheme(this.Resources, value);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="currentValue">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c>.</returns>
        protected bool SetProperty<TProp>(
            ref TProp currentValue,
            TProp newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(currentValue, newValue))
            {
                currentValue = newValue;
                this.OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.propertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.AddExtraResourceDictionary(
                this.Resources, 
                this.Theme,
                this.AccentColor,
                this.ContrastColor,
                this.SemitransparentContrastColor);

            this.SetProperty(ref this.accentColor, (Color)this.Resources["AccentColor"]);
            this.SetProperty(ref this.contrastColor, (Color)this.Resources["ContrastColor"]);
            this.SetProperty(ref this.semitransparentContrastColor, (Color)this.Resources["SemitransparentContrastColor"]);

            ElysiumParameterOverrides.Apply();

            this.isLoaded = true;
        }

        #endregion
    }
}
