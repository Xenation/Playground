using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSystem {
	public struct Quad {
		public int itl, itr, ibl, ibr;
		private MeshData data;

		public Vector3 tl { get { return data.vertices[itl]; } }
		public Vector3 tr { get { return data.vertices[itr]; } }
		public Vector3 bl { get { return data.vertices[ibl]; } }
		public Vector3 br { get { return data.vertices[ibr]; } }

		public Color colortl { get { return data.colors[itl]; } }
		public Color colortr { get { return data.colors[itr]; } }
		public Color colorbl { get { return data.colors[ibl]; } }
		public Color colorbr { get { return data.colors[ibr]; } }

		public Quad(MeshData d, int itl, int itr, int ibl, int ibr) {
			data = d;
			this.itl = itl;
			this.itr = itr;
			this.ibl = ibl;
			this.ibr = ibr;
		}
	}

	public struct Triangle {
		public int ip1, ip2, ip3;
		private MeshData data;

		public Vector3 p1 { get { return data.vertices[ip1]; } }
		public Vector3 p2 { get { return data.vertices[ip2]; } }
		public Vector3 p3 { get { return data.vertices[ip3]; } }

		public Color colorp1 { get { return data.colors[ip1]; } }
		public Color colorp2 { get { return data.colors[ip2]; } }
		public Color colorp3 { get { return data.colors[ip3]; } }

		public Triangle(MeshData d, int ip1, int ip2, int ip3) {
			data = d;
			this.ip1 = ip1;
			this.ip2 = ip2;
			this.ip3 = ip3;
		}

		public Vector3 GetBarycentricCoords(Vector2 meshPos) {
			float divider = (p3.z - p2.z) * (p1.x - p2.x) + (p2.x - p3.x) * (p1.z - p2.z);
			float weightP1 = ((p3.z - p2.z) * (meshPos.x - p2.x) + (p2.x - p3.x) * (meshPos.y - p2.z)) / divider;
			float weightP3 = ((p2.z - p1.z) * (meshPos.x - p2.x) + (p1.x - p2.x) * (meshPos.y - p2.z)) / divider;
			float weightP2 = 1f - weightP1 - weightP3;
			return new Vector3(weightP1, weightP2, weightP3);
		}

	}

	[Serializable]
	public class MeshData {

		[HideInInspector]
		[SerializeField]
		public int[] indices;
		[HideInInspector]
		[SerializeField]
		public Vector3[] relativePosition;
		[HideInInspector]
		[SerializeField]
		public Vector3[] vertices;
		[HideInInspector]
		[SerializeField]
		public Color[] colors;
		[HideInInspector]
		[SerializeField]
		public Vector3[] normals;

		public bool isVertexModified = false;
		public bool isNormalModified = false;
		public bool isColorModified = false;

	}

	[Serializable]
	public class ChunkMesh {

		private Mesh mesh;

		[HideInInspector]
		[SerializeField]
		private MeshData data;

		[SerializeField]
		private Vector2 size;
		public Vector2 Size {
			get {
				return size;
			}
		}
		[SerializeField]
		private int quads = 1;

		public ChunkMesh(Vector2 size, int quads) {
			mesh = new Mesh();
			data = new MeshData();
			this.size = size;
			this.quads = quads;
			Resync();
		}

		private void Resync() {
			data.vertices = mesh.vertices;
			data.colors = mesh.colors;
			data.normals = mesh.normals;
			data.indices = mesh.GetIndices(0);
			ResetRelativePositions();
		}

		public void ApplyVertexModifications() {
			if (!data.isVertexModified) return;
			mesh.Clear();
			mesh.vertices = data.vertices;
			mesh.SetIndices(data.indices, MeshTopology.Triangles, 0);
			mesh.RecalculateNormals();
			data.normals = mesh.normals;
			data.isVertexModified = false;
		}

		public void ApplyNormalsModifications() {
			if (!data.isNormalModified) return;
			mesh.normals = data.normals;
			mesh.UploadMeshData(false);
			data.isNormalModified = false;
		}

		public void ApplyColorModifications() {
			if (!data.isColorModified) return;
			mesh.colors = data.colors;
			data.isColorModified = false;
		}

		public void RecreateMesh() {
			mesh = new Mesh();
			data.isVertexModified = true;
			ApplyVertexModifications();
		}

		public void GeneratePlane() {
			mesh.GeneratePlane(size, quads, quads);
			mesh.RecalculateNormals();
			mesh.colors = new Color[mesh.vertexCount];
			Resync();
		}

		public void ResetRelativePositions() {
			data.relativePosition = new Vector3[data.vertices.Length];
			for (int i = 0; i < data.vertices.Length; i++) {
				data.relativePosition[i] = new Vector3(data.vertices[i].x / size.x, 0f, data.vertices[i].z / size.y);
			}
		}

		public void Resize(Vector3 nSize) {
			for (int i = 0; i < data.vertices.Length; i++) {
				data.vertices[i].x = data.relativePosition[i].x * nSize.x;
				data.vertices[i].y = data.relativePosition[i].y * nSize.y;
				data.vertices[i].z = data.relativePosition[i].z * nSize.z;
			}
			size = new Vector2(nSize.x, nSize.z);
			mesh.vertices = data.vertices;
			mesh.RecalculateBounds();
		}

		public void SetResolution(int nQuads) {
			Vector3[] nVertices;
			int[] nIndices;
			MeshHelper.GeneratePlane(out nVertices, out nIndices, size, nQuads, nQuads);
			Color[] nColors = new Color[nVertices.Length];
			Vector2 quadSize = new Vector2(size.x / quads, size.y / quads);
			for (int i = 0; i < nVertices.Length; i++) {
				nVertices[i].y = GetHeightAt(new Vector2(nVertices[i].x, nVertices[i].z), quadSize);
				nColors[i] = GetColorAt(new Vector2(nVertices[i].x, nVertices[i].z), quadSize);
			}
			data.vertices = nVertices;
			data.indices = nIndices;
			data.colors = nColors;
			data.isVertexModified = true;
			data.isColorModified = true;
			ApplyVertexModifications();
			ApplyColorModifications();
			quads = nQuads;
			ResetRelativePositions();
		}

		public float GetHeightAt(Vector2 pos, Vector2 quadSize) {
			Vector2i quadPos = new Vector2i((int) (pos.x / quadSize.x), (int) (pos.y / quadSize.y));
			if (quadPos.x >= quads) {
				quadPos.x = quads - 1;
			}
			if (quadPos.y >= quads) {
				quadPos.y = quads - 1;
			}
			Vector2 quadRealPos = new Vector2(quadPos.x * quadSize.x, quadPos.y * quadSize.y);
			bool isTopTriangle = ((pos.x - quadRealPos.x) < (pos.y - quadRealPos.y) * quadSize.x / quadSize.y);
			Triangle tri = GetTriangleAt(quadPos, isTopTriangle);
			Vector3 barycentricCoords = tri.GetBarycentricCoords(pos);
			return tri.p1.y * barycentricCoords.x + tri.p2.y * barycentricCoords.y + tri.p3.y * barycentricCoords.z;
		}

		public Color GetColorAt(Vector2 pos, Vector2 quadSize) {
			Vector2i quadPos = new Vector2i((int) (pos.x / quadSize.x), (int) (pos.y / quadSize.y));
			if (quadPos.x >= quads) {
				quadPos.x = quads - 1;
			}
			if (quadPos.y >= quads) {
				quadPos.y = quads - 1;
			}
			Vector2 quadRealPos = new Vector2(quadPos.x * quadSize.x, quadPos.y * quadSize.y);
			bool isTopTriangle = ((pos.x - quadRealPos.x) < (pos.y - quadRealPos.y) * quadSize.x / quadSize.y);
			Triangle tri = GetTriangleAt(quadPos, isTopTriangle);
			Vector3 barycentricCoords = tri.GetBarycentricCoords(pos);
			return tri.colorp1 * barycentricCoords.x + tri.colorp2 * barycentricCoords.y + tri.colorp3 * barycentricCoords.z;
		}

		public Quad GetQuadAt(Vector2i quadPos) {
			int bl = quads * quadPos.y + quadPos.y + quadPos.x;
			int br = bl + 1;
			int tl = quads * (quadPos.y + 1) + (quadPos.y + 1) + quadPos.x;
			int tr = tl + 1;
			return new Quad(data, tl, tr, bl, br);
		}

		public Triangle GetTriangleAt(Vector2i quadPos, bool isTop) {
			if (isTop) {
				int bl = quads * quadPos.y + quadPos.y + quadPos.x;
				int tl = quads * (quadPos.y + 1) + (quadPos.y + 1) + quadPos.x;
				int tr = tl + 1;
				return new Triangle(data, tl, tr, bl);
			} else {
				int bl = quads * quadPos.y + quadPos.y + quadPos.x;
				int br = bl + 1;
				int tr = quads * (quadPos.y + 1) + (quadPos.y + 1) + quadPos.x + 1;
				return new Triangle(data, tr, br, bl);
			}
		}
		
		public int[] GetVirtualVertices(ref VirtualVertex[] virtualVertices, int virtVertCountX, Vector2i chkPos, Vector2 chkSize) {
			int vertCount = (quads + 1) * (quads + 1);
			int[] virtualIndices = new int[vertCount];
			int virtIndicesIndex = 0;
			for (int y = 0; y < quads + 1; y++) {
				for (int x = 0; x < quads + 1; x++) {
					//int virtX = chkPos.x * quads + x;
					//int virtY = chkPos.y * quads + y;
					//int virtIndex = virtY * virtVertCountX + virtX;
					int virtIndex = (chkPos.y * quads + y) * virtVertCountX + (chkPos.x * quads + x);
					int localIndex = y * (quads + 1) + x;
					if (virtualVertices[virtIndex] != null) {
						virtualVertices[virtIndex].AddMesh(localIndex, data);
					} else {
						virtualVertices[virtIndex] = new VirtualVertex(new Vector3(chkPos.x * chkSize.x, 0f, chkPos.y * chkSize.y), localIndex, data);
					}
					virtualIndices[virtIndicesIndex++] = virtIndex;
				}
			}
			return virtualIndices;
		}

		public int GetVertexIndex(int vertX, int vertY) {
			return vertY * (quads + 1) + vertX;
		}

		public static implicit operator Mesh(ChunkMesh chkMesh) {
			return chkMesh.mesh;
		}

	}
}
