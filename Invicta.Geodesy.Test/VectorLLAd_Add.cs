using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Invicta.Geodesy.Test {

	[TestClass]
	public class VectorLLAd_Add {

		[TestMethod]
		public void Add_ProducesExpectedValue() {
			var A = new VectorLLAd(1.0d, 2.0d, 3.0d);
			var B = new VectorLLAd(4.0d, 5.0d, 6.0d);

			var C = VectorLLAd.Add(A, B);

			Assert.IsTrue(
				C.Latitude == 1.0d + 4.0d &&
				C.Longitude == 2.0d + 5.0d &&
				C.Altitude == 3.0d + 6.0d
			);
		}
	}
}
