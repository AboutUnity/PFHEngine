using System;
using System.Collections.Generic;

namespace PFH.Worlds
{
	public interface ILevelLoaderManager : IDisposable
	{
		event Action<ILevelController> LoadEvent;
		event Action<ILevelController> UnloadEvent;
		event Action AllLoadEvent;

		List<ILevelController> Levels { get; }
		bool Loading { get; }

		void ActivateLevel(string levelName);
		bool HasLevel(string levelName);
		
		void UnloadAll(string[] exclude = null);
		void Unload(string levelName);

		void Load(string levelName);
		bool Add(string levelName);
	}
}