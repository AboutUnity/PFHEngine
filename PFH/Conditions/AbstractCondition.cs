using System;

namespace PFH.Conditions
{
	public enum EqualType
	{
		Less,
		LessOrEqual,
		Equal,
		MoreOrEqual,
		More
	}

	public static class EqualTypeExtension
	{
		public static string GetName(this EqualType type)
		{
			switch(type)
			{
				case EqualType.Less: return "<";
				case EqualType.LessOrEqual: return "<=";
				case EqualType.Equal: return "==";
				case EqualType.MoreOrEqual: return ">=";
				case EqualType.More: return ">";
			}
			return string.Empty;
		}

		public static bool Check<T>(this EqualType type, T v1, T v2) where T : IComparable
		{
			var c = v1.CompareTo(v2);
			return type == EqualType.Less && c < 0 ||
					type == EqualType.LessOrEqual && c <= 0 ||
					type == EqualType.Equal && c == 0 ||
					type == EqualType.MoreOrEqual && c >= 0 ||
					type == EqualType.More && c > 0;
		}
	}
	
	[Serializable]
	public abstract class AbstractCondition
	{
		public string ConditionName;
		public bool Not = false;

		public abstract IConditionChecker Create(IConditionCache cache);

		public virtual void AddChangeEvent(Action action)
		{

		}

		public virtual void RemoveChangeEvent(Action action)
		{

		}

#if UNITY_EDITOR
		public virtual void UpdateConditionName()
		{
			ConditionName = GetType().Name;
		}
#endif
	}
}
