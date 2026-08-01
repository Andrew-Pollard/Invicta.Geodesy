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

        private static IEnumerable<object[]> ClampCases => [
            // min = max
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d)],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d)],

            // min < max
            [new GeodeticCoordinate(-2.0d, -3.0d, -4.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d)],
            [new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d)],
            [new GeodeticCoordinate(0.0d, 0.0d, 0.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(0.0d, 0.0d, 0.0d)],
            [new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
            [new GeodeticCoordinate(2.0d, 3.0d, 4.0d), new GeodeticCoordinate(-1.0d, -2.0d, -3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d), new GeodeticCoordinate(1.0d, 2.0d, 3.0d)],
        ];

        #region Constructors
        [Test]
        public void ValueConstructor_PopulatesFieldsCorrectly()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(a.Latitude, Is.EqualTo(1.0d));
                Assert.That(a.Longitude, Is.EqualTo(2.0d));
                Assert.That(a.Altitude, Is.EqualTo(3.0d));
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
            GeodeticCoordinate a = new([1.0d, 2.0d, 3.0d]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(a.Latitude, Is.EqualTo(1.0d));
                Assert.That(a.Longitude, Is.EqualTo(2.0d));
                Assert.That(a.Altitude, Is.EqualTo(3.0d));
            }
        }

        [Test]
        public void SpanConstructor_WithMoreThanThreeValues_PopulatesFieldsCorrectly()
        {
            GeodeticCoordinate a = new([1.0d, 2.0d, 3.0d, 4.0d, 5.0d]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(a.Latitude, Is.EqualTo(1.0d));
                Assert.That(a.Longitude, Is.EqualTo(2.0d));
                Assert.That(a.Altitude, Is.EqualTo(3.0d));
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
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate b = new(4.0d, 5.0d, 6.0d);

            GeodeticCoordinate result = a + b;

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
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);

            Assert.That(a == a, Is.True);
        }

        [Test]
        public void op_Equality_WithEqualValues_ReturnsTrue()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate b = new(1.0d, 2.0d, 3.0d);

            Assert.That(a == b, Is.True);
        }

        [TestCaseSource(nameof(UnequalCases))]
        public void op_Equality_WithUnequalValues_ReturnsFalse(GeodeticCoordinate a, GeodeticCoordinate b)
        {
            Assert.That(a == b, Is.False);
        }

        [Test]
        public void op_Inequality_WithSameValues_ReturnsFalse()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);

            Assert.That(a != a, Is.False);
        }

        [Test]
        public void op_Inequality_WithEqualValues_ReturnsFalse()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate b = new(1.0d, 2.0d, 3.0d);

            Assert.That(a != b, Is.False);
        }

        [TestCaseSource(nameof(UnequalCases))]
        public void op_Inequality_WithUnequalValues_ReturnsTrue(GeodeticCoordinate a, GeodeticCoordinate b)
        {
            Assert.That(a != b, Is.True);
        }

        [Test]
        public void op_Subtraction_ReturnsExpectedResult()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate b = new(4.0d, 5.0d, 6.0d);

            GeodeticCoordinate result = a - b;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(a.Latitude - b.Latitude));
                Assert.That(result.Longitude, Is.EqualTo(a.Longitude - b.Longitude));
                Assert.That(result.Altitude, Is.EqualTo(a.Altitude - b.Altitude));
            }
        }
        #endregion

        #region Static Methods
        [Test]
        public void Add_ReturnsExpectedResult()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);
            GeodeticCoordinate b = new(4.0d, 5.0d, 6.0d);

            GeodeticCoordinate result = GeodeticCoordinate.Add(a, b);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Latitude, Is.EqualTo(a.Latitude + b.Latitude));
                Assert.That(result.Longitude, Is.EqualTo(a.Longitude + b.Longitude));
                Assert.That(result.Altitude, Is.EqualTo(a.Altitude + b.Altitude));
            }
        }

        [Test]
        public void All_WithAllMatchingValues_ReturnsTrue()
        {
            GeodeticCoordinate a = new(4.0d, 4.0d, 4.0d);

            Assert.That(GeodeticCoordinate.All(a, 4.0d), Is.True);
        }

        [TestCaseSource(nameof(All_WithNotAllMatchingCases))]
        public void All_WithNotAllMatchingValues_ReturnsFalse(GeodeticCoordinate a, double v)
        {
            Assert.That(GeodeticCoordinate.All(a, v), Is.False);
        }

        [TestCaseSource(nameof(Any_WithAtLeastOneMatchingCases))]
        public void Any_WithAtLeastOneMatchingValue_ReturnsTrue(GeodeticCoordinate a, double v)
        {
            Assert.That(GeodeticCoordinate.Any(a, v), Is.True);
        }

        [Test]
        public void Any_WithNoMatchingValues_ReturnsFalse()
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);

            Assert.That(GeodeticCoordinate.All(a, 4.0d), Is.False);
        }

        [TestCaseSource(nameof(Clamp_WithMinimumGreaterThanMaximumCases))]
        public void Clamp_WithMinimumGreaterThanMaximum_ThrowsArgumentException(GeodeticCoordinate min, GeodeticCoordinate max)
        {
            GeodeticCoordinate a = new(1.0d, 2.0d, 3.0d);

            Assert.That(() => GeodeticCoordinate.Clamp(a, min, max), Throws.ArgumentException);
        }

        [TestCaseSource(nameof(ClampCases))]
        public void Clamp_ReturnsExpectedResult(
            GeodeticCoordinate a, GeodeticCoordinate min, GeodeticCoordinate max, GeodeticCoordinate expectedResult)
        {
            Assert.That(GeodeticCoordinate.Clamp(a, min, max), Is.EqualTo(expectedResult));
        }

        // TODO NaN and NegativeZero
        #endregion
    }
}
