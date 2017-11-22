using UnityEngine;
using UnityEditor;

namespace Playground.Editor {
	public class EditorTest : EditorWindow {

		private SerializedObject winObj;
		public GameObject editDebug;
		private SerializedProperty propEditDebug;

		[MenuItem("Window/EditorTest")]
		public static void ShowWindow() {
			GetWindow<EditorTest>();
		}

		public void OnEnable() {
			// Value init
			editDebug = null;
			// Properties init
			winObj = new SerializedObject(this);
			propEditDebug = winObj.FindProperty("editDebug");
		}

		public void OnGUI() {
			EditorGUILayout.PropertyField(propEditDebug, new GUIContent("edit debug"));
			if (GUILayout.Button("Test")) {
				Undo.RecordObject(editDebug.transform, "1 up");
				editDebug.transform.position += Vector3.up;
			}
			winObj.ApplyModifiedProperties();
		}

	}
}
