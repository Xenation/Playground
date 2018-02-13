using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace EcoSystem {
	public class VirtualMesh {

		#region Attributes
		private Dictionary<Vector2i, int[]> indicesPerChunk = new Dictionary<Vector2i, int[]>();
		private VirtualVertex[] virtualVertices;
		public int VirtualVertexCount {
			get {
				if (virtualVertices == null) return 0;
				return virtualVertices.Length;
			}
		}

		private List<Vector2i> modifiedChunks = new List<Vector2i>();

		private ETerrain terrain;

		public delegate float InitializeHeatmap(Vector3 pos);
		public InitializeHeatmap initR;
		public InitializeHeatmap initG;
		public InitializeHeatmap initB;
		public InitializeHeatmap initA;

		public delegate float UpdateHeatmap(Vector3 pos, float val);
		public UpdateHeatmap updateR;
		public UpdateHeatmap updateG;
		public UpdateHeatmap updateB;
		public UpdateHeatmap updateA;
		#endregion

		#region Initialisation
		public VirtualMesh(ETerrain terr) {
			terrain = terr;
			Refetch();
		}
		#endregion

		#region Access
		#region VirtualVertex
		private Dictionary<VirtualVertex, float> vertsByDistance = new Dictionary<VirtualVertex, float>();
		public Dictionary<VirtualVertex, float> GetVerticesByDistanceIn2DRange(Vector2 center, float range) {
			TimingDebugger.Start("GetByDistanceIn2DRange");
			// TODO Optimize
			vertsByDistance.Clear();
			float rangeSqr = Mathf.Pow(range, 2f);
			float existingDistance = 0f;
			VirtualVertex[] inRect = GetVerticesInRect(center.x - range, center.x + range, center.y - range, center.y + range);
			for (int i = 0; i < inRect.Length; i++) {
				float distanceSqr = (new Vector2(inRect[i].vertex.x + terrain.transform.position.x, inRect[i].vertex.z + terrain.transform.position.z) - center).sqrMagnitude;
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
				if ((new Vector2(pair.Value.vertex.x + terrain.transform.position.x, pair.Value.vertex.z + terrain.transform.position.z) - center).sqrMagnitude < rangeSqr) {
					vertsByPosition.Add(pair.Key, pair.Value);
				}
			}
			TimingDebugger.Stop();
			return vertsByPosition;
		}

		public VirtualVertex[] GetVerticesInRect(float minX, float maxX, float minZ, float maxZ) {
			Vector2 quadSize = terrain.chunkSize / terrain.actualQuads;
			int minVertX = Mathf.Max((int) ((minX - terrain.transform.position.x) / quadSize.x) + 1, 0);
			int maxVertX = Mathf.Min((int) ((maxX - terrain.transform.position.x) / quadSize.x), terrain.actualQuads * terrain.chunksCountX);
			int minVertZ = Mathf.Max((int) ((minZ - terrain.transform.position.z) / quadSize.y) + 1, 0);
			int maxVertZ = Mathf.Min((int) ((maxZ - terrain.transform.position.z) / quadSize.y), terrain.actualQuads * terrain.chunksCountZ);
			if (minVertX > maxVertX || minVertZ > maxVertZ) return new VirtualVertex[0];
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
			int minVertX = Mathf.Max((int) ((minX - terrain.transform.position.x) / quadSize.x) + 1, 0);
			int maxVertX = Mathf.Min((int) ((maxX - terrain.transform.position.x) / quadSize.x), terrain.actualQuads * terrain.chunksCountX);
			int minVertZ = Mathf.Max((int) ((minZ - terrain.transform.position.z) / quadSize.y) + 1, 0);
			int maxVertZ = Mathf.Min((int) ((maxZ - terrain.transform.position.z) / quadSize.y), terrain.actualQuads * terrain.chunksCountZ);
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
			worldPos = worldPos - terrain.transform.position; // to local
			worldPos += new Vector3(quadSize.x / 2f, 0f, quadSize.y / 2f);
			Vector2i quadPos = new Vector2i((int) (worldPos.x / quadSize.x), (int) (worldPos.z / quadSize.y));
			TimingDebugger.Stop();
			if (!terrain.isGenerated || quadPos.x < 0 || quadPos.y < 0 || quadPos.x > terrain.actualQuads * terrain.chunksCountX || quadPos.y > terrain.actualQuads * terrain.chunksCountZ) return null;
			return GetVertexAtVertPos(quadPos);
		}

		public VirtualVertex GetVertexAt(Vector2i vertPos) {
			if (!terrain.isGenerated || vertPos.x < 0 || vertPos.x > terrain.actualQuads * terrain.chunksCountX || vertPos.y < 0 || vertPos.y > terrain.actualQuads * terrain.chunksCountZ) return null;
			return GetVertexAtVertPos(vertPos);
		}

		private VirtualVertex GetVertexAtVertPos(Vector2i pos) {
			return virtualVertices[pos.y * (terrain.chunksCountX * terrain.actualQuads + 1) + pos.x];
		}

		private VirtualVertex GetVertexAtVertPos(int x, int z) {
			return virtualVertices[z * (terrain.chunksCountX * terrain.actualQuads + 1) + x];
		}
		#endregion
		#endregion

		#region Modification
		#region Channels
		public void InitializeColorChannels() {
			TimingDebugger.Start("Initialize Color Channels");
			// Very Unelegant but optimised
			Vector3 pos;
			if (initR == null) {
				if (initG == null) {
					if (initB == null) {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								virtualVertices[i].color = new Color(0f, 0f, 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, 0f, 0f, initA(pos));
							}
						}
					} else {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, 0f, initB(pos), 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, 0f, initB(pos), initA(pos));
							}
						}
					}
				} else {
					if (initB == null) {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, initG(pos), 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, initG(pos), 0f, initA(pos));
							}
						}
					} else {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, initG(pos), initB(pos), 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(0f, initG(pos), initB(pos), initA(pos));
							}
						}
					}
				}
			} else {
				if (initG == null) {
					if (initB == null) {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), 0f, 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), 0f, 0f, initA(pos));
							}
						}
					} else {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), 0f, initB(pos), 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), 0f, initB(pos), initA(pos));
							}
						}
					}
				} else {
					if (initB == null) {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), initG(pos), 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), initG(pos), 0f, initA(pos));
							}
						}
					} else {
						if (initA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), initG(pos), initB(pos));
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								virtualVertices[i].color = new Color(initR(pos), initG(pos), initB(pos), initA(pos));
							}
						}
					}
				}
			}
			TimingDebugger.Stop();
		}

		public void UpdateColorChannels() {
			TimingDebugger.Start("Update Color Channels");
			// Very Unelegant but optimised
			Vector3 pos;
			Color col;
			float dt = Mathf.Clamp01(Time.deltaTime);
			if (updateR == null) {
				if (updateG == null) {
					if (updateB == null) {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								virtualVertices[i].color = new Color(0f, 0f, 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, 0f, 0f, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					} else {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, 0f, col.b + (updateB(pos, col.b) - col.b) * dt, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, 0f, col.b + (updateB(pos, col.b) - col.b) * dt, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					}
				} else {
					if (updateB == null) {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, col.g + (updateG(pos, col.g) - col.g) * dt, 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, col.g + (updateG(pos, col.g) - col.g) * dt, 0f, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					} else {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, col.g + (updateG(pos, col.g) - col.g) * dt, col.b + (updateB(pos, col.b) - col.b) * dt, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(0f, col.g + (updateG(pos, col.g) - col.g) * dt, col.b + (updateB(pos, col.b) - col.b) * dt, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					}
				}
			} else {
				if (updateG == null) {
					if (updateB == null) {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, 0f, 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, 0f, 0f, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					} else {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, 0f, col.b + (updateB(pos, col.b) - col.b) * dt, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, 0f, col.b + (updateB(pos, col.b) - col.b) * dt, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					}
				} else {
					if (updateB == null) {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, col.g + (updateG(pos, col.g) - col.g) * dt, 0f, 0f);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, col.g + (updateG(pos, col.g) - col.g) * dt, 0f, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					} else {
						if (updateA == null) {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, col.g + (updateG(pos, col.g) - col.g) * dt, col.b + (updateB(pos, col.b) - col.b) * dt);
							}
						} else {
							for (int i = 0; i < virtualVertices.Length; i++) {
								pos = virtualVertices[i].vertex;
								col = virtualVertices[i].color;
								virtualVertices[i].color = new Color(col.r + (updateR(pos, col.r) - col.r) * dt, col.g + (updateG(pos, col.g) - col.g) * dt, col.b + (updateB(pos, col.b) - col.b) * dt, col.a + (updateA(pos, col.a) - col.a) * dt);
							}
						}
					}
				}
			}
			TimingDebugger.Stop();
		}
		#endregion

		public void ApplyModifications() {
			TimingDebugger.Start("Virtual Mesh Apply");
			foreach (ChunkData chkData in terrain.data.chunks.Values) {
				chkData.mesh.ApplyVertexModifications();
				chkData.mesh.ApplyColorModifications();
			}
			TimingDebugger.Stop();
		}

		public void ApplyColorModifications() {
			TimingDebugger.Start("Virtual Mesh Apply Colors");
			foreach (ChunkData chkData in terrain.data.chunks.Values) {
				chkData.mesh.ApplyColorModifications();
			}
			TimingDebugger.Stop();
		}

		public void FixEdgeSeams() {
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
			Refetch(terrain.chunksCountX, terrain.chunksCountZ, terrain.chunkSize);
		}

		public void Refetch(int chunkCountX, int chunkCountZ, Vector2 chunkSize) {
			if (terrain.data.chunks.Count == 0) return;
			TimingDebugger.Start("Virtual Refetch");
			indicesPerChunk.Clear();
			virtualVertices = new VirtualVertex[(chunkCountX * terrain.actualQuads + 1) * (chunkCountZ * terrain.actualQuads + 1)];
			foreach (ChunkData chk in terrain.data.chunks.Values) {
				indicesPerChunk.Add(chk.pos, chk.mesh.GetVirtualVertices(ref virtualVertices, chunkCountX * terrain.actualQuads + 1, chk.pos, chunkSize));
			}
			int nullCount = 0;
			foreach (VirtualVertex vert in virtualVertices) {
				if (vert == null) nullCount++;
			}
			FixEdgeSeams();
			TimingDebugger.Stop();
		}
		#endregion

	}
}
