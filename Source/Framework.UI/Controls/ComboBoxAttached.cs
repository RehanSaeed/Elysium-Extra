namespace Framework.UI.Controls
{
    using System;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class ComboBoxAttached
    {
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached(
            "MaxLength", 
            typeof(int), 
            typeof(ComboBoxAttached), 
            new PropertyMetadata(-1, OnMaxLengthPropertyChanged));

        #region Public Static Methods

        public static int GetMaxLength(ComboBox comboBox)
        {
            return (int)comboBox.GetValue(MaxLengthProperty);
        }

        public static void SetMaxLength(ComboBox comboBox, int value)
        {
            comboBox.SetValue(MaxLengthProperty, value);
        } 

        #endregion

        #region Private Static Methods

        private static void OnMaxLengthPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = dependencyObject as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.IsVisible)
                {
                    SetMaxLengthInternal(comboBox, GetMaxLength(comboBox));
                }
                else
                {
                    var observable = Observable.FromEventPattern<DependencyPropertyChangedEventHandler, DependencyPropertyChangedEventArgs>(
                        x => comboBox.IsVisibleChanged += x,
                        x => comboBox.IsVisibleChanged -= x);
                    IDisposable subscription = null;
                    subscription = observable.Delay(TimeSpan.FromMilliseconds(100)).ObserveOnDispatcher().Subscribe(
                        x =>
                        {
                            SetMaxLengthInternal(comboBox, GetMaxLength(comboBox));
                            subscription.Dispose();
                        });
                }
            }
        }

        private static void SetMaxLengthInternal(ComboBox comboBox, int maxLength)
        {
            TextBox textBox = comboBox.FindVisualChild<TextBox>();
            if (textBox != null)
            {
                textBox.MaxLength = maxLength;
            }
        } 

        #endregion
    }
}
