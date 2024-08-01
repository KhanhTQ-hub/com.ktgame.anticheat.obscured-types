﻿namespace com.ktgame.anticheat.obscured_types.editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredInt))]
	internal class ObscuredIntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredInt.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredInt.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredInt.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredInt.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredInt.fakeValueActive));

			var currentCryptoKey = cryptoKey.intValue;
			var val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredInt.GenerateKey();
				hiddenValue.intValue = ObscuredInt.Encrypt(0, currentCryptoKey);
				inited.boolValue = true;
			}
			else
			{
				val = ObscuredInt.Decrypt(hiddenValue.intValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
#if UNITY_2022_1_OR_NEWER
			val = EditorGUI.DelayedIntField(position, label, val);
#else
			val = EditorGUI.IntField(position, label, val);
#endif
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.intValue = ObscuredInt.Encrypt(val, currentCryptoKey);
				fakeValue.intValue = val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}