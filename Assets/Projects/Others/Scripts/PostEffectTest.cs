using UnityEngine;

namespace Playground.Others {
	[ExecuteInEditMode]
	public class PostEffectTest : MonoBehaviour {
		
		public Material postEffect;

		private Camera cam;

		public void OnEnable() {
			cam = GetComponent<Camera>();
			cam.depthTextureMode = DepthTextureMode.Depth;
		}

		public void OnRenderImage(RenderTexture source, RenderTexture destination) {
			if (postEffect == null) return;
			Graphics.Blit(source, destination, postEffect);
		}

	}
}
