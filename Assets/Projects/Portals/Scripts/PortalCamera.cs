using UnityEngine;

namespace Playground.Portals {
	public class PortalCamera : MonoBehaviour {

		public Material postprocess;

		public void OnRenderImage(RenderTexture source, RenderTexture destination) {
			Graphics.Blit(source, destination, postprocess);
		}

	}
}
