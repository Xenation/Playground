using UnityEngine;

namespace EcoSystem {
	public class VirtualVertex {

		private Vector3 offset;

		private int[] indices;
		private MeshData[] meshesData;

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
					meshesData[i].isModified = true;
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
					meshesData[i].isModified = true;
				}
			}
		}

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

		public override string ToString() {
			return "Virtual Vertex (" + indices.Length + "):\n  vertex: " + vertex + "\n  normal: " + normal;
		}

	}
}
