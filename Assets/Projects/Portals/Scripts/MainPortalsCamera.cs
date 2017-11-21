using UnityEngine;
using UnityEngine.Rendering;

namespace Playground.Portals {
	public class MainPortalsCamera : MonoBehaviour {

		public Material portalRedPost;
		public Material portalBluePost;

		private CommandBuffer portalCmds;

		public void OnEnable() {
			portalCmds = new CommandBuffer();
		}

		public void OnRenderImage(RenderTexture source, RenderTexture destination) {
			RenderTexture tmp = RenderTexture.GetTemporary(source.width, source.height, 24);
			Graphics.Blit(source, tmp, portalRedPost);
			Graphics.Blit(tmp, destination, portalBluePost);
			tmp.Release();
		}

	}
}
