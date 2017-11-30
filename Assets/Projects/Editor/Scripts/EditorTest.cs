using UnityEngine;
using UnityEditor;

namespace Playground.Editor {
	public class EditorTest : EditorWindow {

		private SerializedObject winObj;
		[SerializeField]
		private GameObject selectedObj = null;
		private SerializedProperty selectedObjProp;

		[MenuItem("Window/EditorTest")]
		public static void ShowWindow() {
			GetWindow<EditorTest>();
		}

		public void OnEnable() {
			// Value init
			selectedObj = null;
			// Properties init
			winObj = new SerializedObject(this);
			ResetSelectedProperty();
		}

		private void ResetSelectedProperty() {
			selectedObjProp = winObj.FindProperty("selectedObj");
		}

		public void OnGUI() {
			if (selectedObj == null) {
				GUILayout.Label("Nothing Selected");
				return;
			}
			GUILayout.Label("Selected: " + selectedObj.name);
			if (GUILayout.Button("Reset Transform (Local)")) {
				Undo.RecordObject(selectedObj.transform, "Reset transform (Local)");
				selectedObj.transform.localPosition = Vector3.zero;
				selectedObj.transform.localRotation = Quaternion.identity;
				selectedObj.transform.localScale = Vector3.one;
			}
			if (GUILayout.Button("Reset Transform (World)")) {
				Undo.RecordObject(selectedObj.transform, "Reset transform (World)");
				selectedObj.transform.position = Vector3.zero;
				selectedObj.transform.rotation = Quaternion.identity;
				selectedObj.transform.localScale = Vector3.one;
			}
			winObj.ApplyModifiedProperties();
		}

		public void Update() {
			
		}

		private void OnSelectionChange() {
			selectedObj = Selection.activeGameObject;
			ResetSelectedProperty();
			Repaint();
		}

	}
}
