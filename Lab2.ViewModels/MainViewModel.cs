using System.Numerics;
using System.Windows.Media.Media3D;
using Lab2.CoreNe;

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
            variantData = new VariantData(value, variantData.FocusLength, variantData.SecondCameraAxis);
            NotifyPropertyChanged(nameof(CameraDistance));
            NotifyPropertyChanged(nameof(ResultPoint));
            NotifyPropertyChanged(nameof(FocusPlanePos));
            NotifyPropertyChanged(nameof(Point1AbsPos));
            NotifyPropertyChanged(nameof(Point2AbsPos));
        }
    }

    public string DistanceAxis
    {
        get => variantData.SecondCameraAxis.ToString(); set
        {
            Axis newAxis = char.ToUpper(value.First()) switch
            {
                'X' => Axis.X,
                'Y' => Axis.Y,
                'Z' => Axis.Z,
                _ => variantData.SecondCameraAxis
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

    public Point3D FirstCameraPosition => variantData.FirstCamera.ToPoint3D();

    public int FocusLength
    {
        get => variantData.FocusLength; set
        {
            variantData = new VariantData(variantData.CameraDistance, value, variantData.SecondCameraAxis);
            NotifyPropertyChanged(nameof(FocusLength));
            NotifyPropertyChanged(nameof(ResultPoint));
            NotifyPropertyChanged(nameof(FocusPlanePos));
            NotifyPropertyChanged(nameof(Point1AbsPos));
            NotifyPropertyChanged(nameof(Point2AbsPos));
        }
    }

    public Vector3D FocusPlaneNormal => variantData.SecondCameraAxis switch
    {
        Axis.X or Axis.Y => new(0, 0, 1),
        Axis.Z => new(1, 0, 0),
        _ => throw new ArgumentException("Unexpected axis")
    };

    public Point3D FocusPlanePos => (variantData.SecondCameraAxis switch
    {
        Axis.X or Axis.Y => (variantData.FirstCamera + variantData.SecondCamera) / 2 + new Vector3(0, 0, variantData.FocusLength * 100),
        Axis.Z => (variantData.FirstCamera + variantData.SecondCamera) / 2 + new Vector3(variantData.FocusLength * 100, 0, 0),
        _ => throw new ArgumentException("Unexpected axis")
    }).ToPoint3D();

    public Point3D Point1AbsPos => variantData.FirstPointAbsolutePos(Point1VM.Point).ToPoint3D();
    public ProjectionPointViewModel Point1VM { get; set; } = new(new Vector2(22, 22));
    public Point3D Point2AbsPos => variantData.SecondPointAbsolutePos(Point2VM.Point).ToPoint3D();
    public ProjectionPointViewModel Point2VM { get; set; } = new(new Vector2(-22, 22));

    public Point3D ResultPoint
    {
        get
        {
            var res = variantData.CalculateKeyPoint(Point1VM.Point, Point2VM.Point);
            return res.ToPoint3D();
        }
    }

    public Point3D SecondCameraPosition => variantData.SecondCamera.ToPoint3D();

    private void PointsVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        NotifyPropertyChanged(nameof(ResultPoint));
        NotifyPropertyChanged(nameof(Point1AbsPos));
        NotifyPropertyChanged(nameof(Point2AbsPos));
    }
}