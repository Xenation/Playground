﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace EcoSystem {
	[AddComponentMenu("EcoSystem/Terrain")]
	public class ETerrain : MonoBehaviour {

		#region Attributes/Props
		public Material terrainMaterial;
		public int chunksCountX = 8;
		public int chunksCountZ = 8;
		public Vector2 terrainSize = new Vector2(2048f, 2048f);
		public Vector2 chunkSize {
			get {
				return new Vector2(terrainSize.x / chunksCountX, terrainSize.y / chunksCountZ);
			}
		}
		[Range(1, 256)]
		public int desiredQuads = 32;
		[System.NonSerialized]
		public int actualQuads = 32;

		private Dictionary<Vector2i, EChunk> chunks = new Dictionary<Vector2i, EChunk>();
		public int ActualChunkCount {
			get {
				return chunks.Count;
			}
		}

		public ETerrainData data;

		public VirtualMesh virtualMesh;

		#region Editor
		// TODO Unsafe for build (deserialization)
#if UNITY_EDITOR
		// TOOLS
		[SerializeField]
		private float brushSize = 5f;
		[Range(0f, 1f)]
		[SerializeField]
		private float brushHardCenter = .5f;
		[Range(0f, 1f)]
		[SerializeField]
		private float brushDensity = 1f;
		[SerializeField]
		private float flattenHeight = 0f;
#endif
		#endregion
		#endregion

		public void LoadData() {
			if (data == null) return;
			TimingDebugger.Start("Load Data");
			// Find Existing "Incomplete" chunks
			chunks.Clear();
			EChunk[] existing = transform.GetComponentsInChildren<EChunk>();
			for (int i = 0; i < existing.Length; i++) {
				chunks.Add(existing[i].cachedPos, existing[i]);
			}
			// Load from data
			foreach (ChunkData chkData in data.chunks.Values) {
				EChunk toLoad;
				if (chunks.TryGetValue(chkData.pos, out toLoad)) {
					toLoad.Init(chkData);
				} else {
					chunks.Add(chkData.pos, EChunk.CreateChunk(transform, data, chkData));
				}
			}
			virtualMesh = new VirtualMesh(this);
			TimingDebugger.Stop();
		}

		public void Generate() {
			TimingDebugger.Start("Generation");
			Clear();
			for (int z = 0; z < chunksCountZ; z++) {
				for (int x = 0; x < chunksCountX; x++) {
					AddChunkAt(new Vector2i(x, z));
				}
			}
			virtualMesh = new VirtualMesh(this);
			TimingDebugger.Stop();
		}

		private void AddChunkAt(Vector2i pos) {
			EChunk chk = EChunk.CreateChunk(transform, data, pos, chunkSize, actualQuads);
			chunks.Add(pos, chk);
			chk.SetMaterial(terrainMaterial);
		}

		private void AddChunks(ICollection<Vector2i> positions) {
			foreach (Vector2i pos in positions) {
				AddChunkAt(pos);
			}
		}

		private void RemoveChunks(ICollection<Vector2i> positions) {
			foreach (Vector2i pos in positions) {
				EChunk chk;
				if (chunks.TryGetValue(pos, out chk)) {
					chunks[pos].UnbindData(data);
					DestroyImmediate(chunks[pos].gameObject);
				}
				chunks.Remove(pos);
			}
		}

		public void Clear() {
			TimingDebugger.Start("Clear");
			foreach (EChunk chk in chunks.Values) {
				chk.UnbindData(data);
				DestroyImmediate(chk.gameObject);
			}
			chunks.Clear();
			TimingDebugger.Stop();
		}

		public void UpdateTerrainMaterial() {
			foreach (EChunk chk in chunks.Values) {
				chk.SetMaterial(terrainMaterial);
			}
		}

		public void Resize(Vector2 nTerrainSize, int nCountX, int nCountZ) {
			ResizeChunkGrid(nCountX, nCountZ);
			ModifyChunkSize(new Vector2(nTerrainSize.x / nCountX, nTerrainSize.y / nCountZ));
			virtualMesh.Refetch();
		}

		public void ModifyChunkSize(Vector2 nSize) {
			TimingDebugger.Start("Modify Chunk Size");
			foreach (EChunk chk in chunks.Values) {
				chk.Resize(nSize);
				chk.RebuildCollider();
			}
			TimingDebugger.Stop();
		}

		public void ResizeChunkGrid(int nCountX, int nCountZ) {
			TimingDebugger.Start("Resize Chunk Grid");
			if (nCountX == chunksCountX && nCountZ == chunksCountZ) return;
			// TODO optimise added and then removed corner when downsizing in one dimension and upsizing in the other
			HashSet<Vector2i> removePositions = new HashSet<Vector2i>();
			HashSet<Vector2i> addPositions = new HashSet<Vector2i>();
			if (nCountX < chunksCountX) { // Downsize in X
				for (int x = nCountX; x < chunksCountX; x++) {
					for (int z = 0; z < chunksCountZ; z++) {
						removePositions.Add(new Vector2i(x, z));
					}
				}
			} else if (nCountX > chunksCountX) { // Upsize in x
				for (int x = chunksCountX; x < nCountX; x++) {
					for (int z = 0; z < chunksCountZ; z++) {
						addPositions.Add(new Vector2i(x, z));
					}
				}
			}
			if (nCountZ < chunksCountZ) { // Downsize in z
				for (int z = nCountZ; z < chunksCountZ; z++) {
					for (int x = 0; x < nCountX; x++) {
						removePositions.Add(new Vector2i(x, z));
					}
				}
			} else if (nCountZ > chunksCountZ) { // Upsize in z
				for (int z = chunksCountZ; z < nCountZ; z++) {
					for (int x = 0; x < nCountX; x++) {
						addPositions.Add(new Vector2i(x, z));
					}
				}
			}
			RemoveChunks(removePositions);
			AddChunks(addPositions);
			TimingDebugger.Stop();
		}
		
		public void SetChunkResolution(int quads) {
			TimingDebugger.Start("SetResolution");
			if (!IsPowerOfTwo(quads)) {
				quads = NextPowerOfTwo(quads);
			}
			actualQuads = quads;
			foreach (EChunk chk in chunks.Values) {
				chk.SetResolution(quads);
				chk.RebuildCollider();
			}
			virtualMesh.Refetch();
			TimingDebugger.Stop();
		}

		public void RebuildCollisions() {
			TimingDebugger.Start("Collision Rebuild");
			foreach (EChunk chk in chunks.Values) {
				chk.RebuildCollider();
			}
			TimingDebugger.Stop();
		}

		private bool IsPowerOfTwo(int x) {
			return (x != 0) && ((x & (x - 1)) == 0);
		}

		private int NextPowerOfTwo(int x) {
			if (x == 0) return 1;
			int current = x;
			int shifted = 0;
			while (current > 0) {
				current = current >> 1;
				shifted++;
			}
			return 1 << shifted;
		}

		public bool Raycast(Ray ray, out RaycastHit hit, float distance) {
			hit = new RaycastHit();
			foreach (EChunk chk in chunks.Values) {
				if (chk.Collider.Raycast(ray, out hit, distance)) {
					return true;
				}
			}
			return false;
		}

		public Vector2i[] ChunkPositionsTouching(Rect rect) {
			Vector2 chkSize = chunkSize;
			int bottom = (int) (rect.yMin / chkSize.y);
			int top = (int) (rect.yMax / chkSize.y);
			int left = (int) (rect.xMin / chkSize.x);
			int right = (int) (rect.xMax / chkSize.x);
			Vector2i[] touching = new Vector2i[(top - bottom + 1) * (right - left + 1)];
			int index = 0;
			for (int y = bottom; y <= top; y++) {
				for (int x = left; x <= right; x++) {
					touching[index++] = new Vector2i(x, y);
				}
			}
			return touching;
		}

	}
}
