using UnityEngine;

namespace PFH.Worlds
{
	public interface ILevelData
	{
		string LevelName { get; }
		Rect Rect { get; }

#if UNITY_EDITOR
		void OnValidate(int num);	
#endif
	}
}