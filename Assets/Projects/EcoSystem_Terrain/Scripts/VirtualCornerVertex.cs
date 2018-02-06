using UnityEngine;

namespace EcoSystem {
	public class VirtualCornerVertex : VirtualEdgeVertex {

		public int thirdIndex { get; private set; }
		protected MeshData thirdData;
		public int fourthIndex { get; private set; }
		protected MeshData fourthData;
		public override float height {
			get {
				return data.vertices[index].y;
			}
			set {
				data.vertices[index].y = value;
				secondData.vertices[secondIndex].y = value;
				thirdData.vertices[thirdIndex].y = value;
				fourthData.vertices[fourthIndex].y = value;
			}
		}
		public override Vector3 normal {
			get {
				return data.normals[index];
			}
			set {
				data.normals[index] = value;
				secondData.normals[secondIndex] = value;
				thirdData.normals[thirdIndex] = value;
				fourthData.normals[fourthIndex] = value;
			}
		}

		public VirtualCornerVertex(Vector3 offset, int firstIndex, MeshData firstD, int secondIndex, MeshData secondD, int thirdIndex, MeshData thirdD, int fourthIndex, MeshData fourthD) : base(offset, firstIndex, firstD, secondIndex, secondD) {
			this.thirdIndex = thirdIndex;
			thirdData = thirdD;
			this.fourthIndex = fourthIndex;
			fourthData = fourthD;
		}

		public new void AverageNormals() {
			normal = Vector3.Slerp(Vector3.Slerp(data.normals[index], secondData.normals[secondIndex], .5f), Vector3.Slerp(thirdData.normals[thirdIndex], fourthData.normals[fourthIndex], .5f), .5f);
		}

	}
}
