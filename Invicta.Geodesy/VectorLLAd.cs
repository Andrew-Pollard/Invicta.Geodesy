namespace Invicta.Geodesy {

	/// <summary>
	/// A structure encapsulating a latitude, longitude and altitude value.
	/// </summary>
	public struct VectorLLAd : IEquatable<VectorLLAd> {
		/// <summary>
		/// Returns the vector (0,0,0).
		/// </summary>
		public static VectorLLAd Zero => new();

		/// <summary>
		/// The latitude component of the vector.
		/// </summary>
		public double Latitude;

		/// <summary>
		/// The longitude component of the vector.
		/// </summary>
		public double Longitude;

		/// <summary>
		/// The altitude component of the vector.
		/// </summary>
		public double Altitude;

		/// <summary>
		/// Constructs a vector with the given latitude and longitude elements and zero altitude.
		/// </summary>
		/// <param name="latitude">The latitude component, in radians.</param>
		/// <param name="longitude">The longitude component, in radians.</param>
		public VectorLLAd(double latitude, double longitude) : this(latitude, longitude, 0.0d) { }

		/// <summary>
		/// Constructs a vector with the given individual elements.
		/// </summary>
		/// <param name="latitude">The latitude component, in radians.</param>
		/// <param name="longitude">The longitude component, in radians.</param>
		/// <param name="altitude">The altitude component, in radians.</param>
		public VectorLLAd(double latitude, double longitude, double altitude) {
			Latitude = latitude;
			Longitude = longitude;
			Altitude = altitude;
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
		public static VectorLLAd Add(VectorLLAd left, VectorLLAd right) {
			return left + right;
		}

		/// <summary>
		/// Subtracts the second vector from the first.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
		public static VectorLLAd Subtract(VectorLLAd left, VectorLLAd right) {
			return left - right;
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
		public static VectorLLAd operator +(VectorLLAd left, VectorLLAd right) {
			return new VectorLLAd(left.Latitude + right.Latitude, left.Longitude + right.Longitude, left.Altitude + right.Altitude);
		}

		/// <summary>
		/// Subtracts the second vector from the first.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
		public static VectorLLAd operator -(VectorLLAd left, VectorLLAd right) {
			return new VectorLLAd(left.Latitude - right.Latitude, left.Longitude - right.Longitude, left.Altitude - right.Altitude);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are equal; False otherwise.</returns>
		public static bool operator ==(VectorLLAd left, VectorLLAd right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are not equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are not equal; False if they are equal.</returns>
		public static bool operator !=(VectorLLAd left, VectorLLAd right) {
			return !(left == right);
		}

		/// <summary>
		/// Copies the contents of the vector into the given array.
		/// </summary>
		public void CopyTo(double[] array) {
			CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the contents of the vector into the given array, starting from index.
		/// </summary>
		/// <exception cref="ArgumentNullException">If array is null.</exception>
		/// <exception cref="RankException">If array is multidimensional.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If index is greater than end of the array or index is less than zero.</exception>
		/// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination array.</exception>
		public void CopyTo(double[] array, int index) {
			if (array == null)
				throw new NullReferenceException(nameof(array));

			if (index < 0 || index >= array.Length)
				throw new ArgumentOutOfRangeException(nameof(index));

			if ((array.Length - index) < 3)
				throw new ArgumentException(nameof(array));

			array[index] = Latitude;
			array[index + 1] = Longitude;
			array[index + 2] = Altitude;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this VectorLLAd instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this VectorLLAd; False otherwise.</returns>
		public override bool Equals(object? obj) {
			return obj is VectorLLAd d && Equals(d);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given VectorLLAd is equal to this VectorLLAd instance.
		/// </summary>
		/// <param name="other">The VectorLLAd to compare this instance to.</param>
		/// <returns>True if the other VectorLLAd is equal to this instance; False otherwise.</returns>
		public bool Equals(VectorLLAd other) {
			return Latitude == other.Latitude &&
				   Longitude == other.Longitude &&
				   Altitude == other.Altitude;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() {
			return HashCode.Combine(Latitude, Longitude, Altitude);
		}

		/// <summary>
		/// Returns a String representing this VectorLLAd instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString() {
			return $"<{Latitude}la, {Longitude}lo, {Altitude}a>";
		}
	}
}