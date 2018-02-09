﻿using System;
using System.Collections.Generic;
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

		public bool isModified = false;
		public bool isNormalModified = false;

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

		public void ApplyModifications() {
			if (!data.isModified) return;
			mesh.Clear();
			mesh.vertices = data.vertices;
			mesh.colors = data.colors;
			mesh.SetIndices(data.indices, MeshTopology.Triangles, 0);
			mesh.RecalculateNormals();
			data.normals = mesh.normals;
			data.isModified = false;
		}

		public void ApplyNormalsModifications() {
			if (!data.isNormalModified) return;
			mesh.normals = data.normals;
			mesh.UploadMeshData(false);
			data.isNormalModified = false;
		}

		public void RecreateMesh() {
			mesh = new Mesh();
			data.isModified = true;
			ApplyModifications();
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
			Vector2 quadSize = new Vector2(size.x / quads, size.y / quads);
			for (int i = 0; i < nVertices.Length; i++) {
				nVertices[i].y = GetHeightAt(new Vector2(nVertices[i].x, nVertices[i].z), quadSize);
			}
			data.vertices = nVertices;
			data.indices = nIndices;
			ApplyModifications();
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
			return new Quad(data.vertices[tl], data.vertices[tr], data.vertices[bl], data.vertices[br]);
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
