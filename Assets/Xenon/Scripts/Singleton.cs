using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Xenon {
	[HideInInspector]
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		private static T instance;
		/// <summary>
		/// Returns the instance of the singleton
		/// </summary>
		public static T I {
			get {
				if (instance == null) {
					instance = FindObjectOfType<T>();
					if (instance == null) {
						Debug.LogError("Could not find an instance of " + typeof(T).Name + "!");
					}
				}
				return instance;
			}
		}

	}
}
