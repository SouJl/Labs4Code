using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Labs4Code.Converter
{
    class BoolToVisibleConvert
    {
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
           "IsVisible", typeof(bool?), typeof(BoolToVisibleConvert), new PropertyMetadata(default(bool?), IsVisibleChangedCallback));

        private static void IsVisibleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null)
                return;

            fe.Visibility = ((bool?)e.NewValue) == true
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static void SetIsVisible(DependencyObject element, bool? value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool? GetIsVisible(DependencyObject element)
        {
            return (bool?)element.GetValue(IsVisibleProperty);
        }
    }
}
