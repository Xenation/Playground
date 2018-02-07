using UnityEngine;

namespace Xenon {
	[AddComponentMenu("Xenon/Process/Rythmer")]
	public class ProcessRythmer : MonoBehaviour {

		public bool useUnityTime = true;

		public ProcessManager ProcessManager { get; private set; }

		public ProcessRythmer() {
			ProcessManager = new ProcessManager();
		}

		public void Update() {
			if (useUnityTime) {
				ProcessManager.UpdateProcesses(Time.deltaTime);
			} else {
				// TODO update using a more precise timing
			}
		}

	}
}
