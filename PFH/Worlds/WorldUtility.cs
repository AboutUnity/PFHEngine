#if UNITY_EDITOR
using PFH.Utilities;
using UnityEngine;

namespace PFH.Worlds
{
	public static class WorldUtility
	{
		public static void DrawGizmos(IWorldData data, IWorldManager world)
		{
			Transform target = null;
			if(world != null && world.Target != null)
			{
				target = world.Target;
			}

			if (target != null)
			{
				var offset = new Vector3(0f, 0f, data.RadiusOffsetY);
				GizmoUtility.DrawSquare(target.position + offset, data.PreloadRadius * data.RadiusHorizontal, data.LoadRadius, Color.blue);
				GizmoUtility.DrawSquare(target.position + offset, data.LoadRadius * data.RadiusHorizontal, data.LoadRadius, Color.green);
				GizmoUtility.DrawSquare(target.position + offset, data.UnloadRadius * data.RadiusHorizontal, data.UnloadRadius, Color.red);
			}

			foreach (var cell in data.Levels)
			{
				GizmoUtility.DrawRect(cell.Rect, 0.05f, Color.white);

				var c = cell.Rect.center;
				Vector3 p = new Vector3(c.x, 0.1f, c.y);
				UnityEditor.Handles.Label(p, cell.LevelName);
			}
		}
	}
}
#endif