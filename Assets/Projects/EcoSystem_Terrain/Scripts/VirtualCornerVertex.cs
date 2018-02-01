using UnityEngine;

namespace EcoSystem {
	public class VirtualCornerVertex : VirtualEdgeVertex {

		public int thirdIndex { get; private set; }
		protected Vector3[] thirdVertices;
		public int fourthIndex { get; private set; }
		protected Vector3[] fourthVertices;
		public override Vector3 vertex {
			get {
				return vertices[index];
			}
			set {
				vertices[index] = value;
				secondVertices[secondIndex] = value;
				thirdVertices[thirdIndex] = value;
				fourthVertices[fourthIndex] = value;
			}
		}

		public VirtualCornerVertex(int index, ref Vector3[] verticesArray, int secondIndex, ref Vector3[] secondArray, int thirdIndex, ref Vector3[] thirdArray, int fourthIndex, ref Vector3[] fourthArray) : base(index, ref verticesArray, secondIndex, ref secondArray) {
			this.thirdIndex = thirdIndex;
			thirdVertices = thirdArray;
			this.fourthIndex = fourthIndex;
			fourthVertices = fourthArray;
		}

	}
}
