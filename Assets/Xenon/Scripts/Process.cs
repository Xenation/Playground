using System.Collections.Generic;

namespace Xenon {
	public abstract class Process {

		public delegate void OnTerminateCallback();
		public event OnTerminateCallback TerminateCallback;

		public ProcessState State { get; private set; }

		public List<Process> Attached { get; private set; }

		public Process() {
			State = ProcessState.CREATED;
			Attached = new List<Process>();
		}

		public void Start() {
			State = ProcessState.STARTED;
			OnBegin();
		}

		public void Terminate() {
			State = ProcessState.TERMINATED;
		}

		public void InvokeTerminateCallback() {
			if (TerminateCallback != null) {
				TerminateCallback.Invoke();
			}
		}

		public void Cancel() {
			State = ProcessState.CANCELLED;
		}

		public void Attach(Process process) {
			Attached.Add(process);
		}

		public abstract void Update(float dt);
		public abstract void OnBegin();
		public abstract void OnTerminate();

	}
}
