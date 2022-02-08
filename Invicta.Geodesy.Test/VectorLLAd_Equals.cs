using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class VectorLLAd_Equals {

		[TestMethod]
		public void IEquatableEqualsWithSameObject_ReturnsTrue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsTrue(A.Equals(A));
		}

		[TestMethod]
		public void IEquatableEqualsWithIdenticalVectors_ReturnsTrue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsTrue(A.Equals(B));
		}

		[TestMethod]
		public void IEquatableEqualsWithDifferentVectors_ReturnsFalse() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(4.0d, 5.0d, 6.0d);

			Assert.IsFalse(A.Equals(B));
		}


		[TestMethod]
		public void EqualsWithSameObject_ReturnsTrue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsTrue(A.Equals((object) A));
		}

		[TestMethod]
		public void EqualsWithIdenticalVectors_ReturnsTrue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = (object) new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsTrue(A.Equals(B));
		}

		[TestMethod]
		public void EqualsWithDifferentVectors_ReturnsFalse() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = (object) new VectorLLAd(4.0d, 5.0d, 6.0d);

			Assert.IsFalse(A.Equals(B));
		}
	}
}
