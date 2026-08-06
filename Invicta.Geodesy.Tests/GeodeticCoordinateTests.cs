// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Numerics;

namespace Invicta.Geodesy.Tests
{
    public class GeodeticCoordinateTests
    {
        private const double EarthEquatorialRadius = 6_378_137.0d;
        private const double EarthInverseFlattening = 298.257223563d;
        private const double EarthFlattening = 1.0d / EarthInverseFlattening;
        private const double EarthPolarRadius = EarthEquatorialRadius * (1 - EarthFlattening);
        private const double EarthArithmeticMeanRadius = ((2.0d * EarthEquatorialRadius) + EarthPolarRadius) / 3.0d;

        private const double EarthCircumference = 2.0d * double.Pi * EarthArithmeticMeanRadius;

        private const double LinearTolerance = 0.001d;

        private const double AngularToleranceTurns = LinearTolerance / EarthCircumference;
        private const double AngularToleranceRadians = AngularToleranceTurns * 2.0d * double.Pi;
        private const double AngularToleranceDegrees = AngularToleranceTurns * 360.0d;

        private static readonly GeodeticCoordinate OneTwoThree = new(1.0d, 2.0d, 3.0d);

        private static IEnumerable<object[]> EqualsCases => [
            // 0 equal
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],

            // 1 equal
            [new GeodeticCoordinate(1.0d, 0.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],
            [new GeodeticCoordinate(0.0d, 2.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],
            [new GeodeticCoordinate(0.0d, 0.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],

            // 2 equal
            [new GeodeticCoordinate(1.0d, 2.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],
            [new GeodeticCoordinate(0.0d, 2.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],
            [new GeodeticCoordinate(1.0d, 0.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), false],

             // 3 equal
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), true],

            // identical
            [OneTwoThree, OneTwoThree, true]
        ];

        private static IEnumerable<object[]> AllCases => [
            // 0 equal
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), 4.0d, false],

            // 1 equal
            [new GeodeticCoordinate(4.0d, 2.0d, 3.0d), 4.0d, false],
            [new GeodeticCoordinate(1.0d, 4.0d, 3.0d), 4.0d, false],
            [new GeodeticCoordinate(1.0d, 2.0d, 4.0d), 4.0d, false],

            // 2 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 3.0d), 4.0d, false],
            [new GeodeticCoordinate(1.0d, 4.0d, 4.0d), 4.0d, false],
            [new GeodeticCoordinate(4.0d, 2.0d, 4.0d), 4.0d, false],

            // 3 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 4.0d), 4.0d, true],
        ];

        private static IEnumerable<object[]> AnyCases => [
            // 0 equal
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), 4.0d, false],

            // 1 equal
            [new GeodeticCoordinate(4.0d, 2.0d, 3.0d), 4.0d, true],
            [new GeodeticCoordinate(1.0d, 4.0d, 3.0d), 4.0d, true],
            [new GeodeticCoordinate(1.0d, 2.0d, 4.0d), 4.0d, true],

            // 2 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 3.0d), 4.0d, true],
            [new GeodeticCoordinate(1.0d, 4.0d, 4.0d), 4.0d, true],
            [new GeodeticCoordinate(4.0d, 2.0d, 4.0d), 4.0d, true],

