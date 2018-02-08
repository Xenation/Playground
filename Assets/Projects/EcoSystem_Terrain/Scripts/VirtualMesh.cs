using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace EcoSystem {
	public class VirtualMesh {

		//private Dictionary<Vector2i, ChunkData> data = new Dictionary<Vector2i, ChunkData>();
		//private Dictionary<ChunkMesh, List<VirtualVertex>> virtualVertices = new Dictionary<ChunkMesh, List<VirtualVertex>>();

		private Dictionary<Vector2i, int[]> indicesPerChunk = new Dictionary<Vector2i, int[]>();
		private VirtualVertex[] virtualVertices;

		//private VirtualVertex[] vertices;
		//private int[] indices;

		private ETerrain terrain;

		public VirtualMesh(ETerrain terr) {
			terrain = terr;
			Refetch();
		}

		private Dictionary<VirtualVertex, float> vertsByDistance = new Dictionary<VirtualVertex, float>();
		public Dictionary<VirtualVertex, float> GetVerticesByDistanceIn2DRange(Vector2 center, float range) {
			TimingDebugger.Start("GetIn2DRange");
			// TODO Optimize
			vertsByDistance.Clear();
			float rangeSqr = Mathf.Pow(range, 2f);
			float existingDistance = 0f;
			VirtualVertex[] inRect = GetVerticesInRect(center.x - range, center.x + range, center.y - range, center.y + range);
			for (int i = 0; i < inRect.Length; i++) {
				float distanceSqr = (new Vector2(inRect[i].vertex.x, inRect[i].vertex.z) - center).sqrMagnitude;
				if (distanceSqr < rangeSqr) {
					if (!vertsByDistance.TryGetValue(inRect[i], out existingDistance)) {
						vertsByDistance.Add(inRect[i], distanceSqr);
					}
				}
			}
			TimingDebugger.Stop();
			return vertsByDistance;
		}

		private Dictionary<Vector2i, VirtualVertex> vertsByPosition = new Dictionary<Vector2i, VirtualVertex>();
		public Dictionary<Vector2i, VirtualVertex> GetVerticesByPositionIn2DRange(Vector2 center, float range) {
			TimingDebugger.Start("GetByPositionIn2DRange");
			// TODO Optimize
			vertsByPosition.Clear();
			float rangeSqr = Mathf.Pow(range, 2f);
			Dictionary<Vector2i, VirtualVertex> inRect = GetVerticesByPositionInRect(center.x - range, center.x + range, center.y - range, center.y + range);
			foreach (KeyValuePair<Vector2i, VirtualVertex> pair in inRect) {
				if ((new Vector2(pair.Value.vertex.x, pair.Value.vertex.z) - center).sqrMagnitude < rangeSqr) {
					vertsByPosition.Add(pair.Key, pair.Value);
				}
			}
			TimingDebugger.Stop();
			return vertsByPosition;
		}

		public VirtualVertex[] GetVerticesInRect(float minX, float maxX, float minZ, float maxZ) {
			Vector2 quadSize = terrain.chunkSize / terrain.actualQuads;
			int minVertX = Mathf.Max((int) (minX / quadSize.x) + 1, 0);
			int maxVertX = Mathf.Min((int) (maxX / quadSize.x), terrain.actualQuads * terrain.chunksCountX);
			int minVertZ = Mathf.Max((int) (minZ / quadSize.y) + 1, 0);
			int maxVertZ = Mathf.Min((int) (maxZ / quadSize.y), terrain.actualQuads * terrain.chunksCountZ);
			VirtualVertex[] inRect = new VirtualVertex[(maxVertX - minVertX + 1) * (maxVertZ - minVertZ + 1)];
			int index = 0;
			for (int z = minVertZ; z <= maxVertZ; z++) {
				for (int x = minVertX; x <= maxVertX; x++) {
					inRect[index++] = GetVertexAtVertPos(x, z);
				}
			}
			return inRect;
		}

		public Dictionary<Vector2i, VirtualVertex> GetVerticesByPositionInRect(float minX, float maxX, float minZ, float maxZ) {
			Vector2 quadSize = terrain.chunkSize / terrain.actualQuads;
			int minVertX = Mathf.Max((int) (minX / quadSize.x) + 1, 0);
			int maxVertX = Mathf.Min((int) (maxX / quadSize.x), terrain.actualQuads * terrain.chunksCountX);
			int minVertZ = Mathf.Max((int) (minZ / quadSize.y) + 1, 0);
			int maxVertZ = Mathf.Min((int) (maxZ / quadSize.y), terrain.actualQuads * terrain.chunksCountZ);
			Dictionary<Vector2i, VirtualVertex> inRect = new Dictionary<Vector2i, VirtualVertex>();
			for (int z = minVertZ; z <= maxVertZ; z++) {
				for (int x = minVertX; x <= maxVertX; x++) {
					inRect.Add(new Vector2i(x, z), GetVertexAtVertPos(x, z));
				}
			}
			return inRect;
		}

		public VirtualVertex GetVertexAtWorldPos(Vector3 worldPos) { // TODO take in account terrain origin offset
			TimingDebugger.Start("GetVertexAt");
			Vector2 quadSize = terrain.chunkSize / terrain.actualQuads;
			worldPos += new Vector3(quadSize.x / 2f, 0f, quadSize.y / 2f);
			Vector2i quadPos = new Vector2i((int) (worldPos.x / quadSize.x), (int) (worldPos.z / quadSize.y));
			TimingDebugger.Stop();
			if (quadPos.x < 0 || quadPos.y < 0 || quadPos.x > terrain.actualQuads * terrain.chunksCountX || quadPos.y > terrain.actualQuads * terrain.chunksCountZ) return null;
			return GetVertexAtVertPos(quadPos);
		}

		public VirtualVertex GetVertexAt(Vector2i vertPos) {
			if (vertPos.x < 0 || vertPos.x > terrain.actualQuads * terrain.chunksCountX || vertPos.y < 0 || vertPos.y > terrain.actualQuads * terrain.chunksCountZ) return null;
			return GetVertexAtVertPos(vertPos);
		}

		private VirtualVertex GetVertexAtVertPos(Vector2i pos) {
			return GetVertexAtVertPos(pos.x, pos.y);
		}

		private VirtualVertex GetVertexAtVertPos(int x, int z) {
			return virtualVertices[z * (terrain.chunksCountX * terrain.actualQuads + 1) + x];
		}

		public void ApplyModifications() {
			TimingDebugger.Start("Virtual Mesh Apply");
			foreach (ChunkData chkData in terrain.data.chunks.Values) {
				chkData.mesh.ApplyModifications();
			}
			FixEdgeSeams();
			TimingDebugger.Stop();
		}

		private void FixEdgeSeams() {
			TimingDebugger.Start("Fix Edge Seams");
			for (int i = 0; i < virtualVertices.Length; i++) { // TODO avoid re-averaging edge and corner vertices more than once
				if (virtualVertices[i] != null) virtualVertices[i].AverageNormals();
			}
			foreach (ChunkData chkData in terrain.data.chunks.Values) {
				chkData.mesh.ApplyNormalsModifications();
			}
			TimingDebugger.Stop();
		}

		public void Refetch() {
			if (terrain.data.chunks.Count == 0) return;
			TimingDebugger.Start("Virtual Refetch");
			indicesPerChunk.Clear();
			virtualVertices = new VirtualVertex[(terrain.chunksCountX * terrain.actualQuads + 1) * (terrain.chunksCountZ * terrain.actualQuads + 1)];
			foreach (ChunkData chk in terrain.data.chunks.Values) {
				indicesPerChunk.Add(chk.pos, chk.mesh.GetVirtualVertices(ref virtualVertices, terrain.chunksCountX * terrain.actualQuads + 1, chk.pos, terrain.chunkSize));
			}
			int nullCount = 0;
			foreach (VirtualVertex vert in virtualVertices) {
				if (vert == null) nullCount++;
			}
			FixEdgeSeams();
			TimingDebugger.Stop();
		}

	}
}
