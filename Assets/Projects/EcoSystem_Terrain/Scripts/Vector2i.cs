using System;
using UnityEngine;

namespace EcoSystem {
	[Serializable]
	public struct Vector2i {

		public static Vector2i front {
			get {
				return new Vector2i(0, 1);
			}
		}
		public static Vector2i right {
			get {
				return new Vector2i(1, 0);
			}
		}
		public static Vector2i back {
			get {
				return new Vector2i(0, -1);
			}
		}
		public static Vector2i left {
			get {
				return new Vector2i(-1, 0);
			}
		}
		public static Vector2i zero {
			get {
				return new Vector2i(0, 0);
			}
		}
		public static Vector2i one {
			get {
				return new Vector2i(1, 1);
			}
		}

		public int x, y;

		public Vector2i(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj) {
			if (!(obj is Vector2i)) {
				return false;
			}

			var i = (Vector2i) obj;
			return x == i.x &&
				   y == i.y;
		}

		public override int GetHashCode() {
			var hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public override string ToString() {
			return "(" + x + ", " + y + ")";
		}

		public static Vector2i operator +(Vector2i a, Vector2i b) {
			return new Vector2i(a.x + b.x, a.y + b.y);
		}

		public static Vector2i operator -(Vector2i a, Vector2i b) {
			return new Vector2i(a.x - b.x, a.y - b.y);
		}

		public static Vector2i operator *(Vector2i a, int b) {
			return new Vector2i(a.x * b, a.y * b);
		}

		public static bool operator ==(Vector2i a, Vector2i b) {
			return (a.x == b.x) && (a.y == b.y);
		}

		public static bool operator !=(Vector2i a, Vector2i b) {
			return (a.x != b.x) || (a.y != b.y);
		}

	}
}
