using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class VectorLLAd_StaticProperties {

		[TestMethod]
		public void ZeroProperty_ContainsExpectedValues() {
			Assert.IsTrue(
				VectorLLAd.Zero.Latitude == 0.0d &&
				VectorLLAd.Zero.Longitude == 0.0d &&
				VectorLLAd.Zero.Altitude == 0.0d
			);
		}
	}
}
