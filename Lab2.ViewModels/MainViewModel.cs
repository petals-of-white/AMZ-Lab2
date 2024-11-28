using System.Numerics;
using System.Windows.Media.Media3D;
using Lab2.Core;

namespace Lab2.ViewModels;

public class MainViewModel : SimpleNotifier
{
    private VariantData variantData = new(50, 1, Axis.X);

    public MainViewModel()
    {
        Point1VM.PropertyChanged += PointsVM_PropertyChanged;
        Point2VM.PropertyChanged += PointsVM_PropertyChanged;
    }

    public int CameraDistance
    {
        get => variantData.CameraDistance;
        set
        {
            variantData = new VariantData(value, variantData.FocusLength, variantData.SecondCameraPos);
            NotifyPropertyChanged(nameof(CameraDistance));
            NotifyPropertyChanged(nameof(ResultPoint));
        }
    }

    public string DistanceAxis
    {
        get => variantData.SecondCameraPos.ToString(); set
        {
            Axis newAxis = char.ToUpper(value.First()) switch
            {
                'X' => Axis.X,
                'Y' => Axis.Y,
                'Z' => Axis.Z,
                _ => variantData.SecondCameraPos
            };

            variantData = new VariantData(variantData.CameraDistance, variantData.FocusLength, newAxis);
            NotifyPropertyChanged(nameof(DistanceAxis));
            NotifyPropertyChanged(nameof(SecondCameraPosition));
        }
    }

    public Point3D FirstCameraPosition { get; } = new Point3D(0, 0, 0);

    public int FocusLength
    {
        get => variantData.FocusLength; set
        {
            variantData = new VariantData(variantData.CameraDistance, value, variantData.SecondCameraPos);
            NotifyPropertyChanged(nameof(FocusLength));
            NotifyPropertyChanged(nameof(ResultPoint));
        }
    }

    public ProjectionPointViewModel Point1VM { get; set; } = new(new Vector2(1, 1));

    public ProjectionPointViewModel Point2VM { get; set; } = new(new Vector2(-1, 1));

    public Point3D ResultPoint
    {
        get
        {
            var res = KeyPointLocation.calculate(variantData, Point1VM.Point, Point2VM.Point);
            return new Point3D(res.X, res.Y, res.Z);
        }
    }

    public Point3D SecondCameraPosition => variantData switch
    {
        { CameraDistance: var d, SecondCameraPos: var axis } when axis.IsX => new Point3D(d, 0, 0),
        { CameraDistance: var d, SecondCameraPos: var axis } when axis.IsY => new Point3D(0, d, 0),
        { CameraDistance: var d, SecondCameraPos: var axis } when axis.IsZ => new Point3D(0, 0, d),
        _ => throw new ArgumentException("Uknown axis")
    };

    private void PointsVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        NotifyPropertyChanged(nameof(ResultPoint));
    }
}