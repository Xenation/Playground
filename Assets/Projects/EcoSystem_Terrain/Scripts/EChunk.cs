﻿using UnityEngine;

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

		public static EChunk CreateChunk(Transform parent, ChunkData data) { // Load Existing
			GameObject chunkObj = new GameObject("Chunk" + data.pos);
			chunkObj.transform.parent = parent;
			chunkObj.transform.localPosition = new Vector3(data.pos.x * data.mesh.Size.x, 0f, data.pos.y * data.mesh.Size.y);
			EChunk chk = chunkObj.AddComponent<EChunk>();
			chk.Init(data);
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

		public void Init(ChunkData data) {
			this.data = data;
			meshRenderer = GetComponent<MeshRenderer>();
			filter = GetComponent<MeshFilter>();
			DestroyImmediate(filter.sharedMesh);
			data.mesh.RecreateMesh();
			filter.sharedMesh = data.mesh;
			meshCollider = GetComponent<MeshCollider>();
			meshCollider.sharedMesh = data.mesh;
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
