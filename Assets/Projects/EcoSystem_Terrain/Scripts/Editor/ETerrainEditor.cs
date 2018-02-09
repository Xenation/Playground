using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Xenon;

namespace EcoSystem {
	[CustomEditor(typeof(ETerrain))]
	public class ETerrainEditor : Editor {

		private enum Tool {
			RaiseLower,
			Flatten,
			Smooth,
			VertexPaint
		}

#region ATTRIBUTES
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
		private SerializedProperty flattenHeightProp;
		private SerializedProperty brushCurveProp;
		private SerializedProperty brushUseCurveProp;
		private SerializedProperty brushPaintColorProp;
		private bool isPainting = false;
		private bool isMouseOverTerrain = false;
		private bool isClicking = false;
		private bool shift = false;
		private Tool selectedTool = Tool.RaiseLower;
		private bool isRaiseLowerTool { get { return selectedTool == Tool.RaiseLower; } }
		private bool isFlattenTool { get { return selectedTool == Tool.Flatten; } }
		private bool isSmoothTool { get { return selectedTool == Tool.Smooth; } }
		private bool isVertexPaintTool { get { return selectedTool == Tool.VertexPaint; } }

		private GUIStyle vertexDebugStyle;
		#endregion

#region METHODS
#region Initialization
		public void OnEnable() {
			terrain = (ETerrain) serializedObject.targetObject;
			terrain.LoadData(); // TODO Load at scene load instead of selection of terrain
			InitProperties();
			toolHash = GetHashCode();
			vertexDebugStyle = new GUIStyle();
			vertexDebugStyle.normal.textColor = Color.blue;
		}

