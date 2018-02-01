using UnityEngine;

namespace EcoSystem {
	public class VirtualEdgeVertex : VirtualVertex {

		public int secondIndex { get; private set; }
		protected Vector3[] secondVertices;
		public override Vector3 vertex {
			get {
				return vertices[index];
			}
			set {
				vertices[index] = value;
				secondVertices[secondIndex] = value;
			}
		}

		public VirtualEdgeVertex(int index, ref Vector3[] verticesArray, int secondIndex, ref Vector3[] secondArray) : base(index, ref verticesArray) {
			this.secondIndex = secondIndex;
			secondVertices = secondArray;
		}

	}
}
