#if PFH_ADDRESSABLE
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PFH.Worlds.Levels
{
	[Serializable]
	public class AddressableGameObjectLevelData : ILevelData
	{
		[HideInInspector]
		public string DebugTitle;
		
		[SerializeField]
		private AssetReferenceGameObject _reference;
		[SerializeField]
		private string _levelName;
		[SerializeField]
		private Rect _rect;

		public virtual AssetReferenceGameObject Reference => _reference;
		public virtual string LevelName => _levelName;
		public virtual Rect Rect => _rect;

		public AddressableGameObjectLevelData()
		{
		}

		public AddressableGameObjectLevelData(AssetReferenceGameObject reference, string levelName, Rect rect)
		{
			_reference = reference;
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
#endif