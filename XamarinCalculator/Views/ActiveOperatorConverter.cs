using System;
using System.Globalization;
using Xamarin.Forms;
using XamarinCalculator.Models;

namespace XamarinCalculator.Views
{
    public class ActiveOperatorConverter: IValueConverter
    {
        // Source to target.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                Operator.Add => "+",
                Operator.Subtract => "-",
                Operator.Multiply => "×",
                Operator.Divide => "÷",
                _ => string.Empty
            };
        }

        // Target to source.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
