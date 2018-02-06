using UnityEngine;

namespace EcoSystem {
	public class VirtualEdgeVertex : VirtualVertex {

		public int secondIndex { get; private set; }
		protected MeshData secondData;
		public override float height {
			get {
				return data.vertices[index].y;
			}
			set {
				data.vertices[index].y = value;
				secondData.vertices[secondIndex].y = value;
			}
		}
		public override Vector3 normal {
			get {
				return data.normals[index];
			}
			set {
				data.normals[index] = value;
				secondData.normals[secondIndex] = value;
			}
		}

		public VirtualEdgeVertex(Vector3 offset, int firstIndex, MeshData firstD, int secondIndex, MeshData secD) : base(offset, firstIndex, firstD) {
			this.secondIndex = secondIndex;
			secondData = secD;
		}

		public new void AverageNormals() {
			normal = Vector3.Slerp(data.normals[index], secondData.normals[secondIndex], .5f);
		}

	}
}
