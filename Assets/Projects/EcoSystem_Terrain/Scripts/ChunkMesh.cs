using System;
using UnityEngine;

namespace EcoSystem {
	public struct Quad {
		public Vector3 tl, tr, bl, br;

		public Quad(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br) {
			this.tl = tl;
			this.tr = tr;
			this.bl = bl;
			this.br = br;
		}
	}

	[Serializable]
	public class ChunkMesh {

		private Mesh mesh;

		private int[] indices;
		private Vector3[] relativePosition;
		private Vector3[] vertices;
		private Color[] colors;

		private Vector2 size;
		public Vector2 Size {
			get {
				return size;
			}
		}
		private int quads = 1;

		public ChunkMesh(Vector2 size, int quads) {
			mesh = new Mesh();
			this.size = size;
			this.quads = quads;
			Resync();
		}

		private void Resync() {
			vertices = mesh.vertices;
			colors = mesh.colors;
			indices = mesh.GetIndices(0);
		}

		private void ApplyModifications() {
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.colors = colors;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		}

		public void RecreateMesh() {
			mesh = new Mesh();
			ApplyModifications();
		}

		public void GeneratePlane() {
			mesh.GeneratePlane(size, quads, quads);
			Resync();
			ResetRelativePositions();
		}

		public void ResetRelativePositions() {
			relativePosition = new Vector3[vertices.Length];
			for (int i = 0; i < vertices.Length; i++) {
				relativePosition[i] = new Vector3(vertices[i].x / size.x, 0f, vertices[i].z / size.y);
			}
		}

		public void Resize(Vector3 nSize) {
			for (int i = 0; i < vertices.Length; i++) {
				vertices[i].x = relativePosition[i].x * nSize.x;
				vertices[i].y = relativePosition[i].y * nSize.y;
				vertices[i].z = relativePosition[i].z * nSize.z;
			}
			size = new Vector2(nSize.x, nSize.z);
			mesh.vertices = vertices;
			mesh.RecalculateBounds();
		}

		public void SetResolution(int nQuads) {
			Vector3[] nVertices;
			int[] nIndices;
			MeshHelper.GeneratePlane(out nVertices, out nIndices, size, nQuads, nQuads);
			//Debug.Log("Setting Resolution ...");
			for (int i = 0; i < nVertices.Length; i++) {
				nVertices[i].y = GetHeightAt(new Vector2(nVertices[i].x * .99f, nVertices[i].z * .99f)); // TODO shitty fix for top and right edge vertices out of chunk grid bounds
			}
			vertices = nVertices;
			indices = nIndices;
			ApplyModifications();
			quads = nQuads;
		}

		public float GetHeightAt(Vector2 pos) {
			Vector2 quadSize = new Vector2(size.x / quads, size.y / quads);
			Vector2i quadPos = new Vector2i((int) (pos.x / quadSize.x), (int) (pos.y / quadSize.y));
			Vector2 quadRealPos = new Vector2(quadPos.x * quadSize.x, quadPos.y * quadSize.y);
			bool isTopTriangle = ((pos.x - quadRealPos.x) < (pos.y - quadRealPos.y));
			Quad quad = GetQuadAt(quadPos);
			Vector3 lastPoint;
			if (isTopTriangle) {
				lastPoint = quad.tl;
			} else {
				lastPoint = quad.br;
			}
			float divider = (quad.bl.z - lastPoint.z) * (quad.tr.x - lastPoint.x) + (lastPoint.x - quad.bl.x) * (quad.tr.z - lastPoint.z);
			float weightTR = ((quad.bl.z - lastPoint.z) * (pos.x - lastPoint.x) + (lastPoint.x - quad.bl.x) * (pos.y - lastPoint.z)) / divider;
			float weightBL = ((lastPoint.z - quad.tr.z) * (pos.x - lastPoint.x) + (quad.tr.x - lastPoint.x) * (pos.y - lastPoint.z)) / divider;
			float weightLast = 1f - weightTR - weightBL;
			return quad.tr.y * weightTR + quad.bl.y * weightBL + lastPoint.y * weightLast;
		}

		public Quad GetQuadAt(Vector2i quadPos) {
			int bl = quads * quadPos.y + quadPos.y + quadPos.x;
			int br = bl + 1;
			int tl = quads * (quadPos.y + 1) + (quadPos.y + 1) + quadPos.x;
			int tr = tl + 1;
			//Debug.Log("quadPos=" + quadPos);
			//Debug.Log("tl=" + tl + "  tr=" + tr + "  bl=" + bl + "  br=" + br);
			return new Quad(vertices[tl], vertices[tr], vertices[bl], vertices[br]);
		}

		public static implicit operator Mesh(ChunkMesh chkMesh) {
			return chkMesh.mesh;
		}

	}
}
