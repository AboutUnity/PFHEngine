using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace PFH.Worlds
{
	public class WorldConfig : ScriptableObject, IWorldData
	{
		[Serializable, Obsolete]
		public struct Cell
		{
			[HideInInspector]
			public string DebugTitle;
			public string LevelName;
			public Rect Rect;
		}

		[FormerlySerializedAs("Cells"), Obsolete, HideInInspector] 
		public Cell[] CellsObsolete = Array.Empty<Cell>();
		
		
		[SerializeField]
		private List<SceneLevelData> _cells = new List<SceneLevelData>();
		
		[FormerlySerializedAs("CellSize")]
		[SerializeField]
		private float _cellSize = 48f;
		[FormerlySerializedAs("PreloadRadius")]
		[SerializeField]
		private float _preloadRadius = 20f;
		[FormerlySerializedAs("LoadRadius")]
		[SerializeField]
		private float _loadRadius = 15f;
		[FormerlySerializedAs("UnloadRadius")]
		[SerializeField]
		private float _unloadRadius = 25f;
		[FormerlySerializedAs("RadiusHorizontal")]
		[SerializeField]
		private float _radiusHorizontal = 1.1f;
		[FormerlySerializedAs("RadiusOffsetY")]
		[SerializeField]
		private float _radiusOffsetY = 5f;
		[FormerlySerializedAs("CheckTime")]
		[SerializeField]
		private float _checkTime = 0.5f;

		private List<ILevelData> _cellsCache = null;

		public List<ILevelData> Levels
		{
			get
			{
				if (_cellsCache == null)
				{
					_cellsCache = _cells.Cast<ILevelData>().ToList();
				}
				return _cellsCache;
			}
		}
		public float CellSize => _cellSize;
		public float PreloadRadius => _preloadRadius;
		public float LoadRadius => _loadRadius;
		public float UnloadRadius => _loadRadius;
		public float RadiusHorizontal => _radiusHorizontal;
		public float RadiusOffsetY => _radiusOffsetY;
		public float CheckTime => _checkTime;

#if UNITY_EDITOR
		public void OnValidate()
		{
			if (_cells.Count <= 0)
			{
				foreach (var cell in CellsObsolete)
				{
					_cells.Add(new SceneLevelData(cell.LevelName, cell.Rect));
				}
			}
			
			for (int i = 0; i < _cells.Count; i++)
			{
				_cells[i].OnValidate(i);
			}
		}
#endif
	}
}