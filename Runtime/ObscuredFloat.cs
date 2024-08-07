﻿namespace com.ktgame.anticheat.obscured_types
{
	using System;
	using UnityEngine;
	using System.Runtime.InteropServices;
	using UnityEngine.Serialization;

	/// <summary>
	/// Use it instead of regular <c>float</c> for any cheating-sensitive variables.
	/// </summary>
	/// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong><br/>
	/// Feel free to use regular types for all short-term operations and calculations while keeping obscured type only at the long-term declaration (i.e. class field).
	[Serializable]
	public partial struct ObscuredFloat : IObscuredType
	{
#if UNITY_EDITOR
		public string migratedVersion;
#endif
		[SerializeField] internal int currentCryptoKey;
		[SerializeField] internal int hiddenValue;
		[SerializeField] [FormerlySerializedAs("hiddenValue")]
#pragma warning disable 414
		private ACTkByte4 hiddenValueOldByte4;
#pragma warning restore 414

		[SerializeField] internal float fakeValue;
		[SerializeField] internal bool fakeValueActive;
		[SerializeField] internal bool inited;

		private ObscuredFloat(float value)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(value, currentCryptoKey);
			hiddenValueOldByte4 = default;

#if UNITY_EDITOR
			fakeValue = value;
			fakeValueActive = true;
			migratedVersion = null;
#else
			var detectorRunning = ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = detectorRunning ? value : 0f;
			fakeValueActive = detectorRunning;
#endif
			inited = true;
		}

		/// <summary>
		/// Encrypts passed value using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static int Encrypt(float value, int key)
		{
			return FloatIntBytesUnion.XorFloatToInt(value, key);
		}

		/// <summary>
		/// Decrypts passed value you got from Encrypt() using same key.
		/// </summary>
		/// \sa Encrypt()
		public static float Decrypt(int value, int key)
		{
			return FloatIntBytesUnion.XorIntToFloat(value, key);
		}

		/// <summary>
		/// Allows to update the raw encrypted value to the newer encryption format.
		/// </summary>
		/// Use when you have some encrypted values saved somewhere with previous ACTk version
		/// and you wish to set them using SetEncrypted() to the newer ACTk version obscured type.
		/// Current migration variants:
		/// from 0 or 1 to 2 - migrate obscured type from ACTk 1.5.2.0-1.5.8.0 to the 1.5.9.0+ format
		/// <param name="encrypted">Encrypted value you got from previous ACTk version obscured type with GetEncrypted().</param>
		/// <param name="fromVersion">Source format version.</param>
		/// <param name="toVersion">Target format version.</param>
		/// <returns>Migrated raw encrypted value which you may use for SetEncrypted(0 later.</returns>
		public static int MigrateEncrypted(int encrypted, byte fromVersion = 0, byte toVersion = 2)
		{
			return FloatIntBytesUnion.Migrate(encrypted, fromVersion, toVersion);
		}

		/// <summary>
		/// Creates and fills obscured variable with raw encrypted value previously got from GetEncrypted().
		/// </summary>
		/// Literally does same job as SetEncrypted() but makes new instance instead of filling existing one,
		/// making it easier to initialize new variables from saved encrypted values.
		///
		/// <param name="encrypted">Raw encrypted value you got from GetEncrypted().</param>
		/// <param name="key">Encryption key you've got from GetEncrypted().</param>
		/// <returns>New obscured variable initialized from specified encrypted value.</returns>
		/// \sa GetEncrypted(), SetEncrypted()
		public static ObscuredFloat FromEncrypted(int encrypted, int key)
		{
			var instance = new ObscuredFloat();
			instance.SetEncrypted(encrypted, key);
			return instance;
		}

		/// <summary>
		/// Generates random key. Used internally and can be used to generate key for manual Encrypt() calls.
		/// </summary>
		/// <returns>Key suitable for manual Encrypt() calls.</returns>
		public static int GenerateKey()
		{
			return RandomUtils.GenerateIntKey();
		}

		private static bool Compare(float f1, float f2)
		{
			// var epsilon = ObscuredCheatingDetector.ExistsAndIsRunning ?
			// 	ObscuredCheatingDetector.Instance.floatEpsilon : float.Epsilon;
			return NumUtils.CompareFloats(f1, f2, float.Epsilon);
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// <param name="key">Encryption key needed to decrypt returned value.</param>
		/// <returns>Encrypted value as is.</returns>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		/// \sa FromEncrypted(), SetEncrypted()
		public int GetEncrypted(out int key)
		{
			if (!inited)
				Init();

			key = currentCryptoKey;
			return hiddenValue;
		}

		/// <summary>
		/// Allows to explicitly set current obscured value. Crypto key should be same as when encrypted value was got with GetEncrypted().
		/// </summary>
		/// Use it in conjunction with GetEncrypted().<br/>
		/// Useful for loading data stored in obscured state.
		/// \sa FromEncrypted()
		public void SetEncrypted(int encrypted, int key)
		{
			inited = true;
			hiddenValue = encrypted;
			currentCryptoKey = key;

			// if (ObscuredCheatingDetector.ExistsAndIsRunning)
			// {
			// 	fakeValueActive = false;
			// 	fakeValue = InternalDecrypt();
			// 	fakeValueActive = true;
			// }
			// else
			// {
			fakeValueActive = false;
			// }
		}

		/// <summary>
		/// Alternative to the type cast, use if you wish to get decrypted value
		/// but can't or don't want to use cast to the regular type.
		/// </summary>
		/// <returns>Decrypted value.</returns>
		public float GetDecrypted()
		{
			return InternalDecrypt();
		}

		public void RandomizeCryptoKey()
		{
			var decrypted = InternalDecrypt();
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(decrypted, currentCryptoKey);
		}

		private float InternalDecrypt()
		{
			if (!inited)
			{
				Init();
				return 0;
			}

#if ACTK_OBSCURED_AUTO_MIGRATION
			if (hiddenValueOldByte4.b1 != 0 ||
			    hiddenValueOldByte4.b2 != 0 ||
				hiddenValueOldByte4.b3 != 0 ||
				hiddenValueOldByte4.b4 != 0)
			{
				var union = new FloatIntBytesUnion {b4 = hiddenValueOldByte4};
				union.b4.Shuffle();
				hiddenValue = union.i;

				hiddenValueOldByte4.b1 = 0;
				hiddenValueOldByte4.b2 = 0;
				hiddenValueOldByte4.b3 = 0;
				hiddenValueOldByte4.b4 = 0;
			}
#endif

			var decrypted = Decrypt(hiddenValue, currentCryptoKey);
			// 			if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && !Compare(decrypted, fakeValue))
			// 			{
			// #if ACTK_DETECTION_BACKLOGS
			// 				Debug.LogWarning(ObscuredCheatingDetector.LogPrefix + "Detection backlog:\n" +
			// 				                             $"type: {nameof(ObscuredFloat)}\n" +
			// 				                             $"decrypted: {decrypted}\n" +
			// 				                             $"fakeValue: {fakeValue}\n" +
			// 				                             $"epsilon: {ObscuredCheatingDetector.Instance.floatEpsilon}\n" +
			// 				                             $"compare: {Compare(decrypted, fakeValue)}");
			// #endif
			// 				ObscuredCheatingDetector.Instance.OnCheatingDetected(this, decrypted, fakeValue);
			// 			}

			return decrypted;
		}

		private void Init()
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(0, currentCryptoKey);
			fakeValue = 0;
			fakeValueActive = false;
			inited = true;
		}

		//! @cond

#region obsolete
		[Obsolete("This API is redundant and does not perform any actions. It will be removed in future updates.")]
		public static void SetNewCryptoKey(int newKey) { }

		[Obsolete("This API is redundant and does not perform any actions. It will be removed in future updates.")]
		public void ApplyNewCryptoKey() { }

		[Obsolete("Please use new Encrypt(value, key) API instead.", true)]
		public static int Encrypt(float value)
		{
			throw new Exception();
		}

		[Obsolete("Please use new Decrypt(value, key) API instead.", true)]
		public static float Decrypt(int value)
		{
			throw new Exception();
		}

		[Obsolete("Please use new FromEncrypted(encrypted, key) API instead.", true)]
		public static ObscuredFloat FromEncrypted(int encrypted)
		{
			throw new Exception();
		}

		[Obsolete("Please use new GetEncrypted(out key) API instead.", true)]
		public int GetEncrypted()
		{
			throw new Exception();
		}

		[Obsolete("Please use new SetEncrypted(encrypted, key) API instead.", true)]
		public void SetEncrypted(int encrypted) { }
#endregion

		//! @endcond

		[StructLayout(LayoutKind.Explicit)]
		internal struct FloatIntBytesUnion
		{
			[FieldOffset(0)] private float f;
			[FieldOffset(0)] internal int i; // need to be internal for ACTK_OBSCURED_AUTO_MIGRATION
			[FieldOffset(0)] internal ACTkByte4 b4; // need to be internal for ACTK_OBSCURED_AUTO_MIGRATION

			public static int Migrate(int value, byte fromVersion, byte toVersion)
			{
				var u = FromInt(value);

				if (fromVersion < 2 && toVersion == 2)
					u.b4.Shuffle();

				return u.i;
			}

			internal static int XorFloatToInt(float value, int key)
			{
				return FromFloat(value).Shuffle(key).i;
			}

			internal static float XorIntToFloat(int value, int key)
			{
				return FromInt(value).UnShuffle(key).f;
			}

			private static FloatIntBytesUnion FromFloat(float value)
			{
				return new FloatIntBytesUnion { f = value };
			}

			private static FloatIntBytesUnion FromInt(int value)
			{
				return new FloatIntBytesUnion { i = value };
			}

			private FloatIntBytesUnion Shuffle(int key)
			{
				i ^= key;
				b4.Shuffle();

				return this;
			}

			private FloatIntBytesUnion UnShuffle(int key)
			{
				b4.UnShuffle();
				i ^= key;

				return this;
			}
		}
	}
}
