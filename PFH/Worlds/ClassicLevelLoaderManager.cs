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

		private List<string> _levelLoading = new List<string>();
		private List<ILevelController> _levels = new List<ILevelController>();
		private List<ILevelController> _levelsToUnload = new List<ILevelController>();
		private Dictionary<string, ILevelController> _sceneToLevels = new Dictionary<string, ILevelController>();

		public List<ILevelController> Levels => _levels;
		public bool Loading => _levelLoading.Count > 0;
		
		public ClassicLevelLoaderManager()
		{
			Application.backgroundLoadingPriority = ThreadPriority.Low;
		}

		public void Dispose()
		{
			Application.backgroundLoadingPriority = ThreadPriority.Normal;
		}

		public void InitPreloader(string preloaderLevelName)
		{
			bool hasPreloader = false;
			for (int i = 0; i < SceneManager.sceneCount; ++i)
			{
				var activeScene = SceneManager.GetSceneAt(i);
				if (activeScene.name == preloaderLevelName)
				{
					hasPreloader = true;
					break;
				}
			}

			if (!hasPreloader)
			{
				SceneManager.LoadScene(preloaderLevelName, LoadSceneMode.Additive);
			}
		}
		
		private void RegisterLevelController(ILevelController level)
		{
			_levels.Add(level);
			_sceneToLevels[level.Scene.name] = level;
		}

		private void UnregisterLevelController(ILevelController level)
		{
			var levelName = level.Scene.name;
			_levels.Remove(level);
			if(_sceneToLevels.ContainsKey(levelName))
				_sceneToLevels.Remove(levelName);

			Log.Debug("LEVEL UNLOADED: " + levelName);
		
			if(UnloadEvent != null)
				UnloadEvent(level);
		}
		
		public void ActivateLevel(string levelName)
		{
			if (_sceneToLevels.TryGetValue(levelName, out var level))
			{
				SceneManager.SetActiveScene(level.Scene);
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
			SceneManager.UnloadSceneAsync(level.Scene);
		}
		
		public void UnloadAll(string[] exclude = null)
		{
			for(int i = 0; i < _levels.Count; ++i)
			{
				if(exclude == null || !Array.Exists(exclude, p => p == _levels[i].Scene.name))
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
			ILevelController level = _levels.Find(p => p.Scene.name == levelName);
			UnloadInternal(level);
		}

		public void Load(string levelName)
		{
			if(HasLevel(levelName))
				return;

			UnloadAll();
			Add(levelName);
		}

		public bool Add(string levelName)
		{
			if(HasLevel(levelName))
				return false;
			if (_levelLoading.Contains(levelName))
				return true;

			_levelLoading.Add(levelName);
			if (_levelLoading.Count == 1)
			{
				LoadSceneAsync(levelName);
			}

			return true;
		}
		
		private async void LoadSceneAsync(string levelName)
		{
			await Task.Delay(1);
		
			Application.backgroundLoadingPriority = ThreadPriority.Low;
			var result = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
			if (result != null)
			{
				result.priority = 100;
				result.allowSceneActivation = false;
				while (!result.isDone)
				{
					if (result.progress >= 0.9f)
					{
						await Task.Delay(1);
						result.allowSceneActivation = true;
					}
					await Task.Delay(1);
				}

				ILevelController level = null;
				var levelScene = SceneManager.GetSceneByName(levelName);
				var rootObjects = levelScene.GetRootGameObjects();
				foreach (var rootObject in rootObjects)
				{
					level = rootObject.GetComponent<ILevelController>();
					if (level != null)
					{
						RegisterLevelController(level);
						break;
					}
				}

				_levelLoading.Remove(levelName);

				Log.Debug("LEVEL LOADED: " + levelName);
			
				LoadEvent?.Invoke(level);

				if (_levelLoading.Count > 0)
				{
					LoadSceneAsync(_levelLoading[0]);
				}
				else
				{
					AllLoadEvent?.Invoke();
				}
			}
		}
	}
}