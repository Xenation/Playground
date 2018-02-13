using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace EcoSystem {
	[RequireComponent(typeof(ETerrain))]
	public class TerrainManager : Singleton<TerrainManager> {

		#region Attributes
		private ETerrain _terrain;
		private ETerrain terrain {
			get {
				if (_terrain == null) {
					_terrain = GetComponent<ETerrain>();
				}
				return _terrain;
			}
		}

		private float dt;
		#endregion

		#region Delegates
		/// <summary>
		/// Function called when updating humidity
		/// </summary>
		public VirtualMesh.UpdateHeatmap updateHumidity {
			get {
				return terrain.virtualMesh.updateB;
			}
			set {
				terrain.virtualMesh.updateB = value;
			}
		}
		/// <summary>
		/// Function called when updating temperature
		/// </summary>
		public VirtualMesh.UpdateHeatmap updateTemperature {
			get {
				return terrain.virtualMesh.updateR;
			}
			set {
				terrain.virtualMesh.updateR = value;
			}
		}
		/// <summary>
		/// Function called when initializing humidity
		/// </summary>
		public VirtualMesh.InitializeHeatmap initializeHumidity {
			get {
				return terrain.virtualMesh.initB;
			}
			set {
				terrain.virtualMesh.initB = value;
			}
		}
		/// <summary>
		/// Function called when initializing temperature
		/// </summary>
		public VirtualMesh.InitializeHeatmap initializeTemperature {
			get {
				return terrain.virtualMesh.initR;
			}
			set {
				terrain.virtualMesh.initR = value;
			}
		}
		#endregion

		#region Initialization
		private void Start() {
			if (updateTemperature == null) {
				updateTemperature = DummyUpdateTemperature;
			}
			if (updateHumidity == null) {
				updateHumidity = DummyUpdateHumidity;
			}
			if (initializeTemperature == null) {
				initializeTemperature = DummyInitializeTemperature;
			}
			if (initializeHumidity == null) {
				initializeHumidity = DummyInitializeHumidity;
			}
			terrain.virtualMesh.InitializeColorChannels();
			terrain.virtualMesh.ApplyColorModifications();
			//Debug.Log("TerrainManager Start");
		}

		private float DummyInitializeTemperature(Vector3 pos) {
			return pos.y / 100f;
		}

		private float DummyInitializeHumidity(Vector3 pos) {
			return (50f - pos.y) / 30f;
		}
		#endregion

		#region Update
		private void Update() {
			dt = Time.deltaTime;
			terrain.virtualMesh.UpdateColorChannels();
		}

		private void LateUpdate() {
			terrain.virtualMesh.ApplyColorModifications();
		}

		private float DummyUpdateTemperature(Vector3 pos, float temperature) {
			return temperature + (pos.y / 100f - temperature) * Mathf.Clamp01(dt);
		}
		
		private float DummyUpdateHumidity(Vector3 pos, float humidity) {
			return humidity;
		}
		#endregion

		#region Access
		#region Height
		/// <summary>
		/// Returns the height of the terrain at the given world position
		/// </summary>
		/// <param name="pos">the world position used to find the height (only x,z used)</param>
		/// <returns>the height if the given position was in terrain bounds, -1000 otherwise</returns>
		public float GetHeightAt(Vector3 pos) {
			return terrain.GetHeightAt(pos);
		}
		#endregion

		#region Humidity
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
		/// Sets the humidity value of the vertex closest to pos
		/// </summary>
		/// <param name="pos">the position used to find the closest vertex (only x,z used)</param>
		/// <param name="humidity">the new humidity value of the vertex</param>
		public void SetHumidityAt(Vector3 pos, float humidity) {
			TimingDebugger.Start("Set Temperature At");
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				vert.SetChannelB(humidity);
			}
			TimingDebugger.Stop();
		}

		/// <summary>
		/// Sets the humidity value in a circle using a linear falloff and avoids to setting a humidity value lower than existing
		/// </summary>
		/// <param name="center">The center of the circle (only x,z used)</param>
		/// <param name="radius">The radius of the circle</param>
		/// <param name="humidity">The humidity at the center of the circle</param>
		public void SetHumidityCircleLinearNoDecrease(Vector3 center, float radius, float humidity) {
			TimingDebugger.Start("Set Temperature Circle Linear");
			float radiusSqr = Mathf.Pow(radius, 2f);
			Dictionary<VirtualVertex, float> vertices = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(center.x, center.z), radius);
			float nHum;
			foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
				nHum = humidity * (1f - pair.Value / radiusSqr);
				if (pair.Key.color.r > nHum) continue;
				pair.Key.SetChannelB(nHum);
			}
			TimingDebugger.Stop();
		}

		/// <summary>
		/// Increases the humidity value of the vertex closest to pos
		/// </summary>
		/// <param name="pos">the position used to find the closest vertex (only x,z used)</param>
		/// <param name="humidity">the increase in humidity to apply</param>
		public void IncreaseHumidityAt(Vector3 pos, float humidity) {
			TimingDebugger.Start("Increase Temperature At");
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				vert.AddChannelB(humidity);
			}
			TimingDebugger.Stop();
		}

		/// <summary>
		/// Increases the humidity value in a circle using linear falloff
		/// </summary>
		/// <param name="center">The center of the circle (only x,z used)</param>
		/// <param name="radius">The radius of the circle</param>
		/// <param name="humidity">The humidity increase at the center of the circle</param>
		public void IncreaseHumidityCircleLinear(Vector3 center, float radius, float humidity) {
			TimingDebugger.Start("Increase Temperature Circle Linear");
			float radiusSqr = Mathf.Pow(radius, 2f);
			Dictionary<VirtualVertex, float> vertices = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(center.x, center.z), radius);
			foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
				pair.Key.AddChannelB(humidity * (1f - pair.Value / radiusSqr));
			}
			TimingDebugger.Stop();
		}
		#endregion

		#region Temperature
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

		/// <summary>
		/// Sets the temperature value of the vertex closest to pos
		/// </summary>
		/// <param name="pos">the position used to find the closest vertex (only x,z used)</param>
		/// <param name="humidity">the new temperature value of the vertex</param>
		public void SetTemperatureAt(Vector3 pos, float temperature) {
			TimingDebugger.Start("Set Temperature At");
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				vert.SetChannelR(temperature);
			}
			TimingDebugger.Stop();
		}

		/// <summary>
		/// Sets the temperature value in a circle using a linear falloff and avoids to setting a temperature value lower than existing
		/// </summary>
		/// <param name="center">The center of the circle (only x,z used)</param>
		/// <param name="radius">The radius of the circle</param>
		/// <param name="humidity">The temperature at the center of the circle</param>
		public void SetTemperatureCircleLinearNoDecrease(Vector3 center, float radius, float temperature) {
			TimingDebugger.Start("Set Temperature Circle Linear");
			float radiusSqr = Mathf.Pow(radius, 2f);
			Dictionary<VirtualVertex, float> vertices = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(center.x, center.z), radius);
			float nTemp;
			foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
				nTemp = temperature * (1f - pair.Value / radiusSqr);
				if (pair.Key.color.r > nTemp) continue;
				pair.Key.SetChannelR(nTemp);
			}
			TimingDebugger.Stop();
		}

		/// <summary>
		/// Increases the temperature value of the vertex closest to pos
		/// </summary>
		/// <param name="pos">the position used to find the closest vertex (only x,z used)</param>
		/// <param name="humidity">the increase in temperature to apply</param>
		public void IncreaseTemperatureAt(Vector3 pos, float temperature) {
			TimingDebugger.Start("Increase Temperature At");
			VirtualVertex vert = terrain.virtualMesh.GetVertexAtWorldPos(pos);
			if (vert != null) {
				vert.AddChannelR(temperature);
			}
			TimingDebugger.Stop();
		}

		/// <summary>
		/// Increases the temperature value in a circle using linear falloff
		/// </summary>
		/// <param name="center">The center of the circle (only x,z used)</param>
		/// <param name="radius">The radius of the circle</param>
		/// <param name="humidity">The temperature increase at the center of the circle</param>
		public void IncreaseTemperatureCircleLinear(Vector3 center, float radius, float temperature) {
			TimingDebugger.Start("Increase Temperature Circle Linear");
			float radiusSqr = Mathf.Pow(radius, 2f);
			Dictionary<VirtualVertex, float> vertices = terrain.virtualMesh.GetVerticesByDistanceIn2DRange(new Vector2(center.x, center.z), radius);
			foreach (KeyValuePair<VirtualVertex, float> pair in vertices) {
				pair.Key.AddChannelR(temperature * (1f - pair.Value / radiusSqr));
			}
			TimingDebugger.Stop();
		}
		#endregion
		#endregion

	}
}
