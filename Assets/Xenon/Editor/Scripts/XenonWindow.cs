using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public abstract class XenonWindow<T> : EditorWindow where T : EditorWindow {

		protected abstract float minWidth { get; }
		protected abstract float minHeight { get; }
		protected abstract string titleStr { get; }


		public static void ShowWindow() {
			XenonWindow<T> win = GetWindow<T>() as XenonWindow<T>;
			if (win == null) {
				Debug.LogWarning("Could not create XenonWindow!");
				return;
			}
			win.titleContent = new GUIContent(win.titleStr);
			win.minSize = new Vector2(win.minWidth, win.minHeight);
		}

	}
}
