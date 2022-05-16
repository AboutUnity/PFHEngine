using System;
using System.Collections.Generic;
using UnityEngine;

namespace PFH.Worlds
{
	public class WorldManager : IWorldManager
	{
		public event Action<string> TargetLevelChangeEvent;
		
		public enum CheckState
		{
			Preload,
			Load,
			Unload
		}
		
		private readonly ILevelLoaderManager _levelLoaderManager;
		private readonly IWorldLevelProcessor _levelProcessor;
		
		private bool _active = false;
		private CheckState _checkState = CheckState.Preload;
		private float _checkTimer;
		
		private Transform _target;
		private string _targetLevelName;
		private int _targetLevelId;
		
		private List<string> _needLoad = new List<string>();
		private List<string> _notUnload = new List<string>();
		private List<string> _loadedLevels = new List<string>();

		public IWorldData Data { get; }

		public bool Active 
		{
			get { return _active; }
			set
			{
				_active = value;
				_checkState = CheckState.Preload;
				_checkTimer = 0f;
			}
		}

		public Transform Target
		{
			get => _target;
			set
			{
				_target = value;
			}
		}

		public string TargetLevelName => _targetLevelName;

		public WorldManager(ILevelLoaderManager levelLoaderManager, IWorldLevelProcessor levelProcessor, IWorldData data)
		{
			_levelLoaderManager = levelLoaderManager;
			_levelProcessor = levelProcessor;
			Data = data;
			
			_levelLoaderManager.LoadEvent += OnLevelLoaded;

			Active = false;
		}

		public void Dispose()
		{
			_levelLoaderManager.LoadEvent -= OnLevelLoaded;
			Active = false;
		}

		public void Tick()
		{
			if(Active && _target != null)
			{
				_checkTimer -= Time.deltaTime;
				if (_checkTimer <= 0f)
				{
					if (_checkState == CheckState.Preload)
					{
						CheckPreloadLevels();
						_checkState = CheckState.Load;
					}
					else if (_checkState == CheckState.Load)
					{
						CheckLoadLevels();
						_checkState = CheckState.Unload;
					}
					else
					{
						CheckUnloadLevels();
						_checkState = CheckState.Preload;
					}

					_checkTimer = Data.CheckTime;
				}
			}
		}
		
		private void OnLevelLoaded(ILevelController level)
		{
			if (_levelProcessor != null)
			{
				_levelProcessor.Process(level);
			}
		}

		private void CheckPreloadLevels()
		{
			//TODO need implements
		}

		private void CheckLoadLevels()
		{
			_needLoad.Clear();
			var nextTargetLevelName = CalcLevels(_target.position.x, _target.position.z, Data.LoadRadius, _needLoad);
		
			if(_targetLevelName != nextTargetLevelName)
			{
				_targetLevelName = nextTargetLevelName;
				_levelLoaderManager.ActivateLevel(_targetLevelName);
				TargetLevelChangeEvent?.Invoke(_targetLevelName);
			}
			
			for(int i = 0; i < _needLoad.Count; ++i)
			{
				if(!_loadedLevels.Contains(_needLoad[i]))
				{
					_levelLoaderManager.Add(_needLoad[i]);
					_loadedLevels.Add(_needLoad[i]);
				}
			}
		}

		private void CheckUnloadLevels()
		{
			_notUnload.Clear();

			CalcLevels(_target.position.x, _target.position.z, Data.UnloadRadius, _notUnload);
			for(int i = _loadedLevels.Count - 1; i >= 0; --i)
			{
				if(!_notUnload.Contains(_loadedLevels[i]))
				{
					_levelLoaderManager.Unload(_loadedLevels[i]);
					_loadedLevels.RemoveAt(i);
				}
			}
		}
		
		private string CalcLevels(float x, float y, float radius, List<string> levels)
		{
			string result = null;
			var p = new Vector2(x, y);
		
			var ry = y + Data.RadiusOffsetY;
			var wRadius = radius * Data.RadiusHorizontal;
			var r = new Rect(x - wRadius, ry - radius, wRadius * 2f, radius * 2f);
		
			for (int i = 0; i < Data.Levels.Count; ++i)
			{
				if (result == null && Data.Levels[i].Rect.Contains(p))
				{
					result = Data.Levels[i].LevelName;
				}

				if (Data.Levels[i].Rect.Overlaps(r))
				{
					levels.Add(Data.Levels[i].LevelName);
				}
			}

			return result;
		}
	}
}