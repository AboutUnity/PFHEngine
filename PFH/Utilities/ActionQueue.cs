using System;
using System.Collections.Generic;

namespace PFH.Utilities
{
	public class ActionQueue
	{
		private List<Action<Action<bool>>> _actions = new List<Action<Action<bool>>>();
		private int _index;
		private Action<bool> _callback;
		private bool _running = false;

		public void Add(Action<Action<bool>> action)
		{
			if (_running)
				throw new Exception("ActionQueue already running.");

			_actions.Add(action);
		}

		private void Next(bool success)
		{
			if (!success || _index >= _actions.Count)
			{
				if (!success)
					Log.Error("Fail!");

				_running = false;
				if (_callback != null)
					_callback(success);
				return;
			}

			var action = _actions[_index];
			_index++;
			action(Next);
		}

		public void Run(Action<bool> callback)
		{
			if (_running)
				throw new Exception("ActionQueue already running.");

			_running = true;
			_index = 0;
			_callback = callback;
			Next(true);
		}
	}
}
