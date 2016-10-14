namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Base class for controls that represents controls that can spin.
    /// </summary>
    public abstract class Spinner : Control
    {
        public static readonly DependencyProperty ValidSpinDirectionProperty = DependencyProperty.Register(
            "ValidSpinDirection", 
            typeof(ValidSpinDirections), 
            typeof(Spinner), 
            new PropertyMetadata(ValidSpinDirections.Increase | ValidSpinDirections.Decrease, OnValidSpinDirectionPropertyChanged));

        /// <summary>
        /// Initialises a new instance of the Spinner class.
        /// </summary>
        protected Spinner()
        {
        }

        /// <summary>
        /// Occurs when spinning is initiated by the end-user.
        /// </summary>
        public event EventHandler<SpinEventArgs> Spin;

        #region Public Properties

        /// <summary>
        /// Gets or sets the valid spin direction.
        /// </summary>
        public ValidSpinDirections ValidSpinDirection
        {
            get { return (ValidSpinDirections)this.GetValue(ValidSpinDirectionProperty); }
            set { this.SetValue(ValidSpinDirectionProperty, value); }
        }

        #endregion

        /// <summary>
        /// Raises the OnSpin event when spinning is initiated by the end-user.
        /// </summary>
        /// <param name="e">Spin event args.</param>
        protected virtual void OnSpin(SpinEventArgs e)
        {
            ValidSpinDirections valid = e.Direction == SpinDirection.Increase ? ValidSpinDirections.Increase : ValidSpinDirections.Decrease;

            // Only raise the event if spin is allowed.
            if ((this.ValidSpinDirection & valid) == valid)
            {
                EventHandler<SpinEventArgs> handler = this.Spin;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        /// <summary>
        /// Called when valid spin direction changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnValidSpinDirectionChanged(ValidSpinDirections oldValue, ValidSpinDirections newValue)
        {
        }

        /// <summary>
        /// ValidSpinDirectionProperty property changed handler.
        /// </summary>
        /// <param name="d">ButtonSpinner that changed its ValidSpinDirection.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnValidSpinDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Spinner source = (Spinner)d;
            ValidSpinDirections oldvalue = (ValidSpinDirections)e.OldValue;
            ValidSpinDirections newvalue = (ValidSpinDirections)e.NewValue;
            source.OnValidSpinDirectionChanged(oldvalue, newvalue);
        }
    }
}
