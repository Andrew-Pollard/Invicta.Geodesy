using System;

using Invicta.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class WGS84_LLAToECEF {

		[TestMethod]
		public void Latitude_GreaterThan90_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(Math.PI / 2.0 + 0.0001, 0.0d, 0.0d));
		}

		[TestMethod]
		public void Latitude_LessThanMinus90_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(-Math.PI / 2.0 - 0.0001, 0.0d, 0.0d));
		}

		[TestMethod]
		public void Latitude_NaN_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(double.NaN, 0.0d, 0.0d));
		}


		[TestMethod]
		public void Longitude_GreaterThan180_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(0.0d, Math.PI + 0.0001, 0.0d));
		}

		[TestMethod]
		public void Longitude_LessThanMinus180_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(0.0d, -Math.PI - 0.0001, 0.0d));
		}

		[TestMethod]
		public void Longitude_NaN_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(0.0d, double.NaN, 0.0d));
		}


		[TestMethod]
		public void Altitude_NaN_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.LLAToECEF(0.0d, 0.0d, double.NaN));
		}


		[TestMethod]
		public void LowAltitude_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, 0.0d, -10000.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(6368137.0d, 0.0d, 0.0d), 0.01));
		}

		[TestMethod]
		public void HighAltitude_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, 0.0d, 10000.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(6388137.0d, 0.0d, 0.0d), 0.01));
		}


		[TestMethod]
		public void Test_90_0_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, 0.0d, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, 3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_Minus180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, -Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_Minus135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, -3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_Minus90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, -Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}

		[TestMethod]
		public void Test_90_Minus45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 2.0, -Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, 6356752.31d), 0.01));
		}


		[TestMethod]
		public void Test_45_0_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, 0.0d, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(4517590.88d, 0.0d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(3194419.15d, 3194419.15d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 4517590.88d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, 3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-3194419.15d, 3194419.15d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-4517590.88d, 0.0d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_Minus180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, -Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-4517590.88d, 0.0d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_Minus135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, -3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-3194419.15d, -3194419.15d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_Minus90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, -Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, -4517590.88d, 4487348.41d), 0.01));
		}

		[TestMethod]
		public void Test_45_Minus45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(Math.PI / 4.0, -Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(3194419.15d, -3194419.15d, 4487348.41d), 0.01));
		}


		[TestMethod]
		public void Test_0_0_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, 0.0d, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(6378137.0d, 0.0d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(4510023.92d, 4510023.92d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 6378137.0d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, 3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-4510023.92d, 4510023.92d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-6378137.0d, 0.0d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_Minus180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, -Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-6378137.0d, 0.0d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_Minus135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, -3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-4510023.92d, -4510023.92d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_Minus90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, -Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, -6378137.0d, 0.0d), 0.01));
		}

		[TestMethod]
		public void Test_0_Minus45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(0.0d, -Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(4510023.92d, -4510023.92d, 0.0d), 0.01));
		}


		[TestMethod]
		public void Minus45_0_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, 0.0d, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(4517590.88d, 0.0d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(3194419.15d, 3194419.15d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 4517590.88d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, 3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-3194419.15d, 3194419.15d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-4517590.88d, 0.0d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_Minus180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, -Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-4517590.88d, -0.0d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_Minus135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, -3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(-3194419.15d, -3194419.15d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_Minus90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, -Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, -4517590.88d, -4487348.41d), 0.01));
		}

		[TestMethod]
		public void Minus45_Minus45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 4.0, -Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(3194419.15d, -3194419.15d, -4487348.41d), 0.01));
		}


		[TestMethod]
		public void Minus90_0_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, 0.0d, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, 3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_Minus180_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, -Math.PI, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_Minus135_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, -3.0 * Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_Minus90_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, -Math.PI / 2.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}

		[TestMethod]
		public void Minus90_Minus45_0_ProducesExpectedResult() {
			var ECEF = WGS84.LLAToECEF(-Math.PI / 2.0, -Math.PI / 4.0, 0.0d);

			Assert.IsTrue(ECEF.ApproximatelyEquals(new Vector3d(0.0d, 0.0d, -6356752.31d), 0.01));
		}
	}
}
