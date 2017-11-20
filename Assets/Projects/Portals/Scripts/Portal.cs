using UnityEngine;

namespace Playground.Portals {
	public class Portal : MonoBehaviour {

		public Camera cam;
		public Vector3 CameraOffset {
			get {
				return cam.transform.position - transform.position;
			}
			set {
				cam.transform.position = transform.position - value;
			}
		}

	}
}
