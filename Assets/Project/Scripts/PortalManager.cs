using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.Portals {
	public class PortalManager : Singleton<PortalManager> {

		public Camera mainCam;
		public Portal redPortal;
		public Portal bluePortal;

		public void UpdateCameraPositions() {
			Vector3 offsetRed = bluePortal.transform.position - mainCam.transform.position;
			redPortal.CameraOffset = offsetRed;
			redPortal.cam.transform.rotation = mainCam.transform.rotation;
			Vector3 offsetBlue = redPortal.transform.position - mainCam.transform.position;
			bluePortal.CameraOffset = offsetBlue;
			bluePortal.cam.transform.rotation = mainCam.transform.rotation;
		}

	}
}
