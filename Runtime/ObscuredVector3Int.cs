﻿namespace com.ktgame.anticheat.obscured_types
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Use it instead of regular <c>Vector3Int</c> for any cheating-sensitive variables.
	/// </summary>
	/// <strong>\htmlonly<font color="FF4040">WARNING:</font>\endhtmlonly Doesn't mimic regular type API, thus should be used with extra caution.</strong> Cast it to regular, not obscured type to work with regular APIs.<br/>
	/// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong><br/>
	/// Feel free to use regular types for all short-term operations and calculations while keeping obscured type only at the long-term declaration (i.e. class field).
	[Serializable]
	public partial struct ObscuredVector3Int : IObscuredType
	{
		private static readonly Vector3Int Zero = Vector3Int.zero;

		[SerializeField] internal int currentCryptoKey;
		[SerializeField] internal RawEncryptedVector3Int hiddenValue;
		[SerializeField] internal Vector3Int fakeValue;
		[SerializeField] internal bool fakeValueActive;
		[SerializeField] internal bool inited;

		private ObscuredVector3Int(Vector3Int value)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(value, currentCryptoKey);

#if UNITY_EDITOR
			fakeValue = value;
			fakeValueActive = true;
#else
			var detectorRunning = ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = detectorRunning ? value : Zero;
			fakeValueActive = detectorRunning;
#endif
			inited = true;
		}

		/// <summary>
		/// Mimics constructor of regular Vector3Int.
		/// </summary>
		/// <param name="x">X component of the vector</param>
		/// <param name="y">Y component of the vector</param>
		/// <param name="z">Z component of the vector</param>
		public ObscuredVector3Int(int x, int y, int z)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(x, y, z, currentCryptoKey);

			// if (ObscuredCheatingDetector.ExistsAndIsRunning)
			// {
			// 	fakeValue = new Vector3Int
			// 	{
			// 		x = x,
			// 		y = y,
			// 		z = z
			// 	};
			// 	fakeValueActive = true;
			// }
			// else
			// {
			fakeValue = Zero;
			fakeValueActive = false;
			// }

			inited = true;
		}

		public int x
		{
			get
			{
				var decrypted = ObscuredInt.Decrypt(hiddenValue.x, currentCryptoKey);
				// 				if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue.x)
				// 				{
				// #if ACTK_DETECTION_BACKLOGS
				// 					Debug.LogWarning(ObscuredCheatingDetector.LogPrefix + "Detection backlog:\n" +
				// 					                 $"type: {nameof(ObscuredVector3Int)}\n" +
				// 					                 $"decrypted.x: {decrypted}\n" +
				// 					                 $"fakeValue.x: {fakeValue.x}");
				// #endif
				// 					ObscuredCheatingDetector.Instance.OnCheatingDetected(this, decrypted, fakeValue);
				// 				}

				return decrypted;
			}

			set
			{
				hiddenValue.x = ObscuredInt.Encrypt(value, currentCryptoKey);
				// if (ObscuredCheatingDetector.ExistsAndIsRunning)
				// {
				// 	fakeValue.x = value;
				// 	fakeValue.y = ObscuredInt.Decrypt(hiddenValue.y, currentCryptoKey);
				// 	fakeValue.z = ObscuredInt.Decrypt(hiddenValue.z, currentCryptoKey);
				// 	fakeValueActive = true;
				// }
				// else
				// {
				fakeValueActive = false;
				// }
			}
		}

		public int y
		{
			get
			{
				var decrypted = ObscuredInt.Decrypt(hiddenValue.y, currentCryptoKey);
				// 				if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue.y)
				// 				{
				// #if ACTK_DETECTION_BACKLOGS
				// 					Debug.LogWarning(ObscuredCheatingDetector.LogPrefix + "Detection backlog:\n" +
				// 					                 $"type: {nameof(ObscuredVector3Int)}\n" +
				// 					                 $"decrypted.y: {decrypted}\n" +
				// 					                 $"fakeValue.y: {fakeValue.y}");
				// #endif
				// 					ObscuredCheatingDetector.Instance.OnCheatingDetected(this, decrypted, fakeValue);
				// 				}

				return decrypted;
			}

			set
			{
				hiddenValue.y = ObscuredInt.Encrypt(value, currentCryptoKey);
				// if (ObscuredCheatingDetector.ExistsAndIsRunning)
				// {
				// 	fakeValue.x = ObscuredInt.Decrypt(hiddenValue.x, currentCryptoKey);
				// 	fakeValue.y = value;
				// 	fakeValue.z = ObscuredInt.Decrypt(hiddenValue.z, currentCryptoKey);
				// 	fakeValueActive = true;
				// }
				// else
				// {
				fakeValueActive = false;
				// }
			}
		}

		public int z
		{
			get
			{
				var decrypted = ObscuredInt.Decrypt(hiddenValue.z, currentCryptoKey);
				// 				if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue.z)
				// 				{
				// #if ACTK_DETECTION_BACKLOGS
				// 					Debug.LogWarning(ObscuredCheatingDetector.LogPrefix + "Detection backlog:\n" +
				// 					                 $"type: {nameof(ObscuredVector3Int)}\n" +
				// 					                 $"decrypted.z: {decrypted}\n" +
				// 					                 $"fakeValue.z: {fakeValue.z}");
				// #endif
				// 					ObscuredCheatingDetector.Instance.OnCheatingDetected(this, decrypted, fakeValue);
				// 				}

				return decrypted;
			}

			set
			{
				hiddenValue.z = ObscuredInt.Encrypt(value, currentCryptoKey);
				// if (ObscuredCheatingDetector.ExistsAndIsRunning)
				// {
				// 	fakeValue.x = ObscuredInt.Decrypt(hiddenValue.x, currentCryptoKey);
				// 	fakeValue.y = ObscuredInt.Decrypt(hiddenValue.y, currentCryptoKey);
				// 	fakeValue.z = value;
				// 	fakeValueActive = true;
				// }
				// else
				// {
				fakeValueActive = false;
				// }
			}
		}

		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return x;
					case 1:
						return y;
					case 2:
						return z;
					default:
						throw new IndexOutOfRangeException("Invalid ObscuredVector3Int index!");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
					case 2:
						z = value;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid ObscuredVector3Int index!");
				}
			}
		}

		/// <summary>
		/// Encrypts passed value using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static RawEncryptedVector3Int Encrypt(Vector3Int value, int key)
		{
			return Encrypt(value.x, value.y, value.z, key);
		}

		/// <summary>
		/// Encrypts passed components using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static RawEncryptedVector3Int Encrypt(int x, int y, int z, int key)
		{
			RawEncryptedVector3Int result;
			result.x = ObscuredInt.Encrypt(x, key);
			result.y = ObscuredInt.Encrypt(y, key);
			result.z = ObscuredInt.Encrypt(z, key);

			return result;
		}

		/// <summary>
		/// Decrypts passed value you got from Encrypt() using same key.
		/// </summary>
		/// \sa Encrypt()
		public static Vector3Int Decrypt(RawEncryptedVector3Int value, int key)
		{
			var result = new Vector3Int
			{
				x = ObscuredInt.Decrypt(value.x, key),
				y = ObscuredInt.Decrypt(value.y, key),
				z = ObscuredInt.Decrypt(value.z, key)
			};

			return result;
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
		public static ObscuredVector3Int FromEncrypted(RawEncryptedVector3Int encrypted, int key)
		{
			var instance = new ObscuredVector3Int();
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

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// <param name="key">Encryption key needed to decrypt returned value.</param>
		/// <returns>Encrypted value as is.</returns>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		/// \sa FromEncrypted(), SetEncrypted()
		public RawEncryptedVector3Int GetEncrypted(out int key)
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
		public void SetEncrypted(RawEncryptedVector3Int encrypted, int key)
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
		public Vector3Int GetDecrypted()
		{
			return InternalDecrypt();
		}

		public void RandomizeCryptoKey()
		{
			var decrypted = InternalDecrypt();
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(decrypted, currentCryptoKey);
		}

		private Vector3Int InternalDecrypt()
		{
			if (!inited)
			{
				Init();
				return Zero;
			}

			var decrypted = Decrypt(hiddenValue, currentCryptoKey);

			// 			if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue)
			// 			{
			// #if ACTK_DETECTION_BACKLOGS
			// 				Debug.LogWarning(ObscuredCheatingDetector.LogPrefix + "Detection backlog:\n" +
			// 				                 $"type: {nameof(ObscuredVector3Int)}\n" +
			// 				                 $"decrypted: {decrypted}\n" +
			// 				                 $"fakeValue: {fakeValue}");
			// #endif
			// 				ObscuredCheatingDetector.Instance.OnCheatingDetected(this, decrypted, fakeValue);
			// 			}

			return decrypted;
		}

		private void Init()
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(Zero, currentCryptoKey);
			fakeValue = Zero;
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
		public static RawEncryptedVector3Int Encrypt(Vector3Int value)
		{
			throw new Exception();
		}

		[Obsolete("Please use new Decrypt(value, key) API instead.", true)]
		public static Vector3Int Decrypt(RawEncryptedVector3Int value)
		{
			throw new Exception();
		}

		[Obsolete("Please use new GetEncrypted(out key) API instead.", true)]
		public RawEncryptedVector3Int GetEncrypted()
		{
			throw new Exception();
		}

		[Obsolete("Please use new SetEncrypted(encrypted, key) API instead.", true)]
		public void SetEncrypted(RawEncryptedVector3Int encrypted) { }
#endregion

		//! @endcond
	}
}
