using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class VectorLLAd_CopyTo {

		[TestMethod]
		public void CopyToWithNullArray_ThrowsNullReferenceException() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.ThrowsException<NullReferenceException>(() => A.CopyTo(null));
		}

		[TestMethod]
		public void CopyToWithNegativeIndex_ThrowsArgumentOutOfRangeException() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			double[] Array = new double[3];

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => A.CopyTo(Array, -1));
		}

		[TestMethod]
		public void CopyToWithOutOfBoundsIndex_ThrowsArgumentOutOfRangeException() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			double[] Array = new double[3];

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => A.CopyTo(Array, 4));
		}

		[TestMethod]
		public void CopyToWithTooSmallArray_ThrowsArgumentOutOfRangeException() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			double[] Array = new double[0];

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => A.CopyTo(Array));
		}

		[TestMethod]
		public void CopyToWithGoodParameters_CopiesAsExpected() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			double[] Array = new double[4];

			A.CopyTo(Array, 1);

			Assert.IsTrue(
				Array[1] == A.Latitude &&
				Array[2] == A.Longitude &&
				Array[3] == A.Altitude
			);
		}
	}
}
