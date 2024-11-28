using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Lab2.Views.Converters;

public class TwoPointsToLineConverter : IMultiValueConverter
{
    public object Convert(object [] values, Type targetType, object parameter, CultureInfo culture)
    {
        Point3DCollection points = [(Point3D) values [0], (Point3D) values [1]];

        return points;
    }

    public object [] ConvertBack(object value, Type [] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}