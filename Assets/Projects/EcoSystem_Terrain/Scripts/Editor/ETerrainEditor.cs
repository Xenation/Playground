using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EcoSystem {
	[CustomEditor(typeof(ETerrain))]
	public class ETerrainEditor : Editor {

		private ETerrain terrain;

		// General
		private SerializedProperty terrainDataProp;
		private SerializedProperty terrainMaterialProp;
		private SerializedProperty terrainSizeProp;
		// Chunks
		private SerializedProperty chunksCountXProp;
		private SerializedProperty chunksCountZProp;
		private SerializedProperty chunkQuadsProp;

		public void OnEnable() {
			terrain = (ETerrain) serializedObject.targetObject;
			terrain.LoadData(); // TODO Load at scene load instead of selection of terrain
			InitProperties();
		}

		private void InitProperties() {
			terrainDataProp = serializedObject.FindProperty("data");
			terrainMaterialProp = serializedObject.FindProperty("terrainMaterial");
			terrainSizeProp = serializedObject.FindProperty("terrainSize");
			chunksCountXProp = serializedObject.FindProperty("chunksCountX");
			chunksCountZProp = serializedObject.FindProperty("chunksCountZ");
			chunkQuadsProp = serializedObject.FindProperty("chunkQuads");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

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

			serializedObject.ApplyModifiedProperties();
		}

		private void OnUndoRedo() {

		}
		
	}
}
