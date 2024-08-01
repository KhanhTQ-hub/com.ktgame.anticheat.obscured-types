namespace com.ktgame.anticheat.obscured_types.editor
{
	using System;
	using System.Collections;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using UnityEditor;

	internal static class SerializedPropertyExtensions
	{
		public static SerializedProperty GetProperty(this SerializedObject serializedObject, string propertyName)
		{
			return serializedObject.FindProperty($"<{propertyName}>k__BackingField");
		}

		public static T GetValue<T>(this SerializedProperty property) where T : class
		{
			var obj = (object)property.serializedObject.targetObject;
			var path = property.propertyPath.Replace(".Array.data", "");
			var fieldStructure = path.Split('.');
			var rgx = new Regex(@"\[\d+\]");
			foreach (var fieldPart in fieldStructure)
			{
				if (fieldPart.Contains("["))
				{
					var index = System.Convert.ToInt32(new string(fieldPart.Where(char.IsDigit)
						.ToArray()));
					obj = GetFieldValueWithIndex(rgx.Replace(fieldPart, ""), obj, index);
				}
				else
				{
					obj = GetFieldValue(fieldPart, obj);
				}
			}

			return (T)obj;
		}

		private static object GetFieldValue(string fieldName, object obj,
			BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
									BindingFlags.NonPublic)
		{
			var field = obj.GetType().GetField(fieldName, bindings);
			return field != null ? field.GetValue(obj) : default;
		}

		private static object GetFieldValueWithIndex(string fieldName, object obj, int index,
			BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
									BindingFlags.NonPublic)
		{
			var field = obj.GetType().GetField(fieldName, bindings);
			if (field != null)
			{
				var list = field.GetValue(obj);
				if (list.GetType().IsArray)
					return ((Array)list).GetValue(index);

				if (list is IEnumerable)
					return ((IList)list)[index];
			}

			return default;
		}
	}
}
