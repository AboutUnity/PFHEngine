using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PFH.Worlds
{
	[Serializable]
	public class SceneLevelData : ILevelData
	{
		[HideInInspector]
		public string DebugTitle;
		
		[FormerlySerializedAs("LevelName")]
		[SerializeField]
		private string _levelName;
		[FormerlySerializedAs("Rect")]
		[SerializeField]
		private Rect _rect;

		public string LevelName => _levelName;
		public Rect Rect => _rect;

		public SceneLevelData()
		{
		}

		public SceneLevelData(string levelName, Rect rect)
		{
			_levelName = levelName;
			_rect = rect;
		}

#if UNITY_EDITOR
		public void OnValidate(int num)
		{
			DebugTitle = $"{num}. {_levelName}   [ {_rect.position.x / _rect.width} : {_rect.position.y / _rect.height} ]";
		}
#endif
	}
}