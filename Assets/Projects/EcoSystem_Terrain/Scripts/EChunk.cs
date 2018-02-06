using System.Collections.Generic;
using UnityEngine;

namespace EcoSystem {
	[HideInInspector]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class EChunk : MonoBehaviour {

		private MeshFilter filter;
		private MeshRenderer meshRenderer;
		
		private ChunkData data;

		[HideInInspector]
		public Vector2i cachedPos;
		public Vector2i Pos {
			get {
				return data.pos;
			}
		}

		private MeshCollider meshCollider;
		public Collider Collider {
			get {
				return meshCollider;
			}
		}

		private EChunk front;	// Z+
		private EChunk right;	// X+
		private EChunk back;	// Z-
		private EChunk left;	// X-

		public static EChunk CreateChunk(Transform parent, ETerrainData terrainData, ChunkData data) { // Load Existing
			GameObject chunkObj = new GameObject("Chunk" + data.pos);
			chunkObj.transform.parent = parent;
			chunkObj.transform.localPosition = new Vector3(data.pos.x * data.mesh.Size.x, 0f, data.pos.y * data.mesh.Size.y);
			EChunk chk = chunkObj.AddComponent<EChunk>();
			chk.Init(terrainData, data);
			return chk;
		}

		public static EChunk CreateChunk(Transform parent, ETerrainData terrainData, Vector2i pos, Vector2 chkSize, int quads) { // Create New
			GameObject chunkObj = new GameObject("Chunk" + pos);
			chunkObj.transform.parent = parent;
			chunkObj.transform.localPosition = new Vector3(pos.x * chkSize.x, 0f, pos.y * chkSize.y);
			EChunk chk = chunkObj.AddComponent<EChunk>();
			chk.Init(terrainData, pos, chkSize, quads);
			return chk;
		}

		public void Init(ETerrainData terrainData, ChunkData data) {
			this.data = data;
			meshRenderer = GetComponent<MeshRenderer>();
			filter = GetComponent<MeshFilter>();
			DestroyImmediate(filter.sharedMesh);
			data.mesh.RecreateMesh();
			filter.sharedMesh = data.mesh;
			RebuildCollider();
			LinkMeshes(terrainData);
		}

		private void Init(ETerrainData terrainData, Vector2i pos, Vector2 chkSize, int quads) {
			data = new ChunkData();
			data.pos = pos;
			cachedPos = pos;
			terrainData.chunks.Add(data.pos, data);
			meshRenderer = GetComponent<MeshRenderer>();
			filter = GetComponent<MeshFilter>();
			DestroyImmediate(filter.sharedMesh);
			data.mesh = new ChunkMesh(chkSize, quads);
			filter.sharedMesh = data.mesh;
			data.mesh.GeneratePlane();
			RebuildCollider();
			LinkMeshes(terrainData);
		}

		private void LinkMeshes(ETerrainData terrainData) {
			if (data.mesh.front == null) {
				ChunkData front;
				if (terrainData.chunks.TryGetValue(Pos + Vector2i.front, out front)) {
					data.mesh.front = front.mesh;
					front.mesh.back = data.mesh;
				}
			}
			if (data.mesh.right == null) {
				ChunkData right;
				if (terrainData.chunks.TryGetValue(Pos + Vector2i.right, out right)) {
					data.mesh.right = right.mesh;
					right.mesh.left = data.mesh;
				}
			}
			if (data.mesh.back == null) {
				ChunkData back;
				if (terrainData.chunks.TryGetValue(Pos + Vector2i.back, out back)) {
					data.mesh.back = back.mesh;
					back.mesh.front = data.mesh;
				}
			}
			if (data.mesh.left == null) {
				ChunkData left;
				if (terrainData.chunks.TryGetValue(Pos + Vector2i.left, out left)) {
					data.mesh.left = left.mesh;
					left.mesh.right = data.mesh;
				}
			}
		}

		public void RebuildCollider() {
			meshCollider = GetComponent<MeshCollider>();
			meshCollider.sharedMesh = data.mesh;
		}

		public void UnbindData(ETerrainData terrainData) {
			terrainData.chunks.Remove(Pos);
		}

		public void SetMaterial(Material mat) {
			meshRenderer.material = mat;
		}

		public void Resize(Vector2 nSize) {
			transform.localPosition = new Vector3(Pos.x * nSize.x, 0f, Pos.y * nSize.y);
			data.mesh.Resize(new Vector3(nSize.x, 0f, nSize.y));
		}

		public void SetResolution(int quads) {
			data.mesh.SetResolution(quads);
		}
		
	}
}
