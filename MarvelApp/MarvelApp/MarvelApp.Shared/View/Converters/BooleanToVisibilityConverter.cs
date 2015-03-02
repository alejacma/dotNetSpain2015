using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MarvelApp.View.Converters
{
    /// <summary>
    /// Convierte True a Visible y False a Collapsed.
    /// Si se le pasa como parámetro "Invert", hace lo contrario.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (((bool)value) ^ ((parameter != null) && ((string)parameter).Equals("Invert"))) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
