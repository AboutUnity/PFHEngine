using System.Collections.Generic;
using UnityEngine;

namespace PFH.Conditions
{
	public class AndCondition : AbstractCondition
	{
		[SerializeReference]
		[SRCondition]
		public List<AbstractCondition> Conditions = new List<AbstractCondition>();

		public override IConditionChecker Create(IConditionCache cache)
		{
			if (cache != null)
				return cache.Get<AndConditionChecker>();
			return new AndConditionChecker(cache);
		}
	}

	public class AndConditionChecker : IConditionChecker
	{
		private readonly IConditionCache _cache;
		
		public AndConditionChecker(IConditionCache cache)
		{
			_cache = cache;
		}
		
		public bool Check(AbstractCondition condition, IConditionContext context = null)
		{
			var cond = (AndCondition)condition;
			
			if (cond.Conditions != null && cond.Conditions.Count > 0)
			{
				for (int i = 0; i < cond.Conditions.Count; ++i)
				{
					if (cond.Conditions[i] == null)
					{
						Debug.LogError($"Condition at {i} is null.");
						continue;
					}

					var checker = cond.Conditions[i].Create(_cache);
					if (!checker.Check(cond.Conditions[i], context))
						return false;
				}
			}

			return true;
		}
	}
}