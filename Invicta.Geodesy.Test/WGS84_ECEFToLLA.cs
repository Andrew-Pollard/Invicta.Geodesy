using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class WGS84_ECEFToLLA {
		[TestMethod]
		public void X_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(double.PositiveInfinity, 0.0d, 0.0d));
		}

		[TestMethod]
		public void X_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(double.NegativeInfinity, 0.0d, 0.0d));
		}

		[TestMethod]
		public void X_NaN_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(double.NaN, 0.0d, 0.0d));
		}


		[TestMethod]
		public void Y_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(0.0d, double.PositiveInfinity, 0.0d));
		}

		[TestMethod]
		public void Y_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(0.0d, double.NegativeInfinity, 0.0d));
		}

		[TestMethod]
		public void Y_NaN_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(0.0d, double.NaN, 0.0d));
		}


		[TestMethod]
		public void Z_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(0.0d, 0.0d, double.PositiveInfinity));
		}

		[TestMethod]
		public void Z_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(0.0d, 0.0d, double.NegativeInfinity));
		}

		[TestMethod]
		public void Z_NaN_ThrowsArgumentOutOfRangeException() {
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = WGS84.ECEFToLLA(0.0d, 0.0d, double.NaN));
		}


		private static bool LLAApproximatelyEquals(VectorLLAd a, VectorLLAd b) {
			return Math.Abs(a.Latitude - b.Latitude) < 1e-7 &&
				Math.Abs(a.Longitude - b.Longitude) < 1e-7 &&
				Math.Abs(a.Altitude - b.Altitude) < 1e-2;
		}


		[TestMethod]
		public void LowAltitude_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(6368137.0d, 0.0d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, 0.0d, -10000.0d)));
		}

		[TestMethod]
		public void HighAltitude_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(6388137.0d, 0.0d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, 0.0d, 10000.0d)));
		}


		[TestMethod]
		public void Test_90_0_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, 0.0d, 6356752.31d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 2.0, 0.0d, 0.0d)));
		}


		[TestMethod]
		public void Test_45_0_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(4517590.88d, 0.0d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, 0.0d, 0.0d)));
		}

		[TestMethod]
		public void Test_45_45_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(3194419.15d, 3194419.15d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Test_45_90_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, 4517590.88d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, Math.PI / 2.0, 0.0d)));
		}

		[TestMethod]
		public void Test_45_135_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-3194419.15d, 3194419.15d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, 3.0 * Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Test_45_180_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-4517590.88d, 0.0d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, Math.PI, 0.0d)));
		}

		[TestMethod]
		public void Test_45_Minus135_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-3194419.15d, -3194419.15d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, -3.0 * Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Test_45_Minus90_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, -4517590.88d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, -Math.PI / 2.0, 0.0d)));
		}

		[TestMethod]
		public void Test_45_Minus45_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(3194419.15d, -3194419.15d, 4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(Math.PI / 4.0, -Math.PI / 4.0, 0.0d)));
		}


		[TestMethod]
		public void Test_0_0_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(6378137.0d, 0.0d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, 0.0d, 0.0d)));
		}

		[TestMethod]
		public void Test_0_45_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(4510023.92d, 4510023.92d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Test_0_90_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, 6378137.0d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, Math.PI / 2.0, 0.0d)));
		}

		[TestMethod]
		public void Test_0_135_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-4510023.92d, 4510023.92d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, 3.0 * Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Test_0_180_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-6378137.0d, 0.0d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, Math.PI, 0.0d)));
		}

		[TestMethod]
		public void Test_0_Minus135_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-4510023.92d, -4510023.92d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, -3.0 * Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Test_0_Minus90_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, -6378137.0d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, -Math.PI / 2.0, 0.0d)));
		}

		[TestMethod]
		public void Test_0_Minus45_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(4510023.92d, -4510023.92d, 0.0d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(0.0d, -Math.PI / 4.0, 0.0d)));
		}


		[TestMethod]
		public void Minus45_0_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(4517590.88d, 0.0d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, 0.0d, 0.0d)));
		}

		[TestMethod]
		public void Minus45_45_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(3194419.15d, 3194419.15d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Minus45_90_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, 4517590.88d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, Math.PI / 2.0, 0.0d)));
		}

		[TestMethod]
		public void Minus45_135_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-3194419.15d, 3194419.15d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, 3.0 * Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Minus45_180_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-4517590.88d, 0.0d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, Math.PI, 0.0d)));
		}

		[TestMethod]
		public void Minus45_Minus135_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(-3194419.15d, -3194419.15d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, -3.0 * Math.PI / 4.0, 0.0d)));
		}

		[TestMethod]
		public void Minus45_Minus90_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, -4517590.88d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, -Math.PI / 2.0, 0.0d)));
		}

		[TestMethod]
		public void Minus45_Minus45_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(3194419.15d, -3194419.15d, -4487348.41d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 4.0, -Math.PI / 4.0, 0.0d)));
		}


		[TestMethod]
		public void Minus90_0_0_ProducesExpectedResult() {
			var LLA = WGS84.ECEFToLLA(0.0d, 0.0d, -6356752.31d);

			Assert.IsTrue(LLAApproximatelyEquals(LLA, new VectorLLAd(-Math.PI / 2.0, 0.0d, 0.0d)));
		}
	}
}
