using System.Collections.Generic;
using UnityEngine;

namespace EcoSystem {
	public class ZonePainter : MonoBehaviour {

		public float range = 50f;
		public ETerrain terrain;

		public void Update() {
			if (terrain == null) return;
			Dictionary<VirtualVertex, float> vertices = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(transform.position.x, transform.position.z), range);
			foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
				pair.Key.color = Color.red;
			}
			terrain.virtualMesh.ApplyModifications();
		}

	}
}
