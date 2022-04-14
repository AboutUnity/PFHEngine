using System;
using UnityEngine;

namespace PFH.Worlds
{
	public class WorldConfig : ScriptableObject
	{
		[Serializable]
		public struct Cell
		{
			[HideInInspector]
			public string DebugTitle;
			public string LevelName;
			public Rect Rect;
		}

		public Cell[] Cells = Array.Empty<Cell>();
		public float CellSize = 48f;
		public float PreloadRadius = 20f;
		public float LoadRadius = 15f;
		public float UnloadRadius = 25f;
		public float RadiusHorizontal = 1.1f;
		public float RadiusOffsetY = 5f;
		public float CheckTime = 0.5f;

#if UNITY_EDITOR
		public void OnValidate()
		{
			for (int i = 0; i < Cells.Length; i++)
			{
				Cells[i].DebugTitle = $"{i}. {Cells[i].LevelName}   [ {Cells[i].Rect.position.x / CellSize} : {Cells[i].Rect.position.y / CellSize} ]";
			}
		}
#endif
	}
}