using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Xenon {
	public class EventManager {

		private class HandlingMethod {

			public MethodInfo method;
			public Type type;
			public List<IEventListener> listeners;

			public HandlingMethod(MethodInfo meth, Type t) {
				method = meth;
				type = t;
				listeners = new List<IEventListener>();
			}

			public void Invoke(IEventSender sender, XEvent ev) {
				foreach (IEventListener listener in listeners) {
					method.Invoke(listener, new object[] { sender, ev });
				}
			}

		}

		public static EventManager instance = new EventManager();

		public static EventManager I {
			get {
				return instance;
			}
		}
		
		private Dictionary<string, List<HandlingMethod>> handlers;
		private Dictionary<Type, List<HandlingMethod>> handlingMethods;

		public EventManager() {
			handlers = new Dictionary<string, List<HandlingMethod>>();
			handlingMethods = new Dictionary<Type, List<HandlingMethod>>();
		}

		public void RegisterListener(IEventListener listener) {
			if (IsTypeRegisterNecessary(listener.GetType())) {
				RegisterType(listener.GetType());
			}
			foreach (HandlingMethod meth in handlingMethods[listener.GetType()]) {
				meth.listeners.Add(listener);
			}
		}

		public void UnregisterListener(IEventListener listener) {
			List<HandlingMethod> evHandlers;
			if (handlingMethods.TryGetValue(listener.GetType(), out evHandlers)) {
				foreach (HandlingMethod meth in evHandlers) {
					meth.listeners.Add(listener);
				}
			}
		}
		
		private bool IsTypeRegisterNecessary(Type listenerType) {
			List<HandlingMethod> meths;
			if (handlingMethods.TryGetValue(listenerType, out meths)) {
				return false;
			} else {
				meths = new List<HandlingMethod>();
				handlingMethods.Add(listenerType, meths);
				return true;
			}
		}

		private void RegisterType(Type listenerType) {
			MethodInfo[] methods = listenerType.GetMethods();
			for (int i = 0; i < methods.Length; i++) {
				if (methods[i].Name.StartsWith("On")) {
					ParameterInfo[] parameters = methods[i].GetParameters();
					if (parameters.Length == 2 && IsClassOrSub(parameters[0].ParameterType, typeof(IEventSender)) && IsClassOrSub(parameters[1].ParameterType, typeof(XEvent))) {
						string methSuffix = methods[i].Name.Substring(2);
						HandlingMethod meth = new HandlingMethod(methods[i], listenerType);
						List<HandlingMethod> evHanlders;
						if (!handlers.TryGetValue(methSuffix, out evHanlders)) {
							evHanlders = new List<HandlingMethod>();
							handlers.Add(methSuffix, evHanlders);
						}
						evHanlders.Add(meth);
						handlingMethods[listenerType].Add(meth);
					}
				}
			}
		}

		private bool IsClassOrSub(Type t1, Type t2) {
			return t1 == t2 || t1.IsSubclassOf(t2);
		}

		public void SendEvent(IEventSender sender, XEvent ev) {
			string suffix = HandlingMethodSuffix(ev.GetType());
			List<HandlingMethod> evHandlers;
			if (handlers.TryGetValue(suffix, out evHandlers)) {
				foreach (HandlingMethod meth in evHandlers) {
					meth.Invoke(sender, ev);
				}
			}
		}

		private string HandlingMethodSuffix(Type evType) {
			string cName = evType.Name;
			if (cName.EndsWith("Event")) {
				return cName.Substring(0, cName.Length - 5);
			} else {
				return cName;
			}
		}
		
	}
}
