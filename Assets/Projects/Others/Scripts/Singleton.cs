using UnityEngine;

namespace Playground {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		private static T instance;
		public static T I {
			get {
				if (instance == null) {
					instance = FindObjectOfType<T>();
					if (instance == null) {
						Debug.LogError("Could not find any instance of type " + typeof(T) + " in scene!");
					}
				}
				return instance;
			}
		}

	}
}
