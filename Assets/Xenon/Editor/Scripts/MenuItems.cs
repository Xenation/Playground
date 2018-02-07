using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class MenuItems {

		[MenuItem("GameObject/Empty At Origin", false, 0)]
		public static void CreateEmptyAtOrigin() {
			GameObject go = new GameObject("GameObject");
			if (Selection.activeGameObject != null) {
				go.transform.parent = Selection.activeGameObject.transform;
			}
			Undo.RegisterCreatedObjectUndo(go, "Create Empty At Origin");
			Selection.activeGameObject = go;
		}

	}
}
