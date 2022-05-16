#if PFH_ADDRESSABLE
using System;
using System.Collections.Generic;
using PFH.Worlds.Levels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PFH.Worlds.Levels
{
	//TODO implements IInstanceProvider
	
	public class AddressableGameObjectLevelLoaderManager : ILevelLoaderManager
	{
		public event Action<string> PreloadEvent;
		public event Action<ILevelController> LoadEvent;
		public event Action<ILevelController> UnloadEvent;
		public event Action AllLoadEvent;

		private readonly List<ILevelData> _datas;
		private readonly Transform _container;
		private readonly List<KeyValuePair<string, AsyncOperationHandle<GameObject>>> _loadLevels = new List<KeyValuePair<string, AsyncOperationHandle<GameObject>>>();
		private readonly List<ILevelController> _levels = new List<ILevelController>();
		private readonly List<ILevelController> _levelsToUnload = new List<ILevelController>();

		public List<ILevelController> Levels => _levels;
		public bool Loading { get; }

		public AddressableGameObjectLevelLoaderManager(List<ILevelData> datas, Transform container)
		{
			_datas = datas;
			_container = container;
		}
		
		public void Dispose()
		{
			
		}
		
		private void RegisterLevelController(ILevelController level)
		{
			_levels.Add(level);
		}

		private void UnregisterLevelController(ILevelController level)
		{
			var levelName = level.LevelName;
			_levels.Remove(level);

			Debug.Log("LEVEL UNLOADED: " + levelName);
		
			if(UnloadEvent != null)
				UnloadEvent(level);
		}
		
		public void ActivateLevel(string levelName)
		{
			// not need
		}

		public bool HasLevel(string levelName)
		{
			return _levels.Exists(p => p.LevelName == levelName); //TODO add dictionary?
		}

		private void UnloadInternal(ILevelController level)
		{
			UnregisterLevelController(level);
			Addressables.ReleaseInstance(level.gameObject);
		}
		
		public void UnloadAll(string[] exclude = null)
		{
			for(int i = 0; i < _levels.Count; ++i)
			{
				if(exclude == null || !Array.Exists(exclude, p => p == _levels[i].LevelName))
				{
					_levelsToUnload.Add(_levels[i]);
				}
			}

			foreach (var level in _levelsToUnload)
			{
				UnloadInternal(level);
			}
			_levelsToUnload.Clear();
		}

		public void Unload(string levelName)
		{
			var level = _levels.Find(p => p.LevelName == levelName);
			UnloadInternal(level);
		}

		public void Load(string levelName)
		{
			UnloadAll(new []{levelName});
		
			if(!HasLevel(levelName))
				Add(levelName);
		}
		
		private bool IsLoading(string levelName)
		{
			foreach (var pair in _loadLevels)
			{
				if (pair.Key == levelName)
					return true;
			}

			return false;
		}

		public bool Add(string levelName)
		{
			if(HasLevel(levelName))
				return false;
			if (IsLoading(levelName))
				return true;
			
			Debug.Log("LEVEL LOADING: " + levelName);

			LoadGameObjectAsync(levelName);

			return true;
		}

		private async void LoadGameObjectAsync(string levelName)
		{
			var data = (AddressableGameObjectLevelData)_datas.Find(p => p.LevelName == levelName);
			
			var asyncOp = Addressables.InstantiateAsync(data.Reference, _container);
			var pair = new KeyValuePair<string, AsyncOperationHandle<GameObject>>(levelName, asyncOp);
			_loadLevels.Add(pair);
			
			var levelOG = await asyncOp.Task;
			levelOG.name = $"{levelName} ({data.Reference})";

			_loadLevels.Remove(pair);

			ILevelController level = levelOG.GetComponent<ILevelController>();
			if (level == null)
			{
				Debug.LogError($"Incorrect reference {data.Reference} for level {levelName}");
			}
			else
			{
				level.SetLevelData(data);
				RegisterLevelController(level);
				Debug.Log("LEVEL LOADED: " + levelName);
				LoadEvent?.Invoke(level);
			}

			if (_loadLevels.Count <= 0)
			{
				Debug.Log("ALL LEVELS LOADED");
				AllLoadEvent?.Invoke();
			}
		}
	}
}
#endif