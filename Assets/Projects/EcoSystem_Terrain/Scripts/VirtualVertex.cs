using UnityEngine;

namespace EcoSystem {
	public class VirtualVertex {
		
		public int index { get; private set; }
		protected MeshData data;
		private Vector3 offset;
		public Vector3 vertex {
			get {
				return offset + data.vertices[index];
			}
		}
		public virtual float height {
			get {
				return data.vertices[index].y;
			}
			set {
				data.vertices[index].y = value;
			}
		}
		public virtual Vector3 normal {
			get {
				return data.normals[index];
			}
			set {
				data.normals[index] = value;
			}
		}

		public VirtualVertex(Vector3 offset, int index, MeshData d) {
			this.index = index;
			data = d;
		}

		public virtual void AverageNormals() { }

	}
}
