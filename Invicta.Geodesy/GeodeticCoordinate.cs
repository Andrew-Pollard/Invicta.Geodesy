// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Invicta.Geodesy
{
    /// <summary>Represents a geodetic coordinate with a latitude, longitude and altitude value.</summary>
    public struct GeodeticCoordinate : IEquatable<GeodeticCoordinate>
    {
        /// <summary>Specifies the alignment of the coordinate as used by the <see cref="LoadAligned(double*)" /> and <see cref="VectorD.StoreAligned(Vector3D, double*)" /> APIs.</summary>
        /// <remarks>
        ///     <para>
        ///       Different environments all have their own concepts of alignment/packing.
        ///       For example, a <c>Vector3D</c> in .NET is 4-byte aligned and 12-bytes in size,
        ///       in GLSL a <c>vec3</c> is 16-byte aligned and 16-byte sized, while in HLSL a
        ///       <c>float3</c> is functionally 8-byte aligned and 12-byte sized. These differences
        ///       make it impossible to define a "correct" alignment; additionally, the nuance
        ///       in environments like HLSL where size is not a multiple of alignment introduce complications.
        ///     </para>
        ///     <para>
        ///       For the purposes of the <c>LoadAligned</c> and <c>StoreAligned</c> APIs we
        ///       therefore pick a value that allows for a broad range of compatibility while
        ///       also allowing more optimal codegen for various target platforms.
        ///     </para>
        /// </remarks>
        internal const int Alignment = 8;

        /// <summary>The latitude component of the coordinate.</summary>
        public double Latitude;

        /// <summary>The longitude component of the coordinate.</summary>
        public double Longitude;

        /// <summary>The altitude component of the coordinate.</summary>
        public double Altitude;

        internal const int ElementCount = 3;

        // CTOR(LL, A)?

        /// <summary>Creates a new <see cref="GeodeticCoordinate"/> object whose elements have the specified values.</summary>
        /// <param name="latitude">The value to assign to the <see cref="Latitude"/> field.</param>
        /// <param name="longitude">The value to assign to the <see cref="Longitude"/> field.</param>
        /// <param name="altitude">The value to assign to the <see cref="Altitude"/> field.</param>
        public GeodeticCoordinate(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        /// <summary>Constructs a coordinate from the given <see cref="ReadOnlySpan{Double}"/>. The span must contain at least 3 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        public GeodeticCoordinate(ReadOnlySpan<double> values)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(values.Length, 3);

            Latitude = values[0];
            Longitude = values[1];
            Altitude = values[2];
        }

        /// <summary>Gets a coordinate whose elements are equal to zero.</summary>
        /// <value>A coordinate whose elements are equal to zero (that is, it returns the coordinate <c>(0,0,0)</c>).</value>
        public static GeodeticCoordinate Zero => new(0.0d, 0.0d, 0.0d);

        // INDEXER?

        /// <summary>Adds two coordinates together.</summary>
        /// <param name="left">The first coordinate to add.</param>
        /// <param name="right">The second coordinate to add.</param>
        /// <returns>The summed coordinate.</returns>
        /// <remarks>The <see cref="operator +(GeodeticCoordinate, GeodeticCoordinate)"/> method defines the addition operation for <see cref="GeodeticCoordinate"/> objects.</remarks>
        public static GeodeticCoordinate operator +(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(left.Latitude + right.Latitude, left.Longitude + right.Longitude, left.Altitude + right.Altitude);
        }

        /// <summary>Returns a value that indicates whether each pair of elements in two specified coordinates is equal.</summary>
        /// <param name="left">The first coordinate to compare.</param>
        /// <param name="right">The second coordinate to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Two <see cref="GeodeticCoordinate"/> objects are equal if each element in <paramref name="left"/> is equal to the corresponding element in <paramref name="right"/>.</remarks>
        public static bool operator ==(GeodeticCoordinate left, GeodeticCoordinate right) => left.Equals(right);

        /// <summary>Returns a value that indicates whether two specified coordinates are not equal.</summary>
        /// <param name="left">The first coordinate to compare.</param>
        /// <param name="right">The second coordinate to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(GeodeticCoordinate left, GeodeticCoordinate right) => !(left == right);

        /// <summary>Subtracts the second coordinate from the first.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>The coordinate that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="operator -(GeodeticCoordinate, GeodeticCoordinate)" /> method defines the subtraction operation for <see cref="GeodeticCoordinate" /> objects.</remarks>
        public static GeodeticCoordinate operator -(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(left.Latitude - right.Latitude, left.Longitude - right.Longitude, left.Altitude - right.Altitude);
        }

        // ABS?

        /// <summary>Adds two coordinates together.</summary>
        /// <param name="left">The first coordinate to add.</param>
        /// <param name="right">The second coordinate to add.</param>
        /// <returns>The summed vector.</returns>
        public static GeodeticCoordinate Add(GeodeticCoordinate left, GeodeticCoordinate right) => left + right;

        /// <summary>Determines if all elements of a coordinate are equal to a given value.</summary>
        /// <param name="coordinate">The coordinate whose elements are being checked.</param>
        /// <param name="value">The value to check for in <paramref name="coordinate"/>.</param>
        /// <returns><see langword="true"/> if all elements of <paramref name="coordinate"/> are equal to <paramref name="value"/>; otherwise, <see langword="false"/>.</returns>
        public static bool All(GeodeticCoordinate coordinate, double value)
        {
            return coordinate.Latitude == value && coordinate.Longitude == value && coordinate.Altitude == value;
        }

        /// <summary>Determines if any elements of a coordinate are equal to a given value.</summary>
        /// <param name="coordinate">The coordinate whose elements are being checked.</param>
        /// <param name="value">The value to check for in <paramref name="coordinate"/>.</param>
        /// <returns><see langword="true"/> if any elements of <paramref name="coordinate"/> are equal to <paramref name="value"/>; otherwise, <see langword="false"/>.</returns>
        public static bool Any(GeodeticCoordinate coordinate, double value)
        {
            return coordinate.Latitude == value || coordinate.Longitude == value || coordinate.Altitude == value;
        }

        /// <summary>Restricts a coordinate between a minimum and maximum value.</summary>
        /// <param name="value">The coordinate to restrict.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted coordinate.</returns>
        public static GeodeticCoordinate Clamp(GeodeticCoordinate value, GeodeticCoordinate min, GeodeticCoordinate max)
        {
            return new GeodeticCoordinate(
                double.Clamp(value.Latitude, min.Latitude, max.Latitude),
                double.Clamp(value.Longitude, min.Longitude, max.Longitude),
                double.Clamp(value.Altitude, min.Altitude, max.Altitude));
        }

        /// <summary>Restricts a coordinate between a minimum and maximum value using platform specific behavior for <c>NaN</c> and <c>NegativeZero</c>.</summary>
        /// <param name="value">The coordinate to restrict.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted coordinate.</returns>
        public static GeodeticCoordinate ClampNative(GeodeticCoordinate value, GeodeticCoordinate min, GeodeticCoordinate max)
        {
            return new GeodeticCoordinate(
                double.ClampNative(value.Latitude, min.Latitude, max.Latitude),
                double.ClampNative(value.Longitude, min.Longitude, max.Longitude),
                double.ClampNative(value.Altitude, min.Altitude, max.Altitude));
        }

        // COPY SIGN?

        /// <summary>Determines the number of elements in a coordinate that are equal to a given value.</summary>
        /// <param name="coordinate">The coordinate whose elements are being checked.</param>
        /// <param name="value">The value to check for in <paramref name="coordinate"/>.</param>
        /// <returns>The number of elements in <paramref name="coordinate"/> that are equal to <paramref name="value"/>.</returns>
        public static int Count(GeodeticCoordinate coordinate, double value)
        {
            int count = 0;

            count += coordinate.Latitude == value ? 1 : 0;
            count += coordinate.Longitude == value ? 1 : 0;
            count += coordinate.Altitude == value ? 1 : 0;

            return count;
        }

        // CREATE(LL, A)?

        /// <summary>Creates a coordinate whose elements have the specified values.</summary>
        /// <param name="latitude">The value to assign to the <see cref="Latitude"/> field.</param>
        /// <param name="longitude">The value to assign to the <see cref="Longitude"/> field.</param>
        /// <param name="altitude">The value to assign to the <see cref="Altitude"/> field.</param>
        /// <returns>A new <see cref="GeodeticCoordinate"/> whose elements have the specified values.</returns>
        public static GeodeticCoordinate Create(double latitude, double longitude, double altitude)
        {
            return new GeodeticCoordinate(latitude, longitude, altitude);
        }

        /// <summary>Constructs a coordinate from the given <see cref="ReadOnlySpan{Double}"/>. The span must contain at least 3 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        /// <returns>A new <see cref="GeodeticCoordinate"/> whose elements have the specified values.</returns>
        public static GeodeticCoordinate Create(ReadOnlySpan<double> values)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(values.Length, 3);

            return new GeodeticCoordinate(values[0], values[1], values[2]);
        }

        /// <summary>Converts a given coordinate from radians to radians.</summary>
        /// <param name="degrees">The coordinate to convert ot radians.</param>
        /// <returns>The coordinate of radians converted to radians.</returns>
        public static GeodeticCoordinate DegreesToRadians(GeodeticCoordinate degrees)
        {
            return new GeodeticCoordinate(
                double.DegreesToRadians(degrees.Latitude),
                double.DegreesToRadians(degrees.Longitude),
                degrees.Altitude);
        }

        /// <summary>Compares two coordinates to determine if all elements are equal.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if all elements in <paramref name="left"/> were equal to the corresponding element in <paramref name="right"/>.</returns>
        public static bool EqualsAll(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude == right.Latitude && left.Longitude == right.Longitude && left.Altitude == right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if any elements are equal.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if any elements in <paramref name="left"/> were equal to the corresponding element in <paramref name="right"/>.</returns>
        public static bool EqualsAny(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude == right.Latitude || left.Longitude == right.Longitude || left.Altitude == right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if all elements are greater.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if all elements in <paramref name="left"/> were greater than the corresponding element in <paramref name="right"/>.</returns>
        public static bool GreaterThanAll(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude > right.Latitude && left.Longitude > right.Longitude && left.Altitude > right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if any elements are greater.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if any elements in <paramref name="left"/> were greater than the corresponding element in <paramref name="right"/>.</returns>
        public static bool GreaterThanAny(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude > right.Latitude || left.Longitude > right.Longitude || left.Altitude > right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if all elements are greater or equal.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if all elements in <paramref name="left"/> were greater than or equal to the corresponding element in <paramref name="right"/>.</returns>
        public static bool GreaterThanOrEqualAll(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude >= right.Latitude && left.Longitude >= right.Longitude && left.Altitude >= right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if any elements are greater or equal.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if any elements in <paramref name="left"/> were greater than or equal to the corresponding element in <paramref name="right"/>.</returns>
        public static bool GreaterThanOrEqualAny(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude >= right.Latitude || left.Longitude >= right.Longitude || left.Altitude >= right.Altitude;
        }

        // INDEX OF?

        // LAST INDEX OF?

        // LERP?

        /// <summary>Compares two coordinates to determine if all elements are less.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if all elements in <paramref name="left"/> were less than the corresponding element in <paramref name="right"/>.</returns>
        public static bool LessThanAll(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude < right.Latitude && left.Longitude < right.Longitude && left.Altitude < right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if any elements are less.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if any elements in <paramref name="left"/> were less than the corresponding element in <paramref name="right"/>.</returns>
        public static bool LessThanAny(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude < right.Latitude || left.Longitude < right.Longitude || left.Altitude < right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if all elements are less or equal.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if all elements in <paramref name="left"/> were less than or equal to the corresponding element in <paramref name="right"/>.</returns>
        public static bool LessThanOrEqualAll(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude <= right.Latitude && left.Longitude <= right.Longitude && left.Altitude <= right.Altitude;
        }

        /// <summary>Compares two coordinates to determine if any elements are less or equal.</summary>
        /// <param name="left">The coordinate to compare with <paramref name="right"/>.</param>
        /// <param name="right">The coordinate to compare with <paramref name="left"/>.</param>
        /// <returns><see langword="true"/> if any elements in <paramref name="left"/> were less than or equal to the corresponding element in <paramref name="right"/>.</returns>
        public static bool LessThanOrEqualAny(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return left.Latitude <= right.Latitude || left.Longitude <= right.Longitude || left.Altitude <= right.Altitude;
        }

        /// <summary>Loads a coordinate from the given source.</summary>
        /// <param name="source">The source from which the coordinate will be loaded.</param>
        /// <returns>The coordinate loaded from <paramref name="source"/>.</returns>
        [CLSCompliant(false)]
        public static unsafe GeodeticCoordinate Load(double* source) => LoadUnsafe(in *source);

        /// <summary>Loads a coordinate from the given aligned source.</summary>
        /// <param name="source">The aligned source from which the coordinate will be loaded.</param>
        /// <returns>The coordinate loaded from <paramref name="source"/>.</returns>
        [CLSCompliant(false)]
        public static unsafe GeodeticCoordinate LoadAligned(double* source)
        {
            if (((nuint)(source) % Alignment) != 0)
            {
                throw new AccessViolationException();
            }

            return *(GeodeticCoordinate*)source;
        }

        /// <summary>Loads a coordinate from the given aligned source.</summary>
        /// <param name="source">The aligned source from which the coordinate will be loaded.</param>
        /// <returns>The coordinate loaded from <paramref name="source"/>.</returns>
        [CLSCompliant(false)]
        public static unsafe GeodeticCoordinate LoadAlignedNonTemporal(double* source) => LoadAligned(source);

        /// <summary>Loads a coordinate from the given source.</summary>
        /// <param name="source">The source from which the coordinate will be loaded.</param>
        /// <returns>The coordinate loaded from <paramref name="source"/>.</returns>
        public static GeodeticCoordinate LoadUnsafe(ref readonly double source)
        {
            ref readonly byte address = ref Unsafe.As<double, byte>(ref Unsafe.AsRef(in source));
            return Unsafe.ReadUnaligned<GeodeticCoordinate>(in address);
        }

        /// <summary>Loads a coordinate from the given source and element offset.</summary>
        /// <param name="source">The source to which <paramref name="elementOffset"/> will be added before loading to coordinate.</param>
        /// <param name="elementOffset">The element offset from <paramref name="source"/> from which the coordinate will be loaded.</param>
        /// <returns>The coordinate loaded from <paramref name="source"/> plus <paramref name="elementOffset"/>.</returns>
        [CLSCompliant(false)]
        public static GeodeticCoordinate LoadUnsafe(ref readonly double source, nuint elementOffset)
        {
            ref readonly byte address = ref Unsafe.As<double, byte>(ref Unsafe.Add(ref Unsafe.AsRef(in source), (nint)elementOffset));
            return Unsafe.ReadUnaligned<GeodeticCoordinate>(in address);
        }

        /// <summary>Returns a coordinate whose elements are the maximum of each of the pairs of elements in two specified coordinates.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>The maximized coordinate.</returns>
        public static GeodeticCoordinate Max(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.Max(left.Latitude, right.Latitude),
                double.Max(left.Longitude, right.Longitude),
                double.Max(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates to compute which has the greater magnitude on a per-element basis.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it has a greater magnitude than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MaxMagnitude(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MaxMagnitude(left.Latitude, right.Latitude),
                double.MaxMagnitude(left.Longitude, right.Longitude),
                double.MaxMagnitude(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates, on a per-element basis, to compute which has the greater magnitude and returning the other value if an element is <c>NaN</c>.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it has a greater magnitude than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MaxMagnitudeNumber(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MaxMagnitudeNumber(left.Latitude, right.Latitude),
                double.MaxMagnitudeNumber(left.Longitude, right.Longitude),
                double.MaxMagnitudeNumber(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates to determine which is greater on a per-element basis using platform specific behavior for <c>NaN</c> and <c>NegativeZero</c>.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it is greater than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MaxNative(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MaxNative(left.Latitude, right.Latitude),
                double.MaxNative(left.Longitude, right.Longitude),
                double.MaxNative(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates, on a per-element basis, to compute which is greater and returning the other value if an element is <c>NaN</c>.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it is greater than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MaxNumber(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MaxNumber(left.Latitude, right.Latitude),
                double.MaxNumber(left.Longitude, right.Longitude),
                double.MaxNumber(left.Altitude, right.Altitude));
        }

        /// <summary>Returns a coordinate whose elements are the minimum of each of the pairs of elements in two specified coordinates.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>The minimized coordinate.</returns>
        public static GeodeticCoordinate Min(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.Min(left.Latitude, right.Latitude),
                double.Min(left.Longitude, right.Longitude),
                double.Min(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates to compute which has the lesser magnitude on a per-element basis.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it has a lesser magnitude than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MinMagnitude(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MinMagnitude(left.Latitude, right.Latitude),
                double.MinMagnitude(left.Longitude, right.Longitude),
                double.MinMagnitude(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates, on a per-element basis, to compute which has the lesser magnitude and returning the other value if an element is <c>NaN</c>.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it has a lesser magnitude than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MinMagnitudeNumber(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MinMagnitudeNumber(left.Latitude, right.Latitude),
                double.MinMagnitudeNumber(left.Longitude, right.Longitude),
                double.MinMagnitudeNumber(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates to determine which is lesser on a per-element basis using platform specific behavior for <c>NaN</c> and <c>NegativeZero</c>.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it is lesser than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MinNative(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MinNative(left.Latitude, right.Latitude),
                double.MinNative(left.Longitude, right.Longitude),
                double.MinNative(left.Altitude, right.Altitude));
        }

        /// <summary>Compares two coordinates, on a per-element basis, to compute which is lesser and returning the other value if an element is <c>NaN</c>.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>A coordinate where the corresponding element comes from <paramref name="left"/> if it is lesser than <paramref name="right"/>; otherwise, <paramref name="right"/>.</returns>
        public static GeodeticCoordinate MinNumber(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            return new GeodeticCoordinate(
                double.MinNumber(left.Latitude, right.Latitude),
                double.MinNumber(left.Longitude, right.Longitude),
                double.MinNumber(left.Altitude, right.Altitude));
        }

        /// <summary>Determines if no elements of a vector are equal to a given value.</summary>
        /// <param name="coordinate">The coordinate whose elements are being checked.</param>
        /// <param name="value">The value to check for in <paramref name="coordinate"/>.</param>
        /// <returns><see langword="true"/> if no elements of <paramref name="coordinate"/> are equal to <paramref name="value"/>; otherwise, <see langword="false"/>.</returns>
        public static bool None(GeodeticCoordinate coordinate, double value)
        {
            return coordinate.Latitude != value && coordinate.Longitude != value && coordinate.Altitude != value;
        }

        // NORMALIZE?

        /// <summary>Converts a given coordinate from radians to degrees.</summary>
        /// <param name="radians">The coordinate to convert to degrees.</param>
        /// <returns>The coordinate of radians converted to degrees.</returns>
        public static GeodeticCoordinate RadiansToDegrees(GeodeticCoordinate radians)
        {
            return new GeodeticCoordinate(
                double.RadiansToDegrees(radians.Latitude),
                double.RadiansToDegrees(radians.Longitude),
                radians.Altitude);
        }

        /// <summary>Subtracts the second coordinate from the first.</summary>
        /// <param name="left">The first coordinate.</param>
        /// <param name="right">The second coordinate.</param>
        /// <returns>The difference coordinate.</returns>
        public static GeodeticCoordinate Subtract(GeodeticCoordinate left, GeodeticCoordinate right) => left - right;

        /// <summary>Copies the elements of the coordinate to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least three elements. The method copies the coordinate's elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(double[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons

            if (array.Length < ElementCount)
            {
                throw new ArgumentException("Destination is too short", nameof(array));
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the coordinate to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the coordinate.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the three coordinate elements. In other words, elements <paramref name="index" />, <paramref name="index" /> + 1, and <paramref name="index" /> + 2 must already exist in <paramref name="array" />.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
        /// <paramref name="index" /> is greater than or equal to the array length.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(double[] array, int index)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons

            if ((uint)index >= (uint)array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if ((array.Length - index) < ElementCount)
            {
                throw new ArgumentException("Destination is too short", nameof(array));
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref array[index]), this);
        }

        /// <summary>Copies the coordinate to the given <see cref="Span{T}" />. The length of the destination span must be at least 3.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source coordinate is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<double> destination)
        {
            if (destination.Length < ElementCount)
            {
                throw new ArgumentException("Destination is too short", nameof(destination));
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the coordinate to the given <see cref="Span{Double}" />. The length of the destination span must be at least 3.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source coordinate was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source coordinate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<double> destination)
        {
            if (destination.Length < ElementCount)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="GeodeticCoordinate" /> object and their corresponding elements are equal.</remarks>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is GeodeticCoordinate coordinate && Equals(coordinate);

        /// <summary>Returns a value that indicates whether this instance and another coordinate are equal.</summary>
        /// <param name="other">The other coordinate.</param>
        /// <returns><see langword="true" /> if the two coordinate are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two coordinate are equal if their <see cref="Latitude" />, <see cref="Longitude" />, and <see cref="Altitude" /> elements are equal.</remarks>
        public readonly bool Equals(GeodeticCoordinate other) => Latitude == other.Latitude && Longitude == other.Longitude && Altitude == other.Altitude;

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(Latitude, Longitude, Altitude);

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the coordinate is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the coordinate is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the coordinate is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return $"<{Latitude.ToString(format, formatProvider)}{separator} {Longitude.ToString(format, formatProvider)}{separator} {Altitude.ToString(format, formatProvider)}>";
        }
    }
}
