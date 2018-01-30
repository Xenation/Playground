using UnityEngine;

namespace EcoSystem {
	public static class MeshHelper {
		
		public static void GeneratePlane(this Mesh mesh, Vector2 size, int quadsX, int quadsZ) {
			mesh.GeneratePlane(size, quadsX, quadsZ, false, false);
		}

		public static void GeneratePlane(this Mesh mesh, Vector2 size, int quadsX, int quadsZ, bool centerX, bool centerZ) {
			Vector3[] verts;
			int[] indices;
			GeneratePlane(out verts, out indices, size, quadsX, quadsZ, centerX, centerZ);

			// Mesh Update
			mesh.Clear();
			mesh.vertices = verts;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		}

		public static void GeneratePlane(out Vector3[] verts, out int[] indices, Vector2 size, int quadsX, int quadsZ) {
			GeneratePlane(out verts, out indices, size, quadsX, quadsZ, false, false);
		}

		public static void GeneratePlane(out Vector3[] verts, out int[] indices, Vector2 size, int quadsX, int quadsZ, bool centerX, bool centerZ) {
			if (quadsX < 1) {
				quadsX = 1;
			}
			if (quadsZ < 1) {
				quadsZ = 1;
			}

			// Vertices
			verts = new Vector3[(quadsX + 1) * (quadsZ + 1)];
			int index = 0;
			for (int z = 0; z <= quadsZ; z++) { // vert loop
				for (int x = 0; x <= quadsX; x++) {
					verts[index++] = new Vector3(size.x / quadsX * x - ((centerX) ? size.x / 2f : 0f), 0, size.y / quadsZ * z - ((centerZ) ? size.y / 2f : 0f));
				}
			}

			// Indices
			index = 0;
			indices = new int[quadsX * quadsZ * 6];
			for (int z = 0; z < quadsZ; z++) { // quad loop
				for (int x = 0; x < quadsX; x++) {
					int bl = quadsX * z + z + x;
					int br = bl + 1;
					int tl = quadsX * (z + 1) + (z + 1) + x;
					int tr = tl + 1;

					indices[index++] = bl;
					indices[index++] = tl;
					indices[index++] = tr;

					indices[index++] = bl;
					indices[index++] = tr;
					indices[index++] = br;
				}
			}
		}

	}
}
