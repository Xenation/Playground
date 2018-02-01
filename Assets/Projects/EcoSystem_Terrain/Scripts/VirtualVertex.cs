using UnityEngine;

namespace EcoSystem {
	public class VirtualVertex {
		
		public int index { get; private set; }
		//public Vector3 vertex;
		private Vector3[] vertices;
		public Vector3 vertex {
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
