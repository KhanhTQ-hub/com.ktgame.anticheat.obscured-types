﻿using System;

namespace com.ktgame.anticheat.obscured_types
{
	public partial struct ObscuredVector2Int
	{
		/// <summary>
		/// Used to store encrypted Vector2.
		/// </summary>
		[Serializable]
		public struct RawEncryptedVector2Int : IEquatable<RawEncryptedVector2Int>
		{
			/// <summary>
			/// Encrypted value
			/// </summary>
			public int x;

			/// <summary>
			/// Encrypted value
			/// </summary>
			public int y;

			public bool Equals(RawEncryptedVector2Int other)
			{
				return x == other.x && y == other.y;
			}

			public override bool Equals(object obj)
			{
				return obj is RawEncryptedVector2Int other && Equals(other);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (x * 397) ^ y;
				}
			}
		}
	}
}
