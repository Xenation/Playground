using UnityEditor;
using UnityEngine;

namespace Playground {
	public class ScreenshotTool : EditorWindow {

		private const float WIN_MIN_WIDTH = 300f;
		private const float WIN_MIN_HEIGHT = 65f;

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
				ScreenCapture.CaptureScreenshot(filePath, superSize);
				EditorUtility.RevealInFinder(filePath);
			}
		}

	}
}
