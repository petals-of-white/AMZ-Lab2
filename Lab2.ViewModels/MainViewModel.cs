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
            NotifyPropertyChanged(nameof(FocusPlanePos));
            NotifyPropertyChanged(nameof(Point1AbsPos));
            NotifyPropertyChanged(nameof(Point2AbsPos));
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
            NotifyPropertyChanged(nameof(FocusPlanePos));
            NotifyPropertyChanged(nameof(FocusPlaneNormal));
            NotifyPropertyChanged(nameof(Point1AbsPos));
            NotifyPropertyChanged(nameof(Point2AbsPos));
            NotifyPropertyChanged(nameof(ResultPoint));
        }
    }

    public Point3D FirstCameraPosition => Coordinates.firstCameraPos(variantData).ToPoint3D();

    public int FocusLength
    {
        get => variantData.FocusLength; set
        {
            variantData = new VariantData(variantData.CameraDistance, value, variantData.SecondCameraPos);
            NotifyPropertyChanged(nameof(FocusLength));
            NotifyPropertyChanged(nameof(ResultPoint));
            NotifyPropertyChanged(nameof(FocusPlanePos));
            NotifyPropertyChanged(nameof(Point1AbsPos));
            NotifyPropertyChanged(nameof(Point2AbsPos));
        }
    }

    public Vector3D FocusPlaneNormal => variantData.SecondCameraPos switch
    {
        var axis when axis.IsX || axis.IsY => new(0, 0, 1),
        var axis when axis.IsZ => new(1, 0, 0),
        _ => throw new ArgumentException("Unexpected axis")
    };

    public Point3D FocusPlanePos => (variantData.SecondCameraPos switch
    {
        var axis when axis.IsX || axis.IsY =>
            (Coordinates.firstCameraPos(variantData) + Coordinates.secondCameraPos(variantData)) / 2 + new Vector3(0, 0, variantData.FocusLength * 100),

        var axis when axis.IsZ =>
            (Coordinates.firstCameraPos(variantData) + Coordinates.secondCameraPos(variantData)) / 2 + new Vector3(variantData.FocusLength * 100, 0, 0),

        _ => throw new ArgumentException("Unexpected axis")
    }).ToPoint3D();

    public Point3D Point1AbsPos => Coordinates.firstPointAbsPos(variantData, Point1VM.Point).ToPoint3D();
    public ProjectionPointViewModel Point1VM { get; set; } = new(new Vector2(40, 40));
    public Point3D Point2AbsPos => Coordinates.secondPointAbsPos(variantData, Point2VM.Point).ToPoint3D();
    public ProjectionPointViewModel Point2VM { get; set; } = new(new Vector2(-40, 40));

    public Point3D ResultPoint
    {
        get
        {
            var res = Coordinates.calculateKeyPoint(variantData, Point1VM.Point, Point2VM.Point);
            return res.ToPoint3D();
        }
    }

    public Point3D SecondCameraPosition => Coordinates.secondCameraPos(variantData).ToPoint3D();
    //public Point3D SecondCameraPosition =>  variantData switch
    //{
    //    { CameraDistance: var d, SecondCameraPos: var axis } when axis.IsX => new Point3D(d, 0, 0),
    //    { CameraDistance: var d, SecondCameraPos: var axis } when axis.IsY => new Point3D(0, d, 0),
    //    { CameraDistance: var d, SecondCameraPos: var axis } when axis.IsZ => new Point3D(0, 0, d),
    //    _ => throw new ArgumentException("Uknown axis")
    //};

    private void PointsVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        NotifyPropertyChanged(nameof(ResultPoint));
        NotifyPropertyChanged(nameof(Point1AbsPos));
        NotifyPropertyChanged(nameof(Point2AbsPos));
    }
}