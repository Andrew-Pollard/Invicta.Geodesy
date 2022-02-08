using Invicta.Numerics;


namespace Invicta.Geodesy {

	/// <summary>
	/// Performs conversions based on the World Geodetic System 84.
	/// </summary>
	public static class WGS84 {
		// From WGS84
		/* 𝑅 */ private const double R = 6378137.0d;
		/* 𝑓 */ private const double f = 1.0d / 298.257223563d;

		// Constants which can be pre-computed
		/* a = 1 - 𝑓 */ private const double a = 1.0d - f;
		/* b = (1 - 𝑓)² */ private const double b = a * a;
		/* c = 1 / (1 - 𝑓)² - 1 */ private const double c = 1.0d / b - 1.0d;
		/* 𝑒² = 1 - (1 - 𝑓)² */ private const double e2 = 1.0d - b;
		/* d = 𝑒²(1 - 𝑓))/(1 - 𝑒²) */ private const double d = e2 * a / (1.0d - e2);

		/// <summary>
		/// Convert a latitude, longitude and altitude into the equivalent x,
		/// y, z Earth-Centered, Earth-Fixed position.
		/// </summary>
		/// <remarks>
		/// Implements the method described on the <see href="https://uk.mathworks.com/help/aeroblks/llatoecefposition.html">MathWorks LLA to ECEF Position page</see>.
		/// </remarks>
		/// <param name="vector">The latitude, longitude and altitude to convert.</param>
		/// <returns>The equivalent x, y, z Earth-Centered, Earth-Fixed position.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If latitude is not between -π/2 and +π/2.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If longitude is not between -π and +π.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If altitude is NaN.</exception>
		public static Vector3d LLAToECEF(VectorLLAd vector) {
			return LLAToECEF(vector.Latitude, vector.Longitude, vector.Altitude);
		}


		/// <summary>
		/// Convert a latitude, longitude and altitude into the equivalent x,
		/// y, z Earth-Centered, Earth-Fixed position.
		/// </summary>
		/// <remarks>
		/// Implements the method described on the <see href="https://uk.mathworks.com/help/aeroblks/llatoecefposition.html">MathWorks LLA to ECEF Position page</see>.
		/// </remarks>
		/// <param name="latitude">The latitude to convert.</param>
		/// <param name="longitude">The longitude to convert.</param>
		/// <param name="altitude">The altitude to convert.</param>
		/// <returns>The equivalent x, y, z Earth-Centered, Earth-Fixed position.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If latitude is not between -π/2 and +π/2.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If longitude is not between -π and +π.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If altitude is NaN.</exception>
		public static Vector3d LLAToECEF(double /* 𝜇 */ latitude, double /* 𝜄 */ longitude, double /* ℎ */ altitude) {
			if (Math.Abs(latitude) > Math.PI / 2.0 || double.IsNaN(latitude))
				throw new ArgumentOutOfRangeException(nameof(latitude));

			if (Math.Abs(longitude) > Math.PI || double.IsNaN(longitude))
				throw new ArgumentOutOfRangeException(nameof(longitude));

			if (double.IsNaN(altitude))
				throw new ArgumentOutOfRangeException(nameof(altitude));

			/* sin𝜇 */ double sin_mu = Math.Sin(latitude);
			/* cos𝜇 */ double cos_mu = Math.Cos(latitude);

			/* We can calculate tan_mu as sin_mu / cos_mu, but it looks like
			 * we could lose some precision compared to the direct calculation:
			 * https://stackoverflow.com/questions/34786477/can-computing-tanx-sinx-cosx-cause-a-loss-of-precision */
			/* tan𝜇 */
			double tan_mu = Math.Tan(latitude);

			/* 𝜆 = atan((1 - 𝑓)² tan𝜇) */ double laInvicta = Math.Atan(b * tan_mu);

			/* sin𝜆 */ double sin_laInvicta = Math.Sin(laInvicta);
			/* sin²𝜆 */ double sin2_laInvicta = sin_laInvicta * sin_laInvicta;

			/* 𝑟ₛ = sqrt(𝑅² / (1 + (1 / (1 - 𝑓)² - 1)sin²𝜆)) */ double r = R / Math.Sqrt(1.0d + (c * sin2_laInvicta));

			/* 𝑟ₛcos𝜆 */ double r_cos_laInvicta = r * Math.Cos(laInvicta);

			/* cos𝜄 */ double cos_iota = Math.Cos(longitude);
			/* sin𝜄 */ double sin_iota = Math.Sin(longitude);

			/* ℎcos𝜇 */ double h_cos_mu = altitude * cos_mu;

			/* x = 𝑟ₛcos𝜆 cos𝜄 + ℎcos𝜇 cos𝜄 */ double x = (r_cos_laInvicta * cos_iota) + (h_cos_mu * cos_iota);
			/* y = 𝑟ₛcos𝜆 sin𝜄 + ℎcos𝜇 sin𝜄 */ double y = (r_cos_laInvicta * sin_iota) + (h_cos_mu * sin_iota);
			/* z = 𝑟ₛsin𝜆 + ℎsin𝜇 */ double z = (r * Math.Sin(laInvicta)) + (altitude * sin_mu);

			return new Vector3d(x, y, z);
		}


