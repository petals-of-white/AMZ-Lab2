using System.Numerics;
using System.Windows.Media.Media3D;

namespace Lab2.ViewModels;

public static class ConversionExtensions
{
    public static Point3D ToPoint3D(this Vector3 vector) => new(vector.X, vector.Y, vector.Z);
}