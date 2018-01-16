using UnityEditor;
using UnityEngine;

namespace Playground.Editor {
	public class ScreenshotTool : EditorWindow {

		private const float WIN_MIN_WIDTH = 300f;
		private const float WIN_MIN_HEIGHT = 75f;

		private const int SUPER_SIZE_MIN = 1;
		private const int SUPER_SIZE_MAX = 8;

		private string fileName = "screenshot";
		private string filePath = "screenshot.png";

		private int superSize = 1;

		public static void ShowWindow() {
			ScreenshotTool win = GetWindow<ScreenshotTool>("Screenshot Tool");
			win.minSize = new Vector2(WIN_MIN_WIDTH, WIN_MIN_HEIGHT);
		}

		private void OnGUI() {
			fileName = EditorGUILayout.TextField("File", fileName);
			filePath = fileName + ".png";
			
			superSize = EditorGUILayout.IntSlider("Super Size", superSize, SUPER_SIZE_MIN, SUPER_SIZE_MAX);

			if (GUILayout.Button("Save Screenshot")) {
				if (EditorApplication.isPlaying) {
					ScreenCapture.CaptureScreenshot(filePath, superSize);
					EditorUtility.RevealInFinder(filePath);
				}
			}

			if (!EditorApplication.isPlaying) {
				EditorGUILayout.LabelField("Not in play mode: cannot take screenshots!");
			} else {
				EditorGUILayout.LabelField("In play mode: can take screenshots.");
			}
		}

	}
}
