using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSystem {
	[Serializable]
	public class ChunkDictionary : Dictionary<Vector2i, ChunkData>, ISerializationCallbackReceiver {

		[SerializeField]
		private List<Vector2i> keys = new List<Vector2i>();
		[SerializeField]
		private List<ChunkData> values = new List<ChunkData>();

		public void OnBeforeSerialize() {
			keys.Clear();
			values.Clear();
			foreach (KeyValuePair<Vector2i, ChunkData> pair in this) {
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		public void OnAfterDeserialize() {
			Clear();

			if (keys.Count != values.Count) {
				Debug.LogWarning("There are " + keys.Count + " keys and " + values.Count + " after deserialization. Make sure that both key and value types are serializable.");
				//throw new Exception("There are " + keys.Count + " keys and " + values.Count + " after deserialization. Make sure that both key and value types are serializable.");
			}

			for (int i = 0; i < keys.Count; i++) {
				Add(keys[i], values[i]);
			}
		}

	}
}
