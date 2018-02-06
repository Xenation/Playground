using System.Collections.Generic;
using UnityEngine;

namespace EcoSystem {
	public class VirtualMesh {

		private Dictionary<Vector2i, ChunkData> data = new Dictionary<Vector2i, ChunkData>();
		private Dictionary<ChunkMesh, List<VirtualVertex>> virtualVertices = new Dictionary<ChunkMesh, List<VirtualVertex>>();

		//private VirtualVertex[] vertices;
		//private int[] indices;

		private ETerrain terrain;

		//private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		//private System.Diagnostics.Stopwatch swDist = new System.Diagnostics.Stopwatch();

		public VirtualMesh(ETerrain terr) {
			terrain = terr;
			Refetch(terr.data.chunks);
		}

		private Dictionary<VirtualVertex, float> vertsByDistance = new Dictionary<VirtualVertex, float>();
		public Dictionary<VirtualVertex, float> GetVerticesInRange2D(Vector2 center, float range) {
			// TODO Optimize
			//sw.Start();
			vertsByDistance.Clear();
			float rangeSqr = Mathf.Pow(range, 2f);
			Vector2i[] touching = terrain.ChunkPositionsTouching(new Rect(new Vector2(center.x - range, center.y - range), Vector2.one * range * 2f));
			for (int i = 0; i < touching.Length; i++) {
				ChunkData chkData;
				if (data.TryGetValue(touching[i], out chkData)) {
					foreach (VirtualVertex vert in virtualVertices[chkData.mesh]) {
						//swDist.Start();
						float distanceSqr = (new Vector2(vert.vertex.x, vert.vertex.z) - center).sqrMagnitude; // TODO unsafe quick local to world transform
						//swDist.Stop();
						if (distanceSqr < rangeSqr) {
							if (!vertsByDistance.TryGetValue(vert, out distanceSqr)) {
								vertsByDistance.Add(vert, distanceSqr);
							}
						}
					}
				}
			}
			//Debug.Log("getVerticesInRange2D: " + sw.ElapsedMilliseconds + "ms");
			//Debug.Log("    distanceSqr: " + swDist.ElapsedMilliseconds + "ms");
			//sw.Reset();
			//swDist.Reset();
			return vertsByDistance;
		}

		public void ApplyModifications() {
			foreach (ChunkData chkData in data.Values) {
				chkData.mesh.ApplyModifications();
			}
			FixEdgeSeams();
			foreach (ChunkData chkData in data.Values) {
				chkData.mesh.ApplyNormalsModifications();
			}
		}

		private void FixEdgeSeams() {
			foreach (List<VirtualVertex> verts in virtualVertices.Values) { // TODO avoid re-averaging edge and corner vertices more than once
				foreach (VirtualVertex vert in verts) {
					vert.AverageNormals();
				}
			}
		}

		public void Refetch() {
			Refetch(terrain.data.chunks);
		}

		public void Refetch(ChunkDictionary chunkDict) {
			data.Clear();
			virtualVertices.Clear();
			foreach (ChunkData chk in chunkDict.Values) {
				chk.mesh.ResetVirtualVertices();
			}
			foreach (ChunkData chk in chunkDict.Values) {
				data.Add(chk.pos, chk);
				virtualVertices.Add(chk.mesh, chk.mesh.GetVirtualVertices(new Vector3(chk.pos.x * terrain.chunkSize.x, 0f, chk.pos.y * terrain.chunkSize.y)));
			}
		}

	}
}