		private void InitProperties() {
			// Terrain Params
			terrainDataProp = serializedObject.FindProperty("data");
			terrainMaterialProp = serializedObject.FindProperty("terrainMaterial");
			terrainSizeProp = serializedObject.FindProperty("terrainSize");
			chunksCountXProp = serializedObject.FindProperty("chunksCountX");
			chunksCountZProp = serializedObject.FindProperty("chunksCountZ");
			chunkQuadsProp = serializedObject.FindProperty("desiredQuads");
			// Tools
			brushSizeProp = serializedObject.FindProperty("brushSize");
			brushHardCenterProp = serializedObject.FindProperty("brushHardCenter");
			brushDensityProp = serializedObject.FindProperty("brushDensity");
			flattenHeightProp = serializedObject.FindProperty("flattenHeight");
			brushCurveProp = serializedObject.FindProperty("brushCurve");
			brushUseCurveProp = serializedObject.FindProperty("brushUseCurve");
			brushPaintColorProp = serializedObject.FindProperty("brushPaintColor");
		}
#endregion

#region GUI
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
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Toggle(isRaiseLowerTool, "Raise/Lower", EditorStyles.miniButtonLeft, GUILayout.MinHeight(30f))) {
				selectedTool = Tool.RaiseLower;
			}
			if (GUILayout.Toggle(isFlattenTool, "Flatten", EditorStyles.miniButtonMid, GUILayout.MinHeight(30f))) {
				selectedTool = Tool.Flatten;
			}
			if (GUILayout.Toggle(isSmoothTool, "Smooth", EditorStyles.miniButtonMid, GUILayout.MinHeight(30f))) {
				selectedTool = Tool.Smooth;
			}
			if (GUILayout.Toggle(isVertexPaintTool, "Paint", EditorStyles.miniButtonRight, GUILayout.MinHeight(30f))) {
				selectedTool = Tool.VertexPaint;
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(brushSizeProp);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Brush Shape");
			if (GUILayout.Toggle(!brushUseCurveProp.boolValue, "Simple", EditorStyles.miniButtonLeft)) {
				brushUseCurveProp.boolValue = false;
			}
			if (GUILayout.Toggle(brushUseCurveProp.boolValue, "Curve", EditorStyles.miniButtonRight)) {
				brushUseCurveProp.boolValue = true;
			}
			EditorGUILayout.EndHorizontal();
			if (brushUseCurveProp.boolValue) {
				EditorGUILayout.PropertyField(brushCurveProp);
			} else {
				EditorGUILayout.PropertyField(brushHardCenterProp);
			}
			if (!isFlattenTool) {
				EditorGUILayout.PropertyField(brushDensityProp);
			}
			if (isVertexPaintTool) {
				EditorGUILayout.PropertyField(brushPaintColorProp);
			}
			switch (selectedTool) {
				case Tool.Flatten:
					EditorGUILayout.PropertyField(flattenHeightProp);
					break;
			}

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
#endregion

#region Updating
		private void Update() {
			if (isPainting) {
				Brush();
			}
		}
		
		private void RepaintScene() {
			SceneView view = SceneView.currentDrawingSceneView;
			Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
			RaycastHit hit;
			if (terrain.Raycast(ray, out hit, 5000f)) {
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
			VirtualVertex pointed = terrain.virtualMesh.GetVertexAtWorldPos(brushCenter);
			if (pointed != null) {
				Handles.Label(pointed.vertex, pointed.ToString(), vertexDebugStyle);
			}
		}
#endregion

#region Brushes
		private void Brush() {
			TimingDebugger.Start("Brush");
			Dictionary<VirtualVertex, float> inRangeByDist;
			switch (selectedTool) {
				case Tool.RaiseLower:
					inRangeByDist = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(brushCenter.x, brushCenter.z), brushSizeProp.floatValue);
					if (shift) {
						LowerBrush(inRangeByDist);
					} else {
						RaiseBrush(inRangeByDist);
					}
					break;
				case Tool.Flatten:
					inRangeByDist = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(brushCenter.x, brushCenter.z), brushSizeProp.floatValue);
					FlattenBrush(inRangeByDist);
					break;
				case Tool.Smooth:
					Dictionary<Vector2i, VirtualVertex> inRangeByPos = terrain.virtualMesh.GetVerticesByPositionIn2DRange(new Vector2(brushCenter.x, brushCenter.z), brushSizeProp.floatValue);
					SmoothBrush(inRangeByPos);
					break;
				case Tool.VertexPaint:
					inRangeByDist = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(brushCenter.x, brushCenter.z), brushSizeProp.floatValue);
					VertexPaintBrush(inRangeByDist);
					break;
			}
			terrain.virtualMesh.ApplyModifications();
			TimingDebugger.Stop();
		}

		private float StrengthFromDistanceSqr(float distanceSqr, float brushSizeSqr, float brushHardCenterSqr) {
			if (brushUseCurveProp.boolValue) {
				return brushCurveProp.animationCurveValue.Evaluate(1f - (distanceSqr / brushSizeSqr)) * brushDensityProp.floatValue;
			} else {
				return Mathf.Clamp01(1 - (distanceSqr - brushSizeSqr * brushHardCenterSqr) / (brushSizeSqr - brushSizeSqr * brushHardCenterSqr)) * brushDensityProp.floatValue;
			}
		}
		
		private void RaiseBrush(Dictionary<VirtualVertex, float> vertices) {
			TimingDebugger.Start("Raise");
			if (brushHardCenterProp.floatValue == 1f) {
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height += brushDensityProp.floatValue;
				}
			} else {
				float brushSizeSqr = Mathf.Pow(brushSizeProp.floatValue, 2f);
				float brushHardCenterSqr = Mathf.Pow(brushHardCenterProp.floatValue, 2f);
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height += StrengthFromDistanceSqr(pair.Value, brushSizeSqr, brushHardCenterSqr);
				}
			}
			TimingDebugger.Stop();
		}

		private void LowerBrush(Dictionary<VirtualVertex, float> vertices) {
			TimingDebugger.Start("Lower");
			if (brushHardCenterProp.floatValue == 1f) {
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height -= brushDensityProp.floatValue;
				}
			} else {
				float brushSizeSqr = Mathf.Pow(brushSizeProp.floatValue, 2f);
				float brushHardCenterSqr = Mathf.Pow(brushHardCenterProp.floatValue, 2f);
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height -= StrengthFromDistanceSqr(pair.Value, brushSizeSqr, brushHardCenterSqr);
				}
			}
			TimingDebugger.Stop();
		}

		private void FlattenBrush(Dictionary<VirtualVertex, float> vertices) {
			TimingDebugger.Start("Flatten");
			if (brushHardCenterProp.floatValue == 1f) {
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.height = flattenHeightProp.floatValue;
				}
			} else {
				float strength = 0f;
				float brushSizeSqr = Mathf.Pow(brushSizeProp.floatValue, 2f);
				float brushHardCenterSqr = Mathf.Pow(brushHardCenterProp.floatValue, 2f);
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					strength = StrengthFromDistanceSqr(pair.Value, brushSizeSqr, brushHardCenterSqr);
					pair.Key.height = flattenHeightProp.floatValue + (pair.Key.height - flattenHeightProp.floatValue) * (1 - strength); // TODO use height of vertex before brush to avoid incremental change
				}
			}
			TimingDebugger.Stop();
		}

		private void SmoothBrush(Dictionary<Vector2i, VirtualVertex> vertices) {
			TimingDebugger.Start("Smooth");
			Vector2i[] offsetsVerti = { new Vector2i(0, 1), new Vector2i(0, -1) };
			Vector2i[] offsetsHoriz = { new Vector2i(1, 0), new Vector2i(-1, 0) };
			BlurPass(vertices, offsetsVerti); // Vertical Pass
			BlurPass(vertices, offsetsHoriz); // Horizontal Pass
			TimingDebugger.Stop();
		}

		private void BlurPass(Dictionary<Vector2i, VirtualVertex> vertices, Vector2i[] offsets) {
			foreach (KeyValuePair<Vector2i, VirtualVertex> pair in vertices) {
				float height = pair.Value.height;
				float count = 1;
				for (int i = 0; i < offsets.Length; i++) {
					VirtualVertex v;
					if (vertices.TryGetValue(pair.Key + offsets[i], out v)) {
						height += v.height * brushDensityProp.floatValue;
						count += brushDensityProp.floatValue;
					} else {
						v = terrain.virtualMesh.GetVertexAt(pair.Key + offsets[i]);
						if (v != null) {
							height += v.height * brushDensityProp.floatValue;
							count += brushDensityProp.floatValue;
						}
					}
				}
				height /= count;
				pair.Value.height = height;
			}
		}

		private void VertexPaintBrush(Dictionary<VirtualVertex, float> vertices) {
			if (brushHardCenterProp.floatValue == 1f) {
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					pair.Key.color = Color.Lerp(pair.Key.color, brushPaintColorProp.colorValue, brushDensityProp.floatValue);
				}
			} else {
				float brushSizeSqr = Mathf.Pow(brushSizeProp.floatValue, 2f);
				float brushHardCenterSqr = Mathf.Pow(brushHardCenterProp.floatValue, 2f);
				foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
					//pair.Key.color = new Color(Mathf.Max(StrengthFromDistanceSqr(pair.Value, brushSizeSqr, brushHardCenterSqr), pair.Key.color.r), 0f, 0f);
					pair.Key.color = Color.Lerp(pair.Key.color, brushPaintColorProp.colorValue, StrengthFromDistanceSqr(pair.Value, brushSizeSqr, brushHardCenterSqr) * brushDensityProp.floatValue * .25f);
				}
			}
		}
#endregion

#region Inputs
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
#endregion
#endregion

	}
}