            // 3 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 4.0d), 4.0d, true],
        ];

        private static IEnumerable<object[]> ClampCases => [
            [new GeodeticCoordinate(-2.0d, -3.0d, -4.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(-1.0d, -2.0d, -3.0d) */],
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(-1.0d, -2.0d, -3.0d) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(1.0d, 2.0d, 3.0d) */],
            [new GeodeticCoordinate(2.0d, 3.0d, 4.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(1.0d, 2.0d, 3.0d) */],
        ];

        private static IEnumerable<object[]> ClampWithMinEqualToMaxCases => [
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
        ];

        private const double NaN = double.NaN;
        private static IEnumerable<object[]> ClampWithNaNCases => [
            // NaN, [-, +] => NaN
            [new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],

            // x, [NaN, +] => NaN
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(1.0d, 2.0d, 3.0d) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],

            // x, [-, NaN] => NaN
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],

            // x, [NaN, NaN] => NaN
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
            [new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(NaN, NaN, NaN), new GeodeticCoordinate(NaN, NaN, NaN) /* , new GeodeticCoordinate(NaN, NaN, NaN) */],
        ];

        private const double Nz = double.NegativeZero;
        private static IEnumerable<object[]> ClampWithNegativeZeroCases => [
            // x, [-0, -0] => -0
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],
            [new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],

            // x, [+0, -0] => -0
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],
            [new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(Nz, Nz, Nz) /* , new GeodeticCoordinate(Nz, Nz, Nz) */],

            // x, [-0, +0] => +0
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
            [new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],

            // x, [+0, +0] => +0
            [new GeodeticCoordinate(Nz, Nz, Nz), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d) /* , new GeodeticCoordinate(0.0d, 0.0d, 0.0d) */],
        ];

        private static IEnumerable<object[]> Clamp_WithMinimumGreaterThanMaximumCases => [
            // 1 out of range
            [new GeodeticCoordinate(1.0d, -2.0d, -3.0d), new GeodeticCoordinate(-1.0d, 2.0d, 3.0d)],
            [new GeodeticCoordinate(-1.0d, 2.0d, -3.0d), new GeodeticCoordinate(1.0d, -2.0d, 3.0d)],
            [new GeodeticCoordinate(-1.0d, -2.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, -3.0d)],

            // 2 out of range
            [new GeodeticCoordinate(1.0d, 2.0d, -3.0d), new GeodeticCoordinate(-1.0d, -2.0d, 3.0d)],
            [new GeodeticCoordinate(-1.0d, 2.0d, 3.0d), new GeodeticCoordinate(1.0d, -2.0d, -3.0d)],
            [new GeodeticCoordinate(1.0d, -2.0d, 3.0d), new GeodeticCoordinate(-1.0d, 2.0d, -3.0d)],

            // 3 out of range
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d)],
        ];

        private static IEnumerable<object[]> CountCases => [
            // 0
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), 4.0d, 0],

            // 1
            [new GeodeticCoordinate(4.0d, 2.0d, 3.0d), 4.0d, 1],
            [new GeodeticCoordinate(1.0d, 4.0d, 3.0d), 4.0d, 1],
            [new GeodeticCoordinate(1.0d, 2.0d, 4.0d), 4.0d, 1],

            // 2
            [new GeodeticCoordinate(4.0d, 4.0d, 3.0d), 4.0d, 2],
            [new GeodeticCoordinate(1.0d, 4.0d, 4.0d), 4.0d, 2],
            [new GeodeticCoordinate(4.0d, 2.0d, 4.0d), 4.0d, 2],

            // 3
            [new GeodeticCoordinate(4.0d, 4.0d, 4.0d), 4.0d, 3],
        ];

        private static IEnumerable<object[]> DegreesToRadiansCases => [
            // lat
            [new GeodeticCoordinate(-360.0d, 0.0d, 0.0d), new GeodeticCoordinate(-6.2831853071795862d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(-315.0d, 0.0d, 0.0d), new GeodeticCoordinate(-5.4977871437821380d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(-270.0d, 0.0d, 0.0d), new GeodeticCoordinate(-4.7123889803846897d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(-225.0d, 0.0d, 0.0d), new GeodeticCoordinate(-3.9269908169872414d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(-180.0d, 0.0d, 0.0d), new GeodeticCoordinate(-3.1415926535897931d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(-135.0d, 0.0d, 0.0d), new GeodeticCoordinate(-2.3561944901923448d,  0.0d, 0.0d)],
            [new GeodeticCoordinate( -90.0d, 0.0d, 0.0d), new GeodeticCoordinate(-1.5707963267948966d,  0.0d, 0.0d)],
            [new GeodeticCoordinate( -45.0d, 0.0d, 0.0d), new GeodeticCoordinate(-0.78539816339744828d, 0.0d, 0.0d)],
            [new GeodeticCoordinate(   0.0d, 0.0d, 0.0d), new GeodeticCoordinate( 0.0d,                 0.0d, 0.0d)],
            [new GeodeticCoordinate( +45.0d, 0.0d, 0.0d), new GeodeticCoordinate(+0.78539816339744828d, 0.0d, 0.0d)],
            [new GeodeticCoordinate( +90.0d, 0.0d, 0.0d), new GeodeticCoordinate(+1.5707963267948966d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(+135.0d, 0.0d, 0.0d), new GeodeticCoordinate(+2.3561944901923448d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(+180.0d, 0.0d, 0.0d), new GeodeticCoordinate(+3.1415926535897931d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(+225.0d, 0.0d, 0.0d), new GeodeticCoordinate(+3.9269908169872414d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(+270.0d, 0.0d, 0.0d), new GeodeticCoordinate(+4.7123889803846897d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(+315.0d, 0.0d, 0.0d), new GeodeticCoordinate(+5.4977871437821380d,  0.0d, 0.0d)],
            [new GeodeticCoordinate(+360.0d, 0.0d, 0.0d), new GeodeticCoordinate(+6.2831853071795862d,  0.0d, 0.0d)],

            // lon
            [new GeodeticCoordinate(0.0d, -360.0d, 0.0d), new GeodeticCoordinate(0.0d, -6.2831853071795862d,  0.0d)],
            [new GeodeticCoordinate(0.0d, -315.0d, 0.0d), new GeodeticCoordinate(0.0d, -5.4977871437821380d,  0.0d)],
            [new GeodeticCoordinate(0.0d, -270.0d, 0.0d), new GeodeticCoordinate(0.0d, -4.7123889803846897d,  0.0d)],
            [new GeodeticCoordinate(0.0d, -225.0d, 0.0d), new GeodeticCoordinate(0.0d, -3.9269908169872414d,  0.0d)],
            [new GeodeticCoordinate(0.0d, -180.0d, 0.0d), new GeodeticCoordinate(0.0d, -3.1415926535897931d,  0.0d)],
            [new GeodeticCoordinate(0.0d, -135.0d, 0.0d), new GeodeticCoordinate(0.0d, -2.3561944901923448d,  0.0d)],
            [new GeodeticCoordinate(0.0d,  -90.0d, 0.0d), new GeodeticCoordinate(0.0d, -1.5707963267948966d,  0.0d)],
            [new GeodeticCoordinate(0.0d,  -45.0d, 0.0d), new GeodeticCoordinate(0.0d, -0.78539816339744828d, 0.0d)],
            [new GeodeticCoordinate(0.0d,    0.0d, 0.0d), new GeodeticCoordinate(0.0d,  0.0d,                 0.0d)],
            [new GeodeticCoordinate(0.0d,  +45.0d, 0.0d), new GeodeticCoordinate(0.0d, +0.78539816339744828d, 0.0d)],
            [new GeodeticCoordinate(0.0d,  +90.0d, 0.0d), new GeodeticCoordinate(0.0d, +1.5707963267948966d,  0.0d)],
            [new GeodeticCoordinate(0.0d, +135.0d, 0.0d), new GeodeticCoordinate(0.0d, +2.3561944901923448d,  0.0d)],
            [new GeodeticCoordinate(0.0d, +180.0d, 0.0d), new GeodeticCoordinate(0.0d, +3.1415926535897931d,  0.0d)],
            [new GeodeticCoordinate(0.0d, +225.0d, 0.0d), new GeodeticCoordinate(0.0d, +3.9269908169872414d,  0.0d)],
            [new GeodeticCoordinate(0.0d, +270.0d, 0.0d), new GeodeticCoordinate(0.0d, +4.7123889803846897d,  0.0d)],
            [new GeodeticCoordinate(0.0d, +315.0d, 0.0d), new GeodeticCoordinate(0.0d, +5.4977871437821380d,  0.0d)],
            [new GeodeticCoordinate(0.0d, +360.0d, 0.0d), new GeodeticCoordinate(0.0d, +6.2831853071795862d,  0.0d)],

            // alt
            [new GeodeticCoordinate(0.0d, 0.0d,         0.0d), new GeodeticCoordinate(0.0d, 0.0d,         0.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 1_592_750.0d), new GeodeticCoordinate(0.0d, 0.0d, 1_592_750.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 3_185_500.0d), new GeodeticCoordinate(0.0d, 0.0d, 3_185_500.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 4_778_250.0d), new GeodeticCoordinate(0.0d, 0.0d, 4_778_250.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 6_371_000.0d), new GeodeticCoordinate(0.0d, 0.0d, 6_371_000.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 7_963_750.0d), new GeodeticCoordinate(0.0d, 0.0d, 7_963_750.0d)],
        ];

        #region Constructors
        [Test]
        public void Constructor_WithThreeValues_ReturnsExpectedResult()
        {
            GeodeticCoordinate coordinate = new(1.0d, 2.0d, 3.0d);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(coordinate.Latitude, Is.EqualTo(1.0d));
                Assert.That(coordinate.Longitude, Is.EqualTo(2.0d));
                Assert.That(coordinate.Altitude, Is.EqualTo(3.0d));
            }
        }

        [Test]
        public void Constructor_WithLessThanThreeValuesInSpan_ThrowsArgumentOutOfRangeException()
        {
            static GeodeticCoordinate ctor() => new([1.0d]);

            Assert.That(ctor, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WithThreeValuesInSpan_ReturnsExpectedResult()
        {
            GeodeticCoordinate coordinate = new([1.0d, 2.0d, 3.0d]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(coordinate.Latitude, Is.EqualTo(1.0d));
                Assert.That(coordinate.Longitude, Is.EqualTo(2.0d));
                Assert.That(coordinate.Altitude, Is.EqualTo(3.0d));
            }
        }

        [Test]
        public void Constructor_WithMoreThanThreeValuesInSpan_ReturnsExpectedResult()
        {
            GeodeticCoordinate coordinate = new([1.0d, 2.0d, 3.0d, 4.0d, 5.0d]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(coordinate.Latitude, Is.EqualTo(1.0d));
                Assert.That(coordinate.Longitude, Is.EqualTo(2.0d));
                Assert.That(coordinate.Altitude, Is.EqualTo(3.0d));
            }
        }
        #endregion

        #region Properties
        [Test]
        public void Zero_ReturnsExpectedResult()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(GeodeticCoordinate.Zero.Latitude, Is.Zero);
                Assert.That(GeodeticCoordinate.Zero.Longitude, Is.Zero);
                Assert.That(GeodeticCoordinate.Zero.Altitude, Is.Zero);
            }
        }
        #endregion

        #region Operators
        [Test]
        public void op_Addition_ReturnsExpectedResult()
        {
            GeodeticCoordinate left = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate right = new(4.0d, 5.0d, 6.0d);

            GeodeticCoordinate result = left + right;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(1.0d + 4.0d));
                Assert.That(result.Longitude, Is.EqualTo(2.0d + 5.0d));
                Assert.That(result.Altitude, Is.EqualTo(3.0d + 6.0d));
            }
        }

        [TestCaseSource(nameof(EqualsCases))]
        public void op_Equality_ReturnsExpectedResult(GeodeticCoordinate left, GeodeticCoordinate right, bool expectedResult)
        {
            Assert.That(left == right, Is.EqualTo(expectedResult));
        }

        [Test]
        public void op_Equality_WithNaN_ReturnsFalse()
        {
            GeodeticCoordinate left = new(NaN, NaN, NaN);
            GeodeticCoordinate right = new(NaN, NaN, NaN);

            Assert.That(left == right, Is.False);
        }

        [TestCaseSource(nameof(EqualsCases))]
        public void op_Inequality_ReturnsExpectedResult(GeodeticCoordinate left, GeodeticCoordinate right, bool inverseExpectedResult)
        {
            Assert.That(left != right, Is.EqualTo(!inverseExpectedResult));
        }

        [Test]
        public void op_Inequality_WithNaN_ReturnsTrue()
        {
            GeodeticCoordinate left = new(NaN, NaN, NaN);
            GeodeticCoordinate right = new(NaN, NaN, NaN);

            Assert.That(left != right, Is.True);
        }

        [Test]
        public void op_Subtraction_ReturnsExpectedResult()
        {
            GeodeticCoordinate left = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate right = new(4.0d, 5.0d, 6.0d);

            GeodeticCoordinate result = left - right;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(left.Latitude - right.Latitude));
                Assert.That(result.Longitude, Is.EqualTo(left.Longitude - right.Longitude));
                Assert.That(result.Altitude, Is.EqualTo(left.Altitude - right.Altitude));
            }
        }
        #endregion

        #region Static Methods
        [Test]
        public void Add_ReturnsExpectedResult()
        {
            GeodeticCoordinate left = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate right = new(4.0d, 5.0d, 6.0d);

            GeodeticCoordinate result = GeodeticCoordinate.Add(left, right);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(left.Latitude + right.Latitude));
                Assert.That(result.Longitude, Is.EqualTo(left.Longitude + right.Longitude));
                Assert.That(result.Altitude, Is.EqualTo(left.Altitude + right.Altitude));
            }
        }

        [TestCaseSource(nameof(AllCases))]
        public void All_ReturnsExpectedResult(GeodeticCoordinate coordinate, double value, bool expectedResult)
        {
            Assert.That(GeodeticCoordinate.All(coordinate, value), Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(AnyCases))]
        public void Any_ReturnsExpectedResult(GeodeticCoordinate coordinate, double value, bool expectedResult)
        {
            Assert.That(GeodeticCoordinate.Any(coordinate, value), Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(ClampCases))]
        [TestCaseSource(nameof(ClampWithMinEqualToMaxCases))]
        [TestCaseSource(nameof(ClampWithNaNCases))]
        [TestCaseSource(nameof(ClampWithNegativeZeroCases))]
        public void Clamp_ReturnsSameResultAsDoubleClamp(
            GeodeticCoordinate value, GeodeticCoordinate min, GeodeticCoordinate max)
        {
            GeodeticCoordinate result = GeodeticCoordinate.Clamp(value, min, max);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(double.Clamp(value.Latitude, min.Latitude, max.Latitude)));
                Assert.That(result.Longitude, Is.EqualTo(double.Clamp(value.Longitude, min.Longitude, max.Longitude)));
                Assert.That(result.Altitude, Is.EqualTo(double.Clamp(value.Altitude, min.Altitude, max.Altitude)));
            }
        }

        [TestCaseSource(nameof(Clamp_WithMinimumGreaterThanMaximumCases))]
        public void Clamp_WithMinimumGreaterThanMaximum_ThrowsArgumentException(GeodeticCoordinate min, GeodeticCoordinate max)
        {
            GeodeticCoordinate value = new(1.0d, 2.0d, 3.0d);

            Assert.That(() => GeodeticCoordinate.Clamp(value, min, max), Throws.ArgumentException);
        }

        [TestCaseSource(nameof(ClampCases))]
        [TestCaseSource(nameof(ClampWithMinEqualToMaxCases))]
        [TestCaseSource(nameof(ClampWithNaNCases))]
        [TestCaseSource(nameof(ClampWithNegativeZeroCases))]
        public void ClampNative_ReturnsSameResultAsDoubleClampNative(
            GeodeticCoordinate value, GeodeticCoordinate min, GeodeticCoordinate max)
        {
            GeodeticCoordinate result = GeodeticCoordinate.ClampNative(value, min, max);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(double.ClampNative(value.Latitude, min.Latitude, max.Latitude)));
                Assert.That(result.Longitude, Is.EqualTo(double.ClampNative(value.Longitude, min.Longitude, max.Longitude)));
                Assert.That(result.Altitude, Is.EqualTo(double.ClampNative(value.Altitude, min.Altitude, max.Altitude)));
            }
        }

        [TestCaseSource(nameof(Clamp_WithMinimumGreaterThanMaximumCases))]
        public void ClampNative_WithMinimumGreaterThanMaximum_ThrowsArgumentException(GeodeticCoordinate min, GeodeticCoordinate max)
        {
            GeodeticCoordinate value = new(1.0d, 2.0d, 3.0d);

            Assert.That(() => GeodeticCoordinate.ClampNative(value, min, max), Throws.ArgumentException);
        }

        [TestCaseSource(nameof(CountCases))]
        public void Count_ReturnsExpectedResult(GeodeticCoordinate coordinate, double value, int expectedCount)
        {
            Assert.That(GeodeticCoordinate.Count(coordinate, value), Is.EqualTo(expectedCount));
        }

        [Test]
        public void Create_WithThreeValues_ReturnsExpectedResult()
        {
            GeodeticCoordinate coordinate = GeodeticCoordinate.Create(1.0d, 2.0d, 3.0d);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(coordinate.Latitude, Is.EqualTo(1.0d));
                Assert.That(coordinate.Longitude, Is.EqualTo(2.0d));
                Assert.That(coordinate.Altitude, Is.EqualTo(3.0d));
            }
        }


        [Test]
        public void Create_WithLessThanThreeValuesInSpan_ThrowsArgumentOutOfRangeException()
        {
            static GeodeticCoordinate Create() => GeodeticCoordinate.Create([1.0d]);

            Assert.That(Create, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Create_WithThreeValuesInSpan_ReturnsExpectedResult()
        {
            GeodeticCoordinate coordinate = GeodeticCoordinate.Create([1.0d, 2.0d, 3.0d]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(coordinate.Latitude, Is.EqualTo(1.0d));
                Assert.That(coordinate.Longitude, Is.EqualTo(2.0d));
                Assert.That(coordinate.Altitude, Is.EqualTo(3.0d));
            }
        }

        [Test]
        public void Create_WithMoreThanThreeValuesInSpan_ReturnsExpectedResult()
        {
            GeodeticCoordinate coordinate = GeodeticCoordinate.Create([1.0d, 2.0d, 3.0d, 4.0d, 5.0d]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(coordinate.Latitude, Is.EqualTo(1.0d));
                Assert.That(coordinate.Longitude, Is.EqualTo(2.0d));
                Assert.That(coordinate.Altitude, Is.EqualTo(3.0d));
            }
        }

        [TestCaseSource(nameof(DegreesToRadiansCases))]
        public void DegreesToRadians_ReturnsExpectedResult(
            GeodeticCoordinate degrees, GeodeticCoordinate expectedRadians)
        {
            GeodeticCoordinate radians = GeodeticCoordinate.DegreesToRadians(degrees);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(radians.Latitude, Is.EqualTo(expectedRadians.Latitude).Within(AngularToleranceRadians));
                Assert.That(radians.Longitude, Is.EqualTo(expectedRadians.Longitude).Within(AngularToleranceRadians));
                Assert.That(radians.Altitude, Is.EqualTo(expectedRadians.Altitude).Within(LinearTolerance));
            }
        }

        [Test]
        public void EqualsAll_ReturnsExpectedResult()
        {

        }
        #endregion
    }
}
