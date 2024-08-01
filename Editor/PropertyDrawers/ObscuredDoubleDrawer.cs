﻿namespace com.ktgame.anticheat.obscured_types.editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredDouble))]
	internal class ObscuredDoubleDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredDouble.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredDouble.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredDouble.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredDouble.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredDouble.fakeValueActive));

			var currentCryptoKey = cryptoKey.longValue;

			double val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.longValue = ObscuredDouble.GenerateKey();

				inited.boolValue = true;
				hiddenValue.longValue = ObscuredDouble.Encrypt(0, currentCryptoKey);
			}
			else
			{
				val = ObscuredDouble.Decrypt(hiddenValue.longValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
#if UNITY_2022_1_OR_NEWER
			val = EditorGUI.DelayedDoubleField(position, label, val);
#else
			val = EditorGUI.DoubleField(position, label, val);
#endif
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.longValue = ObscuredDouble.Encrypt(val, currentCryptoKey);
				fakeValue.doubleValue = val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}