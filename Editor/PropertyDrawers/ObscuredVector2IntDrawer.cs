﻿namespace com.ktgame.anticheat.obscured_types.editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredVector2Int))]
	internal class ObscuredVector2IntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative(nameof(ObscuredVector2Int.hiddenValue));
			var cryptoKey = prop.FindPropertyRelative(nameof(ObscuredVector2Int.currentCryptoKey));
			var inited = prop.FindPropertyRelative(nameof(ObscuredVector2Int.inited));
			var fakeValue = prop.FindPropertyRelative(nameof(ObscuredVector2Int.fakeValue));
			var fakeValueActive = prop.FindPropertyRelative(nameof(ObscuredVector2Int.fakeValueActive));
			
			var hiddenValueX = hiddenValue.FindPropertyRelative(nameof(ObscuredVector2Int.RawEncryptedVector2Int.x));
			var hiddenValueY = hiddenValue.FindPropertyRelative(nameof(ObscuredVector2Int.RawEncryptedVector2Int.y));

			var currentCryptoKey = cryptoKey.intValue;
			var val = Vector2Int.zero;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
					currentCryptoKey = cryptoKey.intValue = ObscuredVector2Int.GenerateKey();
				var ev = ObscuredVector2Int.Encrypt(Vector2Int.zero, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
                inited.boolValue = true;
				fakeValue.vector2IntValue = Vector2Int.zero;
			}
			else
			{
				var ev = new ObscuredVector2Int.RawEncryptedVector2Int
				{
					x = hiddenValueX.intValue,
					y = hiddenValueY.intValue
				};
				val = ObscuredVector2Int.Decrypt(ev, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = EditorGUI.Vector2IntField(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				var ev = ObscuredVector2Int.Encrypt(val, currentCryptoKey);
				hiddenValueX.intValue = ev.x;
				hiddenValueY.intValue = ev.y;
				fakeValue.vector2IntValue = val;
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