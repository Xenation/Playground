using UnityEngine;

namespace Playground.Portals {
	public class CameraController : MonoBehaviour {
		
		public float speed;
		public float lookSpeed;

		private PortalManager portalManager;

		private Vector3 vel;
		private Vector3 rot;

		private Vector3 prevMousePos;

		public void Start() {
			portalManager = PortalManager.I;
		}

		public void Update() {
			vel.x = Input.GetAxisRaw("Horizontal");
			vel.y = Input.GetAxisRaw("Vertical");
			vel.z = Input.GetAxisRaw("Depth");
			vel = vel.normalized * speed * Time.fixedDeltaTime;
			if (Input.GetMouseButton(1) && prevMousePos != Vector3.zero) {
				Vector2 mouseDelta = Input.mousePosition - prevMousePos;
				rot.y = mouseDelta.x * lookSpeed * Time.fixedDeltaTime;
				rot.x = -mouseDelta.y * lookSpeed * Time.fixedDeltaTime;
			} else {
				rot = Vector3.zero;
				prevMousePos = Vector3.zero;
			}
			prevMousePos = Input.mousePosition;
		}

		public void FixedUpdate() {
			transform.Translate(vel);
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rot);
			portalManager.UpdateCameraPositions();
		}

	}
}
