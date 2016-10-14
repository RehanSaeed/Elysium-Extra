namespace Framework.UI.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class DatePickerAttached
    {
        #region Dependency Objects

        public static readonly DependencyProperty AvailableDatesProperty = DependencyProperty.RegisterAttached(
            "AvailableDates",
            typeof(IEnumerable),
            typeof(DatePickerAttached),
            new PropertyMetadata(null, OnAvailableDatesChanged));

        #endregion

        #region Public Static Methods

        public static IEnumerable GetAvailableDates(DatePicker datePicker)
        {
            return (IEnumerable)datePicker.GetValue(AvailableDatesProperty);
        }

        public static void SetAvailableDates(DatePicker datePicker, IEnumerable value)
        {
            datePicker.SetValue(AvailableDatesProperty, value);
        } 

        #endregion

        #region Private Static Methods
        
        private static void OnAvailableDatesChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)dependencyObject;
            IEnumerable dates = GetAvailableDates(datePicker);
            if (dates != null)
            {
                List<DateTime> availableDates = GetAvailableDates(datePicker)
                    .Cast<DateTime>()
                    .Select(x => x.Date)
                    .OrderBy(x => x)
                    .ToList();

                datePicker.DisplayDateStart = null;
                datePicker.DisplayDateEnd = null;
                datePicker.BlackoutDates.Clear();

                if (availableDates.Count > 0)
                {
                    datePicker.DisplayDateStart = availableDates.First();
                    datePicker.DisplayDateEnd = availableDates.Last();

                    if (availableDates.Count > 2)
                    {
                        for (int i = 0; (i + 1) < availableDates.Count; ++i)
                        {
                            DateTime start = availableDates[i];
                            DateTime end = availableDates[i + 1];

                            if ((end - start) > TimeSpan.FromDays(1))
                            {
                                datePicker.BlackoutDates.Add(new CalendarDateRange(start.AddDays(1), end.AddDays(-1)));
                            }
                        }
                    }
                }
            }
        } 

        #endregion
    }
}
