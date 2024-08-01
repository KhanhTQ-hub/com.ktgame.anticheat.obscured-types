namespace com.ktgame.anticheat.obscured_types
{
	/// <summary>
	/// Base interface for all obscured types.
	/// </summary>
	public interface IObscuredType
	{
		/// <summary>
		/// Allows to change current crypto key to the new random value and re-encrypt variable using it.
		/// Use it for extra protection against 'unknown value' search.
		/// Just call it sometimes when your variable doesn't change to fool the cheater.
		/// </summary>
		void RandomizeCryptoKey();
	}
}