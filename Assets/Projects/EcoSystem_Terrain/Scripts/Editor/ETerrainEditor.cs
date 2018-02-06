using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EcoSystem {
	[CustomEditor(typeof(ETerrain))]
	public class ETerrainEditor : Editor {

		private ETerrain terrain;

		// TERRAIN PARAMS
		// General
		private SerializedProperty terrainDataProp;
		private SerializedProperty terrainMaterialProp;
		private SerializedProperty terrainSizeProp;
		// Chunks
		private SerializedProperty chunksCountXProp;
		private SerializedProperty chunksCountZProp;
		private SerializedProperty chunkQuadsProp;

		// TOOLS
		private int toolHash;
		private int controlID;
		private Vector2 mousePos = Vector2.zero;
		private Vector3 brushCenter;
		private SerializedProperty brushSizeProp;
		private SerializedProperty brushHardCenterProp;
		private SerializedProperty brushDensityProp;
		private bool isPainting = false;
		private bool isMouseOverTerrain = false;
		private bool isClicking = false;
		private bool shift = false;

		public void OnEnable() {
			terrain = (ETerrain) serializedObject.targetObject;
			terrain.LoadData(); // TODO Load at scene load instead of selection of terrain
			InitProperties();
			toolHash = GetHashCode();
		}

		private void InitProperties() {
			// Terrain Params
			terrainDataProp = serializedObject.FindProperty("data");
			terrainMaterialProp = serializedObject.FindProperty("terrainMaterial");
			terrainSizeProp = serializedObject.FindProperty("terrainSize");
			chunksCountXProp = serializedObject.FindProperty("chunksCountX");
			chunksCountZProp = serializedObject.FindProperty("chunksCountZ");
			chunkQuadsProp = serializedObject.FindProperty("chunkQuads");
			// Tools
			brushSizeProp = serializedObject.FindProperty("brushSize");
			brushHardCenterProp = serializedObject.FindProperty("brushHardCenter");
			brushDensityProp = serializedObject.FindProperty("brushDensity");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			
			// TERRAIN PARAMS
			EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(terrainDataProp);
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				terrain.LoadData();
			}
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(terrainMaterialProp);
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				terrain.UpdateTerrainMaterial();
			}
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(terrainSizeProp);
			if (EditorGUI.EndChangeCheck()) {
				terrain.Resize(terrainSizeProp.vector2Value, chunksCountXProp.intValue, chunksCountZProp.intValue);
			}

			EditorGUILayout.LabelField("Chunks", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck();
			EdGUIHelper.DrawDoublePropertyField("Chunk Count", "X", chunksCountXProp, "Z", chunksCountZProp);
			if (EditorGUI.EndChangeCheck()) { // Chunk Count Change
				terrain.Resize(terrainSizeProp.vector2Value, chunksCountXProp.intValue, chunksCountZProp.intValue);
			}
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(chunkQuadsProp, new GUIContent("Chunk Quads"));
			if (EditorGUI.EndChangeCheck()) { // Chunk Resolution Change
				terrain.SetChunkResolution(chunkQuadsProp.intValue);
			}

			if (GUILayout.Button("Generate")) {
				terrain.Generate();
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
			if (GUILayout.Button("Clear")) {
				terrain.Clear();
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}

			EditorGUILayout.LabelField("Chunks Count: " + terrain.ActualChunkCount);
			

			// TOOLS
			EditorGUILayout.LabelField("TOOLS", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(brushSizeProp);
			EditorGUILayout.PropertyField(brushHardCenterProp);
			EditorGUILayout.PropertyField(brushDensityProp);

			serializedObject.ApplyModifiedProperties();
		}

		private void OnSceneGUI() {
			Event e = Event.current;
			controlID = GUIUtility.GetControlID(toolHash, FocusType.Passive);

			shift = e.shift;

			switch (e.type) {
				case EventType.Repaint:
					Update();
					RepaintScene();
					break;
				case EventType.MouseDrag:
				case EventType.MouseMove:
					mousePos = new Vector2(e.mousePosition.x, e.mousePosition.y);
					break;
				case EventType.KeyDown:
					SceneKeydown(e.keyCode);
					break;
				case EventType.MouseDown:
					SceneMouseDown(e);
					break;
				case EventType.MouseUp:
					SceneMouseUp(e);
					break;
				case EventType.Layout:
					HandleUtility.AddDefaultControl(controlID);
					break;
			}
		}
		
		private void Update() {
			if (isPainting) {
				if (shift) {
					LowerBrush(terrain.virtualMesh.GetVerticesInRange2D(new Vector2(brushCenter.x, brushCenter.z), brushSizeProp.floatValue));
				} else {
					RaiseBrush(terrain.virtualMesh.GetVerticesInRange2D(new Vector2(brushCenter.x, brushCenter.z), brushSizeProp.floatValue));
				}
				terrain.virtualMesh.ApplyModifications();
			}
		}
		
		private void RepaintScene() {
			SceneView view = SceneView.currentDrawingSceneView;
			Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
			RaycastHit hit;
			if (terrain.Raycast(ray, out hit, 1000f)) {
				brushCenter = hit.point;
				Handles.color = Color.red;
				Handles.DrawWireDisc(brushCenter, Vector3.up, brushSizeProp.floatValue);
				Handles.DrawWireDisc(brushCenter, Vector3.up, brushSizeProp.floatValue * brushHardCenterProp.floatValue);
				isMouseOverTerrain = true;
				if (isClicking) {
					isPainting = true;
				}
			} else {
				isMouseOverTerrain = false;
				if (isPainting) {
					isPainting = false;
					terrain.RebuildCollisions();
				}
			}
		}
		
		private void RaiseBrush(Dictionary<VirtualVertex, float> vertices) {
			if (brushHardCenterProp.floatValue == 1f) {
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height += brushDensityProp.floatValue;
				}
			} else {
				float brushSizeSqr = Mathf.Pow(brushSizeProp.floatValue, 2f);
				float brushHardCenterSqr = Mathf.Pow(brushHardCenterProp.floatValue, 2f);
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height += Mathf.Clamp01(1 - (pair.Value - brushSizeSqr * brushHardCenterSqr) / (brushSizeSqr - brushSizeSqr * brushHardCenterSqr)) * brushDensityProp.floatValue;
				}
			}
		}

		private void LowerBrush(Dictionary<VirtualVertex, float> vertices) {
			if (brushHardCenterProp.floatValue == 1f) {
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height -= brushDensityProp.floatValue;
				}
			} else {
				float brushSizeSqr = Mathf.Pow(brushSizeProp.floatValue, 2f);
				float brushHardCenterSqr = Mathf.Pow(brushHardCenterProp.floatValue, 2f);
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height -= Mathf.Clamp01(1 - (pair.Value - brushSizeSqr * brushHardCenterSqr) / (brushSizeSqr - brushSizeSqr * brushHardCenterSqr)) * brushDensityProp.floatValue;
				}
			}
		}

		private void SceneKeydown(KeyCode key) {
			if (key == KeyCode.Keypad0) {
				
			}
		}

		private void SceneMouseDown(Event e) {
			if (e.button == 0) {
				isClicking = true;
				if (isMouseOverTerrain) {
					isPainting = true;
					e.Use();
				}
			}
		}

		private void SceneMouseUp(Event e) {
			if (e.button == 0) {
				isClicking = false;
				if (isPainting) {
					isPainting = false;
					e.Use();
					terrain.RebuildCollisions();
				}
			}
		}

	}
}
