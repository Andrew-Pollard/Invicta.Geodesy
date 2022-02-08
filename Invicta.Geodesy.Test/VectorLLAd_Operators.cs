using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class VectorLLAd_Operators {

		[TestMethod]
		public void AdditionOperator_ProducesExpectedValue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(4.0d, 5.0d, 6.0d);

			Assert.IsTrue(
				(A + B).Latitude == 1.0d + 4.0d &&
				(A + B).Longitude == 2.0d + 5.0d &&
				(A + B).Altitude == 3.0d + 6.0d
			);
		}


		[TestMethod]
		public void SubtractionOperator_ProducesExpectedValue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(4.0d, 5.0d, 6.0d);

			Assert.IsTrue(
				(A - B).Latitude == 1.0d - 4.0d &&
				(A - B).Longitude == 2.0d - 5.0d &&
				(A - B).Altitude == 3.0d - 6.0d
			);
		}


		[TestMethod]
		public void EqualityOperatorWithIdenticalVectors_ReturnsTrue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsTrue(A == B);
		}

		[TestMethod]
		public void EqualityOperatorWithDifferentVectors_ReturnsFalse() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(4.0d, 5.0d, 6.0d);

			Assert.IsFalse(A == B);
		}


		[TestMethod]
		public void InequalityOperatorWithIdenticalVectors_ReturnsFalse() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsFalse(A != B);
		}

		[TestMethod]
		public void InequalityOperatorWithDifferentVectors_ReturnsTrue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(4.0d, 5.0d, 6.0d);

			Assert.IsTrue(A != B);
		}
	}
}
