using System.Collections.Generic;
using UnityEngine;

namespace EcoSystem {
	public class ZonePainter : MonoBehaviour {

		public float range = 50f;
		public ETerrain terrain;

		public void Update() {
			if (terrain == null) return;
			float temp = TerrainManager.I.GetTemperatureAt(transform.position);
			TerrainManager.I.SetTemperatureCircleLinearNoDecrease(transform.position, range, 3f);
		}

	}
}
