using System;
using System.Collections.Generic;

namespace PFH.Utilities
{
	[Serializable]
	public class BoolValue
	{
		public bool DefaultValue = false;

		private bool? _overrideValue = null;
		private Dictionary<object, bool> _values = new Dictionary<object, bool>();

		public bool Value { get; private set; }

		public void Set(object owner, bool value)
		{
			_values[owner] = value;
			UpdateValue();
		}

		public void Reset(object owner)
		{
			_values.Remove(owner);
			UpdateValue();
		}

		public void SetOverrideValue(bool value)
		{
			_overrideValue = value;
			UpdateValue();
		}

		public void ResetOverrideValue()
		{
			_overrideValue = null;
			UpdateValue();
		}

		public void UpdateValue()
		{
			if (_values.Count <= 0)
			{
				if (_overrideValue != null)
					Value = _overrideValue.Value;
				else
					Value = DefaultValue;
			}
			else
			{
				Value = true;
				foreach (var pair in _values)
				{
					if (pair.Value)
						continue;

					Value = false;
					break;
				}
			}
		}
	}
}
