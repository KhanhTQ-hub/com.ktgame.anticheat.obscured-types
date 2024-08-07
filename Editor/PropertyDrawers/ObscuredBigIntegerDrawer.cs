﻿namespace com.ktgame.anticheat.obscured_types.editor
{
	using System.Numerics;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredBigInteger))]
	internal class ObscuredBigIntegerDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty sp, GUIContent label)
		{
			var hiddenValue = sp.FindPropertyRelative(nameof(ObscuredBigInteger.hiddenValue));
			var cryptoKey = sp.FindPropertyRelative(nameof(ObscuredBigInteger.currentCryptoKey));
			var inited = sp.FindPropertyRelative(nameof(ObscuredBigInteger.inited));
			var fakeValue = sp.FindPropertyRelative(nameof(ObscuredBigInteger.fakeValue));
			var fakeValueActive = sp.FindPropertyRelative(nameof(ObscuredBigInteger.fakeValueActive));

			var currentCryptoKey = (uint)cryptoKey.intValue;
			BigInteger val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
				{
					currentCryptoKey = ObscuredBigInteger.GenerateKey();
					cryptoKey.intValue = (int)currentCryptoKey;
				}
				
				var encrypted = ObscuredBigInteger.Encrypt(0, currentCryptoKey);
				SetBigInteger(hiddenValue, encrypted);
				inited.boolValue = true;
				SetBigInteger(fakeValue, 0);
			}
			else
			{
				val = ObscuredBigInteger.Decrypt(GetBigInteger(hiddenValue), currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, sp);

			EditorGUI.BeginChangeCheck();
#if UNITY_2022_1_OR_NEWER
			var bigIntString = EditorGUI.DelayedTextField(position, label, val.ToString());
#else
			var bigIntString = EditorGUI.TextField(position, label, val.ToString());
#endif
			if (EditorGUI.EndChangeCheck())
			{
				if (!BigInteger.TryParse(bigIntString, out var newValue))
					newValue = 0;
				
				var encrypted = ObscuredBigInteger.Encrypt(newValue, currentCryptoKey);
				SetBigInteger(hiddenValue, encrypted);
				SetBigInteger(fakeValue, newValue);
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}

		private static BigInteger GetBigInteger(SerializedProperty serializableBigInteger)
		{
			var result = new SerializableBigInteger();
			var rawProperty = serializableBigInteger.FindPropertyRelative(nameof(SerializableBigInteger.raw));
			var signProperty = rawProperty.FindPropertyRelative(nameof(SerializableBigInteger.BigIntegerContents.sign));
			var bitsProperty = rawProperty.FindPropertyRelative(nameof(SerializableBigInteger.BigIntegerContents.bits));
			var bits = ReadBitsArray(bitsProperty);
			
			result.raw = new SerializableBigInteger.BigIntegerContents
			{
				sign = signProperty.intValue,
				bits = bits
			};

			return result.value;
		}
		
		private static void SetBigInteger(SerializedProperty serializableBigInteger, BigInteger value)
		{
			var explicitStruct = new SerializableBigInteger
			{
				value = value
			};

			var sign = explicitStruct.raw.sign;
			var bits = explicitStruct.raw.bits;
			
			var rawProperty = serializableBigInteger.FindPropertyRelative(nameof(SerializableBigInteger.raw));
			var signProperty = rawProperty.FindPropertyRelative(nameof(SerializableBigInteger.BigIntegerContents.sign));
			var bitsProperty = rawProperty.FindPropertyRelative(nameof(SerializableBigInteger.BigIntegerContents.bits));

			signProperty.intValue = sign;
			WriteBitsArray(bitsProperty, bits);
		}
		
		private static uint[] ReadBitsArray(SerializedProperty bits)
		{
			var count = bits.arraySize;
			if (count == 0)
				return null;
			var result = new uint[count];
			for (var i = 0; i < count; i++)
				result[i] = (uint)bits.GetArrayElementAtIndex(i).longValue;

			return result;
		}

		private static void WriteBitsArray(SerializedProperty bitsProperty, uint[] bits)
		{
			bitsProperty.arraySize = bits?.Length ?? 0;
			for (var i = 0; i < bits?.Length; i++)
				bitsProperty.GetArrayElementAtIndex(i).longValue = bits[i];
		}
	}
}