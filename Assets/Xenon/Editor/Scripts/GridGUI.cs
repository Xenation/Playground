using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class GridCell {

		public GUIContent content;
		public GUIStyle style;

		public GridCell(GUIContent content, GUIStyle style) {
			this.content = content;
			this.style = style;
		}

	}

	public class GridGUI {

		private List<GridCell> header;
		private List<List<GridCell>> grid;

		private void SetHeader(params string[] titles) {
			header = new List<GridCell>();
			for (int i = 0; i < titles.Length; i++) {
				header.Add(new GridCell(new GUIContent(titles[i]), EditorStyles.boldLabel));
			}
		}

	}
}
