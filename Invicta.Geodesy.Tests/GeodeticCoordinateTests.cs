// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

namespace Invicta.Geodesy.Tests
{
    public class GeodeticCoordinateTests
    {
        private static IEnumerable<object[]> UnequalCases => [
            // 0 equal
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],

            // 1 equal
            [new GeodeticCoordinate(1.0d, 0.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
            [new GeodeticCoordinate(0.0d, 2.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],

            // 2 equal
            [new GeodeticCoordinate(1.0d, 2.0d, 0.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
            [new GeodeticCoordinate(0.0d, 2.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
            [new GeodeticCoordinate(1.0d, 0.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
        ];

        private static IEnumerable<object[]> All_WithNotAllMatchingCases => [
            // 0 equal
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), 4.0d],

            // 1 equal
            [new GeodeticCoordinate(4.0d, 2.0d, 3.0d), 4.0d],
            [new GeodeticCoordinate(1.0d, 4.0d, 3.0d), 4.0d],
            [new GeodeticCoordinate(1.0d, 2.0d, 4.0d), 4.0d],

            // 2 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 3.0d), 4.0d],
            [new GeodeticCoordinate(1.0d, 4.0d, 4.0d), 4.0d],
            [new GeodeticCoordinate(4.0d, 2.0d, 4.0d), 4.0d],
        ];

        private static IEnumerable<object[]> Any_WithAtLeastOneMatchingCases => [
            // 1 equal
            [new GeodeticCoordinate(4.0d, 2.0d, 3.0d), 4.0d],
            [new GeodeticCoordinate(1.0d, 4.0d, 3.0d), 4.0d],
            [new GeodeticCoordinate(1.0d, 2.0d, 4.0d), 4.0d],

            // 2 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 3.0d), 4.0d],
            [new GeodeticCoordinate(1.0d, 4.0d, 4.0d), 4.0d],
            [new GeodeticCoordinate(4.0d, 2.0d, 4.0d), 4.0d],

            // 3 equal
            [new GeodeticCoordinate(4.0d, 4.0d, 4.0d), 4.0d],
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

        #region Constructors
        [Test]
        public void ValueConstructor_PopulatesFieldsCorrectly()
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
        public void SpanConstructor_WithLessThanThreeValues_ThrowsArgumentOutOfRangeException()
        {
            static GeodeticCoordinate ctor() => new([1.0d]);

            Assert.That(ctor, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SpanConstructor_WithThreeValues_PopulatesFieldsCorrectly()
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
        public void SpanConstructor_WithMoreThanThreeValues_PopulatesFieldsCorrectly()
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

        [Test]
        public void op_Equality_WithSameValues_ReturnsTrue()
        {
            GeodeticCoordinate coordinate = new(1.0d, 2.0d, 3.0d);

            Assert.That(coordinate == coordinate, Is.True);
        }

        [Test]
        public void op_Equality_WithEqualValues_ReturnsTrue()
        {
            GeodeticCoordinate left = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate right = new(1.0d, 2.0d, 3.0d);

            Assert.That(left == right, Is.True);
        }

        [TestCaseSource(nameof(UnequalCases))]
        public void op_Equality_WithUnequalValues_ReturnsFalse(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            Assert.That(left == right, Is.False);
        }

        [Test]
        public void op_Equality_WithNaN_ReturnsFalse()
        {
            GeodeticCoordinate left = new(NaN, NaN, NaN);
            GeodeticCoordinate right = new(NaN, NaN, NaN);

            Assert.That(left == right, Is.False);
        }

        [Test]
        public void op_Inequality_WithSameValues_ReturnsFalse()
        {
            GeodeticCoordinate coordinate = new(1.0d, 2.0d, 3.0d);

            Assert.That(coordinate != coordinate, Is.False);
        }

        [Test]
        public void op_Inequality_WithEqualValues_ReturnsFalse()
        {
            GeodeticCoordinate left = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate right = new(1.0d, 2.0d, 3.0d);

            Assert.That(left != right, Is.False);
        }

        [TestCaseSource(nameof(UnequalCases))]
        public void op_Inequality_WithUnequalValues_ReturnsTrue(GeodeticCoordinate left, GeodeticCoordinate right)
        {
            Assert.That(left != right, Is.True);
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

        [Test]
        public void All_WithAllMatchingValues_ReturnsTrue()
        {
            GeodeticCoordinate coordinate = new(4.0d, 4.0d, 4.0d);

            Assert.That(GeodeticCoordinate.All(coordinate, 4.0d), Is.True);
        }

        [TestCaseSource(nameof(All_WithNotAllMatchingCases))]
        public void All_WithNotAllMatchingValues_ReturnsFalse(GeodeticCoordinate coordinate, double value)
        {
            Assert.That(GeodeticCoordinate.All(coordinate, value), Is.False);
        }

        [TestCaseSource(nameof(Any_WithAtLeastOneMatchingCases))]
        public void Any_WithAtLeastOneMatchingValue_ReturnsTrue(GeodeticCoordinate coordinate, double value)
        {
            Assert.That(GeodeticCoordinate.Any(coordinate, value), Is.True);
        }

        [Test]
        public void Any_WithNoMatchingValues_ReturnsFalse()
        {
            GeodeticCoordinate coordinate = new(1.0d, 2.0d, 3.0d);

            Assert.That(GeodeticCoordinate.All(coordinate, 4.0d), Is.False);
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
        #endregion
    }
}
