﻿using System;
using UnityEngine;

namespace com.ktgame.anticheat.obscured_types
{
	public partial struct ObscuredVector3Int : IEquatable<ObscuredVector3Int>, IEquatable<Vector3Int>
	{
		[System.Reflection.Obfuscation(Exclude = true)]
		public static implicit operator ObscuredVector3Int(Vector3Int value)
		{
			return new ObscuredVector3Int(value);
		}

		[System.Reflection.Obfuscation(Exclude = true)]
		public static implicit operator Vector3Int(ObscuredVector3Int value)
		{
			return value.InternalDecrypt();
		}

		[System.Reflection.Obfuscation(Exclude = true)]
		public static implicit operator Vector3(ObscuredVector3Int value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredVector3Int operator +(ObscuredVector3Int a, ObscuredVector3Int b)
		{
			return a.InternalDecrypt() + b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator +(Vector3Int a, ObscuredVector3Int b)
		{
			return a + b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator +(ObscuredVector3Int a, Vector3Int b)
		{
			return a.InternalDecrypt() + b;
		}

		public static ObscuredVector3Int operator -(ObscuredVector3Int a, ObscuredVector3Int b)
		{
			return a.InternalDecrypt() - b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator -(Vector3Int a, ObscuredVector3Int b)
		{
			return a - b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator -(ObscuredVector3Int a, Vector3Int b)
		{
			return a.InternalDecrypt() - b;
		}

		public static ObscuredVector3Int operator *(ObscuredVector3Int a, int d)
		{
			return a.InternalDecrypt() * d;
		}

		public static bool operator ==(ObscuredVector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs.InternalDecrypt() == rhs.InternalDecrypt();
		}

		public static bool operator ==(Vector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs == rhs.InternalDecrypt();
		}

		public static bool operator ==(ObscuredVector3Int lhs, Vector3Int rhs)
		{
			return lhs.InternalDecrypt() == rhs;
		}

		public static bool operator !=(ObscuredVector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs.InternalDecrypt() != rhs.InternalDecrypt();
		}

		public static bool operator !=(Vector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs != rhs.InternalDecrypt();
		}

		public static bool operator !=(ObscuredVector3Int lhs, Vector3Int rhs)
		{
			return lhs.InternalDecrypt() != rhs;
		}

		public override bool Equals(object other)
		{
			return other is ObscuredVector3Int o && Equals(o) ||
				   other is Vector3Int r && Equals(r);
		}

		public bool Equals(ObscuredVector3Int other)
		{
			return currentCryptoKey == other.currentCryptoKey ? hiddenValue.Equals(other.hiddenValue) : InternalDecrypt().Equals(other.InternalDecrypt());
		}

		public bool Equals(Vector3Int other)
		{
			return InternalDecrypt().Equals(other);
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
	}
}
