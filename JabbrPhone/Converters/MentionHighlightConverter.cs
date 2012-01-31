using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace JabbrPhone.Converters
{
    public class MentionHighlightConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var highlight = (bool)value;
            if (highlight)
            {
                return (SolidColorBrush)System.Windows.Application.Current.Resources["PhoneContrastForegroundBrush"];
            }
            else
            {
                return (SolidColorBrush)System.Windows.Application.Current.Resources["PhoneBackgroundBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
