using System.Collections.Generic;
using UnityEngine;

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
		public Dictionary<VirtualVertex, float> GetVerticesInRange2D(Vector2 center, float range) {
			// TODO Optimize
			vertsByDistance.Clear();
			float rangeSqr = Mathf.Pow(range, 2f);
			float existingDistance = 0f;
			Vector2i[] touching = terrain.ChunkPositionsTouching(new Rect(new Vector2(center.x - range, center.y - range), Vector2.one * range * 2f));
			for (int i = 0; i < touching.Length; i++) {
				int[] affectedVertices;
				if (indicesPerChunk.TryGetValue(touching[i], out affectedVertices)) {
					for (int affVertIndex = 0; affVertIndex < affectedVertices.Length; affVertIndex++) {
						float distanceSqr = (new Vector2(virtualVertices[affectedVertices[affVertIndex]].vertex.x, virtualVertices[affectedVertices[affVertIndex]].vertex.z) - center).sqrMagnitude; // TODO unsafe quick local to world transform
						if (distanceSqr < rangeSqr) {
							if (!vertsByDistance.TryGetValue(virtualVertices[affectedVertices[affVertIndex]], out existingDistance)) {
								vertsByDistance.Add(virtualVertices[affectedVertices[affVertIndex]], distanceSqr);
							}
						}
					}
				}
			}
			return vertsByDistance;
		}

		public void ApplyModifications() {
			foreach (ChunkData chkData in terrain.data.chunks.Values) {
				chkData.mesh.ApplyModifications();
			}
			FixEdgeSeams();
		}

		private void FixEdgeSeams() {
			for (int i = 0; i < virtualVertices.Length; i++) { // TODO avoid re-averaging edge and corner vertices more than once
				if (virtualVertices[i] != null) virtualVertices[i].AverageNormals();
			}
			foreach (ChunkData chkData in terrain.data.chunks.Values) {
				chkData.mesh.ApplyNormalsModifications();
			}
		}

		public void Refetch() {
			if (terrain.data.chunks.Count == 0) return;
			indicesPerChunk.Clear();
			virtualVertices = new VirtualVertex[(terrain.chunksCountX * terrain.actualQuads + 1) * (terrain.chunksCountZ * terrain.actualQuads + 1)];
			Debug.Log("virtualVertices.Length = " + virtualVertices.Length);
			foreach (ChunkData chk in terrain.data.chunks.Values) {
				indicesPerChunk.Add(chk.pos, chk.mesh.GetVirtualVertices(ref virtualVertices, terrain.chunksCountX * terrain.actualQuads + 1, chk.pos, terrain.chunkSize));
			}
			int nullCount = 0;
			foreach (VirtualVertex vert in virtualVertices) {
				if (vert == null) nullCount++;
			}
			Debug.Log("nullCount = " + nullCount);
			FixEdgeSeams();
		}

	}
}
