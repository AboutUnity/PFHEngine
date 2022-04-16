using System;
using UnityEngine;

namespace PFH.GameActions
{
	public interface IGameActionRunner
	{
		void Execute(AbstractGameAction gameAction, IGameActionContext context, Action<bool> callback);
	}
}