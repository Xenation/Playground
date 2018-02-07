using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class ScreenshotTool : XenonWindow<ScreenshotTool> {

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 75f; } }
		protected override string titleStr { get { return "Screenshot Tool"; } }

		private const int SUPER_SIZE_MIN = 1;
		private const int SUPER_SIZE_MAX = 8;

		private string fileName = "screenshot";
		private string filePath = "screenshot.png";

		private int superSize = 1;

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
