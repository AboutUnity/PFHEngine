using UnityEngine;

namespace PFH.Worlds
{
	public static class WorldUtility
	{
		public static void DrawGizmos(WorldConfig config, IWorldManager world)
		{
			Transform target = null;
			if(world != null && world.Target != null)
			{
				target = world.Target;
			}

			if (target != null)
			{
				var offset = new Vector3(0f, 0f, config.RadiusOffsetY);
				GizmoUtility.DrawSquare(target.position + offset, config.PreloadRadius * config.RadiusHorizontal, config.LoadRadius, Color.blue);
				GizmoUtility.DrawSquare(target.position + offset, config.LoadRadius * config.RadiusHorizontal, config.LoadRadius, Color.green);
				GizmoUtility.DrawSquare(target.position + offset, config.UnloadRadius * config.RadiusHorizontal, config.UnloadRadius, Color.red);
			}

			foreach (var cell in config.Cells)
			{
				GizmoUtility.DrawRect(cell.Rect, 0.05f, Color.white);

				var c = cell.Rect.center;
				Vector3 p = new Vector3(c.x, 0.1f, c.y);
				UnityEditor.Handles.Label(p, cell.LevelName);
			}
		}
	}
}