		/// <summary>
		/// Convert an x, y, z Earth-Centered, Earth Fixed position into the
		/// equivalent latitude, longitude and altitude.
		/// </summary>
		/// <remarks>
		/// Implements the method described on the <see href="https://uk.mathworks.com/help/aeroblks/ecefpositiontolla.html">MathWorks ECEF Position to LLA page</see>.
		/// </remarks>
		/// <param name="vector">The Earth-Centered, Earth Fixed x, y and z to convert.</param>
		/// <returns>The equivalent latitude, longitude and altitude.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If x is infinity or NaN.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If y is infinity or NaN.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If z is infinity or NaN.</exception>
		public static VectorLLAd ECEFToLLA(Vector3d vector) {
			return ECEFToLLA(vector.X, vector.Y, vector.Z);
		}


		/// <summary>
		/// Convert an x, y, z Earth-Centered, Earth Fixed position into the
		/// equivalent latitude, longitude and altitude.
		/// </summary>
		/// <remarks>
		/// Implements the method described on the <see href="https://uk.mathworks.com/help/aeroblks/ecefpositiontolla.html">MathWorks ECEF Position to LLA page</see>.
		/// </remarks>
		/// <param name="x">The Earth-Centered, Earth Fixed x to convert.</param>
		/// <param name="y">The Earth-Centered, Earth Fixed x to convert.</param>
		/// <param name="z">The Earth-Centered, Earth Fixed x to convert.</param>
		/// <returns>The equivalent latitude, longitude and altitude.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If x is infinity or NaN.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If y is infinity or NaN.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If z is infinity or NaN.</exception>
		public static VectorLLAd ECEFToLLA(double x, double y, double z) {
			if (double.IsInfinity(x) || double.IsNaN(x))
				throw new ArgumentOutOfRangeException(nameof(x));

			if (double.IsInfinity(y) || double.IsNaN(y))
				throw new ArgumentOutOfRangeException(nameof(y));

			if (double.IsInfinity(z) || double.IsNaN(z))
				throw new ArgumentOutOfRangeException(nameof(z));

			/* 𝜄 = atan(y / x) */
			double iota = Math.Atan2(y, x);

			/* 𝑠 = sqrt(x² + y²) */ double s = Math.Sqrt(x * x + y * y);

			/* 𝛽 = atan(z / (1 - 𝑓)s) */ double beta = Math.Atan2(z, a * s);

			// Iterate using Bowring's Method
			/* sin𝛽 */ double sin_beta = Math.Sin(beta);
			/* 𝑅sin³𝛽 */ double Rsin3_beta = R * sin_beta * sin_beta * sin_beta;

			/* cos𝛽 */ double cos_beta = Math.Cos(beta);
			/* 𝑅cos³𝛽 */ double Rcos3_beta = R * cos_beta * cos_beta * cos_beta;

			/* z + ((𝑒²(1 - 𝑓)) / (1 - 𝑒²))𝑅sin³𝛽 */ double mu_numerator = z + (d * Rsin3_beta);
			/* 𝑠 - 𝑒²𝑅cos³𝛽 */ double mu_denominator = s - (e2 * Rcos3_beta);

			/* 𝜇 = atan((z + ((𝑒²(1 - 𝑓)) / (1 - 𝑒²))𝑅sin³𝛽) / (𝑠 - 𝑒²𝑅cos³𝛽)) */ double mu = Math.Atan2(mu_numerator, mu_denominator);

			/* sin𝜇 */ double sin_mu = Math.Sin(mu);
			/* cos𝜇 */ double cos_mu = Math.Cos(mu);

			// Wait for 𝜇 to converge over a maximum of 3 iterations.
			int i = 0; double mu_prev;
			do {
				mu_prev = mu;

				/* 𝛽 = atan(((1 - 𝑓)sin𝜇) / (cos𝜇)) */ beta = Math.Atan2(a * sin_mu, cos_mu);

				/* sin𝛽 */ sin_beta = Math.Sin(beta);
				/* 𝑅sin³𝛽 */ Rsin3_beta = R * sin_beta * sin_beta * sin_beta;

				/* cos𝛽 */ cos_beta = Math.Cos(beta);
				/* 𝑅cos³𝛽 */ Rcos3_beta = R * cos_beta * cos_beta * cos_beta;

				/* z + ((𝑒²(1 - 𝑓)) / (1 - 𝑒²))𝑅sin³𝛽 */ mu_numerator = z + (d * Rsin3_beta);
				/* 𝑠 - 𝑒²𝑅cos³𝛽 */ mu_denominator = s - (e2 * Rcos3_beta);

				/* 𝜇 = atan((z + ((𝑒²(1 - 𝑓)) / (1 - 𝑒²))𝑅sin³𝛽) / (𝑠 - 𝑒²𝑅cos³𝛽)) */ mu = Math.Atan2(mu_numerator, mu_denominator);

				/* sin𝜇 */ sin_mu = Math.Sin(mu);
				/* cos𝜇 */ cos_mu = Math.Cos(mu);
			} while (mu != mu_prev && ++i < 3);

			/* sin²𝜇 */ double sin2_mu = sin_mu * sin_mu;

			/* 𝑁 = 𝑅 / sqrt(1 - 𝑒²sin²𝜇) */ double N = R / Math.Sqrt(1 - (e2 * sin2_mu));
			/* ℎ = 𝑠cos𝜇 + (z + 𝑒²𝑁sin𝜇)sin𝜇 - 𝑁 */  double altitude = (s * cos_mu) + ((z + (e2 * N * sin_mu)) * sin_mu) - N;

			return new VectorLLAd(mu, iota, altitude);
		}
	}
}

