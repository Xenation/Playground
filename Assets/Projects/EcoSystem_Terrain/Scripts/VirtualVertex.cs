using UnityEngine;

namespace EcoSystem {
	public class VirtualVertex {

		#region Attributes
		private Vector3 offset;

		private int[] indices;
		private MeshData[] meshesData;

		#region Properties
		public Vector3 vertex {
			get {
				return offset + meshesData[0].vertices[indices[0]];
			}
		}
		public float height {
			get {
				return meshesData[0].vertices[indices[0]].y;
			}
			set {
				for (int i = 0; i < meshesData.Length; i++) {
					meshesData[i].vertices[indices[i]].y = value;
					meshesData[i].isVertexModified = true;
				}
			}
		}
		public Vector3 normal {
			get {
				return meshesData[0].normals[indices[0]];
			}
			set {
				for (int i = 0; i < meshesData.Length; i++) {
					meshesData[i].normals[indices[i]] = value;
					meshesData[i].isNormalModified = true;
				}
			}
		}
		public Color color {
			get {
				return meshesData[0].colors[indices[0]];
			}
			set {
				for (int i = 0; i < meshesData.Length; i++) {
					meshesData[i].colors[indices[i]] = value;
					meshesData[i].isColorModified = true;
				}
			}
		}
		#endregion
		#endregion

		#region Initialization
		public VirtualVertex(Vector3 offset, int index, MeshData data) {
			this.offset = offset;
			indices = new int[1];
			indices[0] = index;
			meshesData = new MeshData[1];
			meshesData[0] = data;
		}

		public void AddMesh(int index, MeshData data) {
			int[] nIndices = new int[indices.Length + 1];
			for (int i = 0; i < indices.Length; i++) {
				nIndices[i] = indices[i];
			}
			nIndices[indices.Length] = index;
			indices = nIndices;
			MeshData[] nMeshesData = new MeshData[meshesData.Length + 1];
			for (int i = 0; i < meshesData.Length; i++) {
				nMeshesData[i] = meshesData[i];
			}
			nMeshesData[meshesData.Length] = data;
			meshesData = nMeshesData;
		}
		#endregion

		#region MeshModification
		public void AverageNormals() {
			if (indices.Length < 2) return;
			Vector3 avgNormal = meshesData[0].normals[indices[0]];
			for (int i = 1; i < indices.Length; i++) {
				avgNormal += meshesData[i].normals[indices[i]];
			}
			avgNormal.Normalize();
			for (int i = 0; i < indices.Length; i++) {
				meshesData[i].normals[indices[i]] = avgNormal;
				meshesData[i].isNormalModified = true;
			}
		}

		#region ColorChannels
		public void SetChannel(int chaIndex, float v) {
			for (int i = 0; i < meshesData.Length; i++) {
				meshesData[i].colors[indices[i]][chaIndex] = v;
				meshesData[i].isColorModified = true;
			}
		}

		public void SetChannelR(float r) {
			SetChannel(0, r);
		}

		public void SetChannelG(float g) {
			SetChannel(1, g);
		}

		public void SetChannelB(float b) {
			SetChannel(2, b);
		}

		public void SetChannelA(float a) {
			SetChannel(3, a);
		}

		public void AddChannel(int chaIndex, float v) {
			for (int i = 0; i < meshesData.Length; i++) {
				meshesData[i].colors[indices[i]][chaIndex] += v;
				meshesData[i].isColorModified = true;
			}
		}

		public void AddChannelR(float r) {
			AddChannel(0, r);
		}

		public void AddChannelG(float g) {
			AddChannel(1, g);
		}

		public void AddChannelB(float b) {
			AddChannel(2, b);
		}

		public void AddChannelA(float a) {
			AddChannel(3, a);
		}
		#endregion
		#endregion

		#region Others
		public override string ToString() {
			return "Virtual Vertex (" + indices.Length + "):\n  vertex: " + vertex + "\n  normal: " + normal + "\n  color: " + color;
		}
		#endregion

	}
}
