using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class TimingDebuggerWindow : XenonWindow<TimingDebuggerWindow> {

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 300f; } }
		protected override string titleStr { get { return "Timings Debugger"; } }

		private void OnEnable() {
			EditorApplication.playmodeStateChanged = PlayModeChange;
		}

		private void PlayModeChange() {
			if (EditorApplication.isPlaying) {
				TimingDebugger.ClearAll();
			}
		}

		private void Update() {
			if (TimingDebugger.hasRecordedThisFrame) {
				Repaint();
			}

			if (Time.frameCount > TimingDebugger.prevFrameCount) {
				TimingDebugger.EndFrame();
				TimingDebugger.prevFrameCount = Time.frameCount;
			}
		}

		private void OnGUI() {
			TimingDebugger.root.DrawGUI(position.width);

			if (GUILayout.Button("Reset")) {
				TimingDebugger.ClearAll();
			}
		}

	}
}
