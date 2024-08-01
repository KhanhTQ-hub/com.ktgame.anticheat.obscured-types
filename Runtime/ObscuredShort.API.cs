﻿using System;

namespace com.ktgame.anticheat.obscured_types
{
	public partial struct ObscuredShort : IFormattable, IEquatable<ObscuredShort>, IEquatable<short>, IComparable<ObscuredShort>, IComparable<short>,
		IComparable
	{
		[System.Reflection.Obfuscation(Exclude = true)]
		public static implicit operator ObscuredShort(short value)
		{
			return new ObscuredShort(value);
		}

		[System.Reflection.Obfuscation(Exclude = true)]
		public static implicit operator short(ObscuredShort value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredShort operator ++(ObscuredShort input)
		{
			return Increment(input, 1);
		}

		public static ObscuredShort operator --(ObscuredShort input)
		{
			return Increment(input, -1);
		}

		private static ObscuredShort Increment(ObscuredShort input, int increment)
		{
			var decrypted = (short)(input.InternalDecrypt() + increment);
			input.hiddenValue = Encrypt(decrypted, input.currentCryptoKey);

			// if (ObscuredCheatingDetector.ExistsAndIsRunning)
			// {
			// 	input.fakeValue = decrypted;
			// 	input.fakeValueActive = true;
			// }
			// else
			// {
			input.fakeValueActive = false;
			// }

			return input;
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InternalDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InternalDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InternalDecrypt().ToString(format, provider);
		}

		public override bool Equals(object other)
		{
			return other is ObscuredShort o && Equals(o) ||
				   other is short r && Equals(r);
		}

		public bool Equals(ObscuredShort obj)
		{
			return currentCryptoKey == obj.currentCryptoKey ? hiddenValue.Equals(obj.hiddenValue) : InternalDecrypt().Equals(obj.InternalDecrypt());
		}

		public bool Equals(short other)
		{
			return InternalDecrypt().Equals(other);
		}

		public int CompareTo(ObscuredShort other)
		{
			return InternalDecrypt().CompareTo(other.InternalDecrypt());
		}

		public int CompareTo(short other)
		{
			return InternalDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			return InternalDecrypt().CompareTo(obj);
		}
	}
}
