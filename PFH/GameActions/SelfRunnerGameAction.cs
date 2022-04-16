using System;
using UnityEngine;

namespace PFH.GameActions
{
	[Serializable]
	public abstract class SelfRunnerGameAction : AbstractGameAction, IGameActionRunner
	{
		public override IGameActionRunner Create(IGameActionRunnerCache cache)
		{
			return this;
		}

		public abstract void Execute(AbstractGameAction gameAction, IGameActionContext context, Action<bool> callback);
	}
}