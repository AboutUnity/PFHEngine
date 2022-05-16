using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PFH.Worlds
{
	public class ClassicLevelLoaderManager : ILevelLoaderManager
	{
		public event Action<ILevelController> LoadEvent;
		public event Action<ILevelController> UnloadEvent;
		public event Action AllLoadEvent;

		private readonly List<ILevelData> _datas;
		private readonly List<string> _preloadLevels = new List<string>();
		private readonly List<KeyValuePair<string, AsyncOperation>> _loadLevels = new List<KeyValuePair<string, AsyncOperation>>();
		private readonly List<ILevelController> _levels = new List<ILevelController>();
		private readonly List<ILevelController> _levelsToUnload = new List<ILevelController>();
		private readonly Dictionary<string, ILevelController> _sceneToLevels = new Dictionary<string, ILevelController>();

		public List<ILevelController> Levels => _levels;
		public bool Loading => _preloadLevels.Count > 0 || _loadLevels.Count > 0;
		
		public ClassicLevelLoaderManager(List<ILevelData> datas)
		{
			_datas = datas;
			Application.backgroundLoadingPriority = ThreadPriority.Low;
		}

		public void Dispose()
		{
			Application.backgroundLoadingPriority = ThreadPriority.Normal;
		}

		public void InitLoadedScenes(string preloaderSceneName)
		{
			var hasPreloader = false;
			for (int i = 0; i < SceneManager.sceneCount; ++i)
			{
				var activeScene = SceneManager.GetSceneAt(i);
				if (activeScene.name == preloaderSceneName)
				{
					hasPreloader = true;
				}
				else
				{
					ProcessLevelScene(activeScene);
				}
			}

			if (!hasPreloader)
			{
				SceneManager.LoadScene(preloaderSceneName, LoadSceneMode.Additive);
			}
		}

		private ILevelController ProcessLevelScene(Scene scene)
		{
			var levelData = _datas.Find(p => p.LevelName == scene.name);
			ILevelController level = null;
			var rootObjects = scene.GetRootGameObjects();
			foreach (var rootObject in rootObjects)
			{
				level = rootObject.GetComponent<ILevelController>();
				if (level != null)
				{
					level.SetLevelData(levelData);
					RegisterLevelController(level);
					break;
				}
			}

			return level;
		}
		
		private void RegisterLevelController(ILevelController level)
		{
			_levels.Add(level);
			_sceneToLevels[level.LevelName] = level;
		}

		private void UnregisterLevelController(ILevelController level)
		{
			var levelName = level.LevelName;
			_levels.Remove(level);
			if(_sceneToLevels.ContainsKey(levelName))
				_sceneToLevels.Remove(levelName);

			Debug.Log("LEVEL UNLOADED: " + levelName);
		
			if(UnloadEvent != null)
				UnloadEvent(level);
		}
		
		public void ActivateLevel(string levelName)
		{
			if (_sceneToLevels.TryGetValue(levelName, out var level))
			{
				SceneManager.SetActiveScene(level.gameObject.scene);
			}
		}
		
		public bool HasLevel(string levelName)
		{
			if (_sceneToLevels.TryGetValue(levelName, out var level))
			{
				return level != null;
			}

			return false;
		}

		private void UnloadInternal(ILevelController level)
		{
			UnregisterLevelController(level);
			SceneManager.UnloadSceneAsync(level.gameObject.scene);
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
			if (_preloadLevels.Contains(levelName))
				return true;
		
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

			_preloadLevels.Add(levelName);
			PreloadSceneAsync(levelName);

			return true;
		}
		
		private async void PreloadSceneAsync(string levelName)
		{
			await Task.Delay(1);
		
			Application.backgroundLoadingPriority = ThreadPriority.Low;
			var result = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
			if (result != null)
			{
				result.priority = 100;
				result.allowSceneActivation = false;
			
				Debug.Log("LEVEL LOADING: " + levelName);
			
				while (!result.isDone)
				{
					if (result.progress >= 0.9f)
					{
						_preloadLevels.Remove(levelName);
						_loadLevels.Add(new KeyValuePair<string, AsyncOperation>(levelName, result));

						if (_loadLevels.Count == 1)
						{
							ActivateSceneAsync();
						}

						return;
					}
					await Task.Delay(1);
				}
			}
		}
		
		private async void ActivateSceneAsync()
		{
			await Task.Delay(1);
		
			if(_loadLevels.Count <= 0)
				return;
		
			var pair = _loadLevels[0];
			var levelName = pair.Key;
			var operation = pair.Value;
			operation.allowSceneActivation = true;
		
			while (!operation.isDone)
			{
				await Task.Delay(1);
			}
		
			await Task.Delay(1);
		
			_loadLevels.Remove(pair);

			var levelScene = SceneManager.GetSceneByName(levelName);
			ILevelController level = ProcessLevelScene(levelScene);

			if (level == null)
			{
				Debug.LogError($"Incorrect level scene: {levelName}");
			}
			else
			{
				Debug.Log("LEVEL LOADED: " + levelName);
				LoadEvent?.Invoke(level);
			}

			if (_loadLevels.Count > 0)
			{
				ActivateSceneAsync();
			}
			else
			{
				AllLoadEvent?.Invoke();
			}
		}
	}
}