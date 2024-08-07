﻿namespace com.ktgame.anticheat.obscured_types.editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredVector2))]
	internal class ObscuredVector2Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredVector2.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredVector2.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredVector2.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredVector2.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredVector2.fakeValueActive));
			
			var hiddenValueX = hiddenValue.FindPropertyRelative(nameof(ObscuredVector2.RawEncryptedVector2.x));
			var hiddenValueY = hiddenValue.FindPropertyRelative(nameof(ObscuredVector2.RawEncryptedVector2.y));

			var currentCryptoKey = cryptoKey.intValue;
			var val = Vector2.zero;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredVector2.GenerateKey();
				var ev = ObscuredVector2.Encrypt(Vector2.zero, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
                inited.boolValue = true;
				fakeValue.vector2Value = Vector2.zero;
			}
			else
			{
				var ev = new ObscuredVector2.RawEncryptedVector2
				{
					x = hiddenValueX.intValue,
					y = hiddenValueY.intValue
				};
				val = ObscuredVector2.Decrypt(ev, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = EditorGUI.Vector2Field(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				var ev = ObscuredVector2.Encrypt(val, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
				fakeValue.vector2Value = val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.wideMode ? EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight * 2f;
		}
	}
}