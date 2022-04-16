using System;

namespace PFH.GameActions
{
	[Serializable]
	public abstract class AbstractGameAction
	{
		public string ActionName;

		public abstract IGameActionRunner Create(IGameActionRunnerCache cache);

#if UNITY_EDITOR
		public virtual void OnValidate()
		{
		}
#endif
	}
}