using UnityEngine;
using UnityEditor;

namespace Playground.Editor {
	public class EditorTest : EditorWindow {

		[HideInInspector]
		[SerializeField]
		private GameObject selectedObj = null;
		[SerializeField]
		private EditorData data;
		
		private bool DisplayVertices {
			get {
				return data.DisplayVertices;
			}
			set {
				if (value != data.DisplayVertices) {
					data.DisplayVertices = value;
					SceneView.RepaintAll();
				}
			}
		}
		[SerializeField]
		private Color VerticesColor {
			get {
				return data.VerticesColor;
			}
			set {
				if (value != data.VerticesColor) {
					data.VerticesColor = value;
					SceneView.RepaintAll();
				}
			}
		}
		
		private bool DepthCull {
			get {
				return data.DepthCull;
			}
			set {
				if (value != data.DepthCull) {
					data.DepthCull = value;
					SceneView.RepaintAll();
				}
			}
		}
		private bool VertexCount {
			get {
				return data.VertexCount;
			}
			set {
				if (value != data.VertexCount) {
					data.VertexCount = value;
					SceneView.RepaintAll();
				}
			}
		}

		private SerializedObject winObj;

		[MenuItem("Window/EditorTest")]
		public static void ShowWindow() {
			GetWindow<EditorTest>();
		}

		public void OnEnable() {
			//data = CreateInstance<EditorData>();

			Undo.undoRedoPerformed += OnUndoRedo;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			// Value init
			selectedObj = Selection.activeGameObject;
			// Properties init
			winObj = new SerializedObject(this);

		}

		public void OnGUI() {
			if (selectedObj == null) {
				GUILayout.Label("Nothing Selected");
				return;
			}
			GUILayout.Label("Selected: " + selectedObj.name);
			DisplayVertices = GUILayout.Toggle(DisplayVertices, "Display Vertices");
			if (DisplayVertices) {
				VerticesColor = EditorGUILayout.ColorField("Vertices Color", VerticesColor);
				GUILayout.BeginHorizontal();
				DepthCull = GUILayout.Toggle(DepthCull, "Depth Cull");
				VertexCount = GUILayout.Toggle(VertexCount, "Vertex Count");
				GUILayout.EndHorizontal();
			}
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
			if (GUILayout.Button("Mesh Test")) {
				MeshTest();
			}
			winObj.ApplyModifiedProperties();
			CheckRepaint();
		}

		private void CheckRepaint() {
			SceneView.RepaintAll();
		}

		private void OnSceneGUI(SceneView view) {
			if (selectedObj == null || !selectedObj.activeInHierarchy) return;
			EditorGUI.BeginChangeCheck();
			data.VerticesColor.a = Handles.RadiusHandle(Quaternion.identity, selectedObj.transform.position, data.VerticesColor.a * 2) / 2;
			if (EditorGUI.EndChangeCheck()) {
				Repaint();
			}

			if (Event.current.type != EventType.Repaint) return;
			MeshFilter filter = selectedObj.GetComponent<MeshFilter>();
			if (filter == null) return;
			Mesh mesh = filter.sharedMesh;
			if (mesh == null) return;

			if (DisplayVertices) {
				if (DepthCull) {
					Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
				}
				Handles.color = VerticesColor;
				Vector3[] verts = mesh.vertices;
				for (int i = 0; i < verts.Length; i++) {
					Vector3 worldPos = selectedObj.transform.position + verts[i];
					//Handles.SphereHandleCap(0, worldPos, Quaternion.identity, HandleUtility.GetHandleSize(worldPos) * .1f, EventType.Repaint);
					//Handles.CircleHandleCap(0, worldPos, Quaternion.LookRotation(norms[i]), HandleUtility.GetHandleSize(worldPos) * .1f, EventType.Repaint);
					//Handles.DotHandleCap(0, worldPos, Quaternion.identity, HandleUtility.GetHandleSize(worldPos) * .025f, EventType.Repaint);
					Handles.CubeHandleCap(0, worldPos, Quaternion.identity, HandleUtility.GetHandleSize(worldPos) * .075f, EventType.Repaint);
					//verts[i].x = Handles.ScaleValueHandle(verts[i].x, worldPos, Quaternion.identity, HandleUtility.GetHandleSize(worldPos) * 1.5f, Handles.ConeHandleCap, 0.1f);
					//verts[i] = Handles.FreeMoveHandle(worldPos, Quaternion.identity, HandleUtility.GetHandleSize(worldPos) * 1f, Vector3.one, Handles.ArrowHandleCap) - selectedObj.transform.position;
				}
				if (VertexCount) {
					Handles.Label(selectedObj.transform.position + Vector3.Cross((SceneView.GetAllSceneCameras()[0].transform.position - selectedObj.transform.position), Vector3.up).normalized * 1f, "vertices: " + verts.Length);
				}
				//data.VerticesColor.a = Handles.ScaleValueHandle(data.VerticesColor.a, selectedObj.transform.position, Quaternion.identity, 10f, Handles.ArrowHandleCap, 0.01f);
				Handles.color = Color.red;
				//data.VerticesColor.a = Handles.RadiusHandle(Quaternion.identity, selectedObj.transform.position, data.VerticesColor.a * 2) / 2;
				//mesh.vertices = verts;
			}
		}

		public void MeshTest() {
			MeshFilter filter = selectedObj.GetComponent<MeshFilter>();
			if (filter == null) return;
			Mesh mesh = filter.sharedMesh;
			if (mesh == null) return;

			Vector3[] verts = mesh.vertices;
			for (int i = 0; i < verts.Length; i++) {
				verts[i].y -= 1f;
			}
			Undo.RecordObject(mesh, "Mesh Test");
			mesh.vertices = verts;
		}

		private void OnSelectionChange() {
			selectedObj = Selection.activeGameObject;
			Repaint();
		}

		private void OnUndoRedo() {
			if (selectedObj == null) return;
			MeshFilter filter = selectedObj.GetComponent<MeshFilter>();
			if (filter == null) return;
			Mesh mesh = filter.sharedMesh;
			if (mesh == null) return;
			
			mesh.vertices = mesh.vertices;
			SceneView.RepaintAll();
		}

		private void OnDisable() {
			selectedObj = null;
			SceneView.RepaintAll();
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

	}
}
