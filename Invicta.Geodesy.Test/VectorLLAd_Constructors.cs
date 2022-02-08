using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class VectorLLAd_Constructors {

		[TestMethod]
		public void DoubleValueConstructor_CreatesExpectedValues() {
			var Vector = new VectorLLAd(1.0d, 2.0d);

			Assert.IsTrue(
				Vector.Latitude == 1.0d &&
				Vector.Longitude == 2.0d &&
				Vector.Altitude == 0.0d
			);
		}

		[TestMethod]
		public void TripleValueConstructor_CreatesExpectedValues() {
			var Vector = new VectorLLAd(1.0d, 2.0d, 3.0d);

			Assert.IsTrue(
				Vector.Latitude == 1.0d &&
				Vector.Longitude == 2.0d &&
				Vector.Altitude == 3.0d
			);
		}
	}
}
