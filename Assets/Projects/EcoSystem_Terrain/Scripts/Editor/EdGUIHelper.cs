using UnityEditor;
using UnityEngine;

namespace EcoSystem {
	public static class EdGUIHelper {

		public static void DrawDoublePropertyField(string mainLabel, string label1, SerializedProperty prop1, string label2, SerializedProperty prop2) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(mainLabel);
			Rect pos = EditorGUILayout.GetControlRect();
			pos.x -= 1f;
			pos.width += 1f;
			GUIContent[] subLabels = { new GUIContent(label1), new GUIContent(label2), null };
			SerializedProperty[] values = { prop1, prop2, null };
			MultiPropertyField(pos, subLabels, values);
			EditorGUILayout.EndHorizontal();
		}

		public static void MultiPropertyField(Rect position, GUIContent[] subLabels, SerializedProperty[] values) {
			MultiPropertyField(position, subLabels, values, 13f);
		}

		public static void MultiPropertyField(Rect position, GUIContent[] subLabels, SerializedProperty[] values, float labelWidth) {
			int num = values.Length;
			float num2 = (position.width - (float) (num - 1) * 2f) / (float) num;
			Rect position2 = new Rect(position);
			position2.width = num2;
			float labelWidth2 = EditorGUIUtility.labelWidth;
			int indentLevel = EditorGUI.indentLevel;
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.indentLevel = 0;
			for (int i = 0; i < values.Length; i++) {
				if (values[i] != null) {
					EditorGUI.PropertyField(position2, values[i], subLabels[i]);
				}
				position2.x += num2 + 2f;
			}
			EditorGUIUtility.labelWidth = labelWidth2;
			EditorGUI.indentLevel = indentLevel;
		}

	}
}
