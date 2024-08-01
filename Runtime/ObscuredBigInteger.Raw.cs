﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.ktgame.anticheat.obscured_types
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct SerializableBigInteger : IEquatable<SerializableBigInteger>
	{
		[FieldOffset(0)] public BigInteger value;

		[FieldOffset(0), SerializeField] public BigIntegerContents raw;

		[Serializable]
		public struct BigIntegerContents
		{
			public int sign;
			public uint[] bits;
		}

		public SerializableBigInteger Encrypt(uint key)
		{
			return SymmetricShuffle(key);
		}

		public BigInteger Decrypt(uint key)
		{
			BigInteger result;
			SymmetricShuffle(key);
			result = new BigInteger(value.ToByteArray());
			SymmetricShuffle(key);
			return result;
		}

		private SerializableBigInteger SymmetricShuffle(uint key)
		{
			raw.sign ^= (int)key;
			if (raw.bits == null)
				return this;

			var count = raw.bits.Length;
			if (count == 0)
			{
				raw.bits = null;
				return this;
			}

			if (count == 1)
				raw.bits[0] ^= key;
			else
				(raw.bits[0], raw.bits[count - 1]) = (raw.bits[count - 1] ^ key, raw.bits[0] ^ key);

			return this;
		}

		[System.Reflection.Obfuscation(Exclude = true)]
		public static implicit operator BigInteger(SerializableBigInteger value)
		{
			if (value.raw.bits != null && value.raw.bits.Length == 0)
				value.raw.bits = null;

			return value.value;
		}

		public bool Equals(SerializableBigInteger other)
		{
			return value.Equals(other.value);
		}

		public override bool Equals(object obj)
		{
			return obj is SerializableBigInteger other && Equals(other);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
