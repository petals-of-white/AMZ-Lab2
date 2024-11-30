using System.Numerics;

namespace Lab2.CoreNe
{
    public enum Axis
    {
        X, Y, Z
    }

    public record class VariantData(int CameraDistance, int FocusLength, Axis SecondCameraAxis)
    {
        public Vector3 FirstCamera => new(0, 0, 0);
        public Vector3 SecondCamera => SecondCameraAxis switch
        {
            Axis.X => new(CameraDistance, 0, 0),
            Axis.Y => new(0, CameraDistance, 0),
            Axis.Z => new(0, 0, CameraDistance),
            _ => throw new ArgumentException("Unexpected axis", nameof(SecondCameraAxis))
        };

        public Vector3 FirstPointAbsolutePos(Vector2 point) => SecondCameraAxis switch
        {
            Axis.X or Axis.Y => new(point.X, point.Y, FocusLength * 100),
            Axis.Z => new(FocusLength * 100, point.Y, -point.X),
            _ => throw new ArgumentException("Unexpected axis", nameof(SecondCameraAxis))
        };

        public Vector3 SecondPointAbsolutePos(Vector2 point) => SecondCameraAxis switch
        {
            Axis.X => new(point.X + CameraDistance, point.Y, FocusLength * 100),
            Axis.Y => new(point.X, point.Y + CameraDistance, FocusLength * 100),
            Axis.Z => new(FocusLength * 100, point.Y, -point.X + CameraDistance),
            _ => throw new ArgumentException("Unexpected axis", nameof(SecondCameraAxis))
        };

        public Vector3 CalculateKeyPoint(Vector2 point1, Vector2 point2)
        {
            (float s, Matrix4x4 transformMatrix) = SecondCameraAxis switch
            {
                Axis.X => (CameraDistance / (point1.X - point2.X), Matrix4x4.Identity),
                Axis.Y => (CameraDistance / (point1.Y - point2.Y), Matrix4x4.Identity),
                Axis.Z => (CameraDistance / (point1.X - point1.X),
                            new(0, 0, -1, 0,
                                0, 1, 0, 0,
                                1, 0, 0, 0,
                                0, 0, 0, 1)),
                _ => throw new ArgumentException("Wrong axis.")
            };
            Vector3 point = new(s * point1.X, s * point1.Y, s * FocusLength * 100);
            return Vector3.Transform(point, transformMatrix);
        }
    }
}