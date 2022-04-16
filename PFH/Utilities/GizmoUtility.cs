#if UNITY_EDITOR
using UnityEngine;

namespace PFH.Utilities
{
	public static class GizmoUtility
	{
		public static Vector3 Fault = new Vector3(0f, 0.1f, 0f);

		public static void DrawString(string text, Vector3 worldPos, Color? colour = null)
		{
			UnityEditor.Handles.BeginGUI();

			var restoreColor = GUI.color;

			if (colour.HasValue) GUI.color = colour.Value;
			var view = UnityEditor.SceneView.currentDrawingSceneView;
			Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

			if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width ||
				screenPos.z < 0)
			{
				GUI.color = restoreColor;
				UnityEditor.Handles.EndGUI();
				return;
			}

			Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
			GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y),
				text);
			GUI.color = restoreColor;
			UnityEditor.Handles.EndGUI();
		}

		public static void DrawSquare(Vector3 pos, float halfSize, Color color)
		{
			Vector3 v = pos;
			Gizmos.color = color;
			Gizmos.DrawLine(v + new Vector3(-halfSize, 0.1f, -halfSize), v + new Vector3(halfSize, 0.1f, -halfSize));
			Gizmos.DrawLine(v + new Vector3(halfSize, 0.1f, -halfSize), v + new Vector3(halfSize, 0.1f, halfSize));
			Gizmos.DrawLine(v + new Vector3(halfSize, 0.1f, halfSize), v + new Vector3(-halfSize, 0.1f, halfSize));
			Gizmos.DrawLine(v + new Vector3(-halfSize, 0.1f, halfSize), v + new Vector3(-halfSize, 0.1f, -halfSize));
		}

		public static void DrawSquare(Vector3 pos, float halfWidth, float halfHeight, Color color)
		{
			Vector3 v = pos;
			Gizmos.color = color;
			Gizmos.DrawLine(v + new Vector3(-halfWidth, 0.1f, -halfHeight),
				v + new Vector3(halfWidth, 0.1f, -halfHeight));
			Gizmos.DrawLine(v + new Vector3(halfWidth, 0.1f, -halfHeight),
				v + new Vector3(halfWidth, 0.1f, halfHeight));
			Gizmos.DrawLine(v + new Vector3(halfWidth, 0.1f, halfHeight),
				v + new Vector3(-halfWidth, 0.1f, halfHeight));
			Gizmos.DrawLine(v + new Vector3(-halfWidth, 0.1f, halfHeight),
				v + new Vector3(-halfWidth, 0.1f, -halfHeight));
		}

		public static void DrawRect(Rect rect, float y, Color color)
		{
			Vector3 v1 = new Vector3(rect.xMin, y, rect.yMin);
			Vector3 v2 = new Vector3(rect.xMax, y, rect.yMin);
			Vector3 v3 = new Vector3(rect.xMax, y, rect.yMax);
			Vector3 v4 = new Vector3(rect.xMin, y, rect.yMax);
			Gizmos.color = color;
			Gizmos.DrawLine(v1, v2);
			Gizmos.DrawLine(v2, v3);
			Gizmos.DrawLine(v3, v4);
			Gizmos.DrawLine(v4, v1);
		}
	}
}
#endif