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

module Coordinates =
    let firstCameraPos (_: VariantData) = Vector3(0f, 0f, 0f)

    let secondCameraPos =
        function
        | { CameraDistance = b
            SecondCameraPos = X } -> Vector3(float32 b, 0f, 0f)
        | { CameraDistance = b
            SecondCameraPos = Y } -> Vector3(0f, float32 b, 0f)
        | { CameraDistance = b
            SecondCameraPos = Z } -> Vector3(0f, 0f, float32 b)

    let firstPointAbsPos options (point1: Vector2) =
        let focusLengthCm = float32 options.FocusLength * 100f

        match options.SecondCameraPos with
        | X
        | Y -> Vector3(point1.X, point1.Y, focusLengthCm)
        | Z -> Vector3(focusLengthCm, point1.Y, -point1.X)

    let secondPointAbsPos options (point2: Vector2) =
        let focusLengthCm = float32 options.FocusLength * 100f

        match options.SecondCameraPos with
        | X -> Vector3(point2.X + float32 options.CameraDistance, point2.Y, focusLengthCm)
        | Y -> Vector3(point2.X, point2.Y + float32 options.CameraDistance, focusLengthCm)
        | Z -> Vector3(focusLengthCm, point2.Y, -point2.X + float32 options.CameraDistance)

    let calculateKeyPoint
        { CameraDistance = distance
          FocusLength = f
          SecondCameraPos = cam2Pos }
        (projFirst: Vector2)
        (projSecond: Vector2)
        =
        let fCm =
            UnitConversion.mToCm (LanguagePrimitives.FloatWithMeasure(double f))
            |> single
        
        let s, transformMat =
            match cam2Pos with
            | X -> (single distance / (projFirst.X - projSecond.X), Matrix4x4.Identity)
            | Y -> (single distance / (projFirst.Y - projSecond.Y), Matrix4x4.Identity)
            | Z ->
                (single distance / (projFirst.X - projSecond.X),
                 Matrix4x4(0f, 0f, -1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 1f))

        Vector3.Transform(Vector3(s * projFirst.X, s * projFirst.Y, s * fCm), transformMat)