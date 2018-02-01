using UnityEngine;

namespace EcoSystem {
	public class VirtualVertex {
		
		public int index { get; private set; }
		protected Vector3[] vertices;
		public virtual Vector3 vertex {
			get {
				return vertices[index];
			}
			set {
				vertices[index] = value;
			}
		}

		public VirtualVertex(int index, ref Vector3[] verticesArray) {
			this.index = index;
			vertices = verticesArray;
		}

	}
}
