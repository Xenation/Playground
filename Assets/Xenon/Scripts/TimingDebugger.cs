using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Xenon {
	[Serializable]
	public class Timing {

		private Stopwatch stopwatch = new Stopwatch();

		public string id { get; private set; }
		public int frames { get; private set; }

		public Timing parent { get; private set; }
		private Dictionary<string, Timing> childs = new Dictionary<string, Timing>();

		private long prevFrameTotal = 0L;
		private long maxTime = 0;

		public Timing(Timing parent, string id) {
			this.parent = parent;
			this.id = id;
			frames = 0;
		}

		public void EndFrame() {
			frames++;
			if (stopwatch.ElapsedMilliseconds - prevFrameTotal > maxTime) {
				maxTime = stopwatch.ElapsedMilliseconds - prevFrameTotal;
			}
			prevFrameTotal = stopwatch.ElapsedMilliseconds;
		}

		public void Start() {
			stopwatch.Start();
		}

		public void Stop() {
			stopwatch.Stop();
		}

		public float GetAverageMilliseconds() {
			if (frames == 0) {
				return 0f;
			}
			return stopwatch.ElapsedMilliseconds / frames;
		}

		public long GetMaxMilliseconds() {
			return maxTime;
		}

		public Timing GetChildTiming(string id) {
			Timing timing;
			childs.TryGetValue(id, out timing);
			return timing;
		}

		public Timing AddChild(string id) {
			Timing timing = new Timing(this, id);
			childs.Add(id, timing);
			return timing;
		}

		public void DrawGUI(float width) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(id, GUILayout.MaxWidth(width / 2f - 10f));
			EditorGUILayout.LabelField("avg: " + GetAverageMilliseconds() + "ms" + " - max: " + GetMaxMilliseconds() + "ms", TimingDebugger.alignRight, GUILayout.MaxWidth(width / 2f));
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel++;
			foreach (Timing timing in childs.Values) {
				timing.DrawGUI(width);
			}
			EditorGUI.indentLevel--;
		}

	}

	public static class TimingDebugger {

		public static readonly GUIStyle alignRight = new GUIStyle();

		public static int prevFrameCount = -1;

		public static Timing root { get; private set; }
		public static Timing scope { get; private set; }
		private static HashSet<Timing> currentFrameTimings;

		public static bool hasRecordedThisFrame {
			get {
				return currentFrameTimings.Count != 0;
			}
		}

		static TimingDebugger() {
			Init();
		}

		private static void Init() {
			root = new Timing(null, "__Root__");
			scope = root;
			currentFrameTimings = new HashSet<Timing>();
			alignRight.alignment = TextAnchor.MiddleRight;
		}

		public static void EndFrame() {
			foreach (Timing timing in currentFrameTimings) {
				timing.EndFrame();
			}
			currentFrameTimings.Clear();
			scope = root;
		}

		public static void Start(string id) {
			Timing timing = scope.GetChildTiming(id);
			if (timing == null) {
				timing = scope.AddChild(id);
			}
			timing.Start();
			scope = timing;
			currentFrameTimings.Add(timing);
		}

		public static void Stop() {
			scope.Stop();
			scope = scope.parent;
		}

		public static void ClearAll() {
			Init();
		}

	}
}
