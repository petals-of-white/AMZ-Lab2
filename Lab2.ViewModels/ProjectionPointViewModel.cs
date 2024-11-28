using System.Drawing;
using System.Numerics;

namespace Lab2.ViewModels;

public class ProjectionPointViewModel : SimpleNotifier
{
    private Vector2 point;

    public ProjectionPointViewModel()
    {
    }

    public ProjectionPointViewModel(Vector2 vector)
    {
        point = vector;
    }

    public ProjectionPointViewModel(PointF point)
    {
        this.point = point.ToVector2();
    }

    public Vector2 Point => point;

    public float U
    {
        get => point.X; set
        {
            point.X = value;
            NotifyPropertyChanged(nameof(U));
        }
    }

    public float V
    {
        get => point.Y; set
        {
            point.Y = value;
            NotifyPropertyChanged(nameof(V));
        }
    }
}