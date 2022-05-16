using UnityEngine;

namespace PFH.Worlds
{
	public interface ILevelController
	{
		GameObject gameObject { get; }
		string LevelName { get; }
		
		void SetLevelData(ILevelData data);
	}
}