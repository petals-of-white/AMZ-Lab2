namespace Lab2.Core

open FSharp.Data.UnitSystems.SI.UnitSymbols
open System.Numerics

[<Measure>]
type cm = m^- 3

[<Measure>]
type px

type Axis =
    | X
    | Y
    | Z

type VariantData =
    { CameraDistance: int<cm>
      FocusLength: int<m>
      SecondCameraPos: Axis }

module UnitConversion =
    let mToCm (meters: float<m>) = meters * 100.0<cm/m>
    let cmToPixels (cm: float<cm>) = cm * 10.0<px/cm>

module KeyPointLocation =
    let calculate
        { CameraDistance = distance
          FocusLength = f
          SecondCameraPos = cam2Pos }
        (projFirst: Vector2)
        (projSecond: Vector2)
        =
        let s = single distance / (projFirst.X - projSecond.X)
        let fCm = UnitConversion.mToCm (LanguagePrimitives.FloatWithMeasure(double f))

        Vector3.Transform(
            Vector3(s * projFirst.X, s * projFirst.Y, s * single fCm),
            match cam2Pos with
            | X -> Matrix4x4.Identity
            | Y -> Matrix4x4(
                    0f, 1f, 0f, 0f,
                    1f, 0f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f)
            | Z -> Matrix4x4(
                    0f, 0f, -1f, 0f,
                    0f, 1f, 0f, 0f,
                    1f, 0f, 0f, 0f,
                    0f, 0f, 0f, 1f
                )
        )