using System;
using UnityEditor;
using UnityEngine;

namespace Playground {
	public class Essentials : EditorWindow {

		private const float WIN_MIN_WIDTH = 300f;
		private const float WIN_MIN_HEIGHT = 200f;

		private bool shortcutsEnabled = true;

		private bool ctrl = false;

		public static void ShowWindow() {
			Essentials win = GetWindow<Essentials>("Essentials");
			win.minSize = new Vector2(WIN_MIN_WIDTH, WIN_MIN_HEIGHT);
		}

		public void ResetModifiers() {
			ctrl = false;
		}

		private void Awake() {
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
		}

		private void OnDestroy() {
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

		private void OnGUI() {
			shortcutsEnabled = EditorGUILayout.ToggleLeft("Shortcuts Enabled", shortcutsEnabled);
			if (!shortcutsEnabled) {
				ResetModifiers();
			}
		}

		private void OnSceneGUI(SceneView scene) {
			Event e = Event.current;
			if (!shortcutsEnabled && (e.type == EventType.keyDown || e.type == EventType.KeyUp)) {
				return;
			}
			switch (e.type) {
				case EventType.KeyDown: // DOWN
					//Debug.Log(e.keyCode); // Pressed debug
					switch (e.keyCode) {
						case KeyCode.LeftControl: // Left Control Modifier
							ctrl = true;
							break;
						case KeyCode.W: // Wireframe switch
							if (ctrl) {
								SwitchRenderMode(scene);
							}
							break;
						case KeyCode.Backslash: // Deselector
							DeselectAll();
							break;
					}
					break;

				case EventType.KeyUp: // UP
					switch (e.keyCode) {
						case KeyCode.LeftControl: // Left Control Modifier
							ctrl = false;
							break;
					}
					break;
			}
		}

		private void SwitchRenderMode(SceneView sceneView) {
			switch (sceneView.renderMode) {
				default:
				case DrawCameraMode.Textured:
					sceneView.renderMode = DrawCameraMode.TexturedWire;
					break;
				case DrawCameraMode.TexturedWire:
					sceneView.renderMode = DrawCameraMode.Textured;
					break;
			}
			sceneView.Repaint();
		}

		private void DeselectAll() {
			Selection.activeGameObject = null;
		}

	}
}
