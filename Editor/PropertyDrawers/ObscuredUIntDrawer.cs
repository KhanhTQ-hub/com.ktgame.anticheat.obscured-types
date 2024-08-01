﻿namespace com.ktgame.anticheat.obscured_types.editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredUInt))]
	internal class ObscuredUIntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredUInt.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredUInt.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredUInt.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredUInt.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredUInt.fakeValueActive));

			var currentCryptoKey = (uint)cryptoKey.intValue;
			uint val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					cryptoKey.intValue = (int)(currentCryptoKey = ObscuredUInt.GenerateKey());
				hiddenValue.intValue = (int)ObscuredUInt.Encrypt(0, currentCryptoKey);
				inited.boolValue = true;
			}
			else
			{
				val = ObscuredUInt.Decrypt((uint)hiddenValue.intValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
#if UNITY_2022_1_OR_NEWER
			val = (uint)EditorGUI.DelayedIntField(position, label, (int)val);
#else
			val = (uint)EditorGUI.IntField(position, label, (int)val);
#endif
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.intValue = (int)ObscuredUInt.Encrypt(val, currentCryptoKey);
				fakeValue.intValue = (int)val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}