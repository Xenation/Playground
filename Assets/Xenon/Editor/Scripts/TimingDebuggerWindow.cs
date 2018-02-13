using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class TimingDebuggerWindow : XenonWindow<TimingDebuggerWindow> {

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 300f; } }
		protected override string titleStr { get { return "Timings Debugger"; } }

		private void OnEnable() {
			//EditorApplication.playmodeStateChanged = PlayModeChange;
			EditorApplication.playModeStateChanged += PlayModeChange;
		}

		private void PlayModeChange(PlayModeStateChange change) {
			if (change == PlayModeStateChange.ExitingEditMode) { // TODO executed after start -> can't see initialiazation timings
				TimingDebugger.ClearAll();
				//Debug.Log("Exiting editmode: Timings Cleared");
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
