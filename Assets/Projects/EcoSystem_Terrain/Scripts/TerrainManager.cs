using System.Collections.Generic;
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
		}

		private void Update() {
			terrain.virtualMesh.UpdateColorChannels();
		}

		private void LateUpdate() {
			terrain.virtualMesh.ApplyColorModifications();
		}

		private float DummyUpdateTemperature(Vector3 pos) {
			return pos.y / 100f;
		}
		
		private float DummyUpdateHumidity(Vector3 pos) {
			return (50f - pos.y) / 30f;
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

		public void SetTemperatureAt(Vector3 pos, float temp) {
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				vert.SetChannelR(temp);
			}
		}

		public void IncreaseTemperatureAt(Vector3 pos, float temp) {
			TimingDebugger.Start("Increase Temperature At");
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				vert.AddChannelR(temp);
			}
			TimingDebugger.Stop();
		}

		public void IncreaseTemperatureCircleLinear(Vector3 center, float radius, float temp) {
			TimingDebugger.Start("Increase Temperature Circle");
			float radiusSqr = Mathf.Pow(radius, 2f);
			Dictionary<VirtualVertex, float> vertices = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(center.x, center.z), radius);
			foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
				pair.Key.AddChannelR(temp * (1f - pair.Value / radiusSqr));
			}
			TimingDebugger.Stop();
		}

	}
}
