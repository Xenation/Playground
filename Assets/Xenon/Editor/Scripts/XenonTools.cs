using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class XenonTools : EditorWindow {

		private const float WIN_MIN_WIDTH = 300f;
		private const float WIN_MIN_HEIGHT = 200f;

		[MenuItem("Window/Xenon Tools")]
		public static void ShowWindow() {
			XenonTools win = GetWindow<XenonTools>("Xenon Tools");
			win.minSize = new Vector2(WIN_MIN_WIDTH, WIN_MIN_HEIGHT);
		}

		private void OnGUI() {

			if (GUILayout.Button("Essentials")) {
				Essentials.ShowWindow();
			}
			
			if (GUILayout.Button("Screenshot Tool")) {
				ScreenshotTool.ShowWindow();
			}

			if (GUILayout.Button("Timings Debugger")) {
				TimingDebuggerWindow.ShowWindow();
			}

		}

	}
}
