using UnityEngine;
using Xenon;

namespace EcoSystem {
	[RequireComponent(typeof(ETerrain))]
	public class TerrainManager : Singleton<TerrainManager> {

		private ETerrain _terrain;
		private ETerrain terrain {
			get {
				if (_terrain == null) {
					_terrain = GetComponent<ETerrain>();
				}
				return _terrain;
			}
		}
		
		public VirtualMesh.UpdateHeatmap updateHumidity {
			get {
				return terrain.virtualMesh.updateB;
			}
			set {
				terrain.virtualMesh.updateB = value;
			}
		}
		public VirtualMesh.UpdateHeatmap updateTemperature {
			get {
				return terrain.virtualMesh.updateR;
			}
			set {
				terrain.virtualMesh.updateR = value;
			}
		}

		private void Start() {
			updateTemperature = DummyUpdateTemperature;
			updateHumidity = DummyUpdateHumidity;
			terrain.virtualMesh.updateG = DummyG;
			terrain.virtualMesh.updateA = DummyA;
		}

		private void Update() {
			terrain.virtualMesh.UpdateColorChannels();
		}

		private float DummyUpdateTemperature(Vector3 pos) {
			return pos.y / 100f;
		}

		private float DummyUpdateHumidity(Vector3 pos) {
			return (50f - pos.y) / 30f;
		}

		private float DummyG(Vector3 pos) {
			return 0f;
		}

		private float DummyA(Vector3 pos) {
			return 0f;
		}

		/// <summary>
		/// Returns the height of the terrain at the given world position
		/// </summary>
		/// <param name="pos">the world position used to find the height (only x,z used)</param>
		/// <returns>the height if the given position was in terrain bounds, -1000 otherwise</returns>
		public float GetHeightAt(Vector3 pos) {
			return terrain.GetHeightAt(pos);
		}

		/// <summary>
		/// Returns the local humidity at the given world position
		/// </summary>
		/// <param name="pos">the world position used to find the local humidity (only x,z used)</param>
		/// <returns>the humidity if the given position was in terrain bounds, -1 otherwise</returns>
		public float GetHumidityAt(Vector3 pos) { // Humidity encoded in B channel
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				return vert.color.b;
			} else {
				return -1f;
			}
		}

		/// <summary>
		/// Returns the local temperature at the given world position
		/// </summary>
		/// <param name="pos">the world position used to find the local temperature (only x,z used)</param>
		/// <returns>the temperature if the given position was in terrain bounds, -1 otherwise</returns>
		public float GetTemperatureAt(Vector3 pos) { // Temperature encoded in R channel
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				return vert.color.r;
			} else {
				return -1f;
			}
		}

	}
}
