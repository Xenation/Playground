using UnityEditor;
using UnityEngine;

namespace EcoSystem {
	public class ETerrainData : ScriptableObject {

		public ChunkDictionary chunks = new ChunkDictionary();

		[MenuItem("Assets/Create/EcoSystem TerrainData")]
		public static void CreateTerrainData() {
			ETerrainData data = new ETerrainData();
			AssetDatabase.CreateAsset(data, "Assets/EcoSystem TerrainData.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = data;
		}

	}
}
