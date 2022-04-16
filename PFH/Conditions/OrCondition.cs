using System.Collections.Generic;
using UnityEngine;

namespace PFH.Conditions
{
	public class OrCondition : AbstractCondition
	{
		[SerializeReference]
		[SRCondition]
		public List<AbstractCondition> Conditions = new List<AbstractCondition>();

		public override IConditionChecker Create(IConditionCache cache)
		{
			if (cache != null)
				return cache.Get<OrConditionChecker>();
			return new OrConditionChecker(cache);
		}
	}

	public class OrConditionChecker : IConditionChecker
	{
		private readonly IConditionCache _cache;
		
		public OrConditionChecker(IConditionCache cache)
		{
			_cache = cache;
		}
		
		public bool Check(AbstractCondition condition, IConditionContext context = null)
		{
			var cond = (OrCondition)condition;
			
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
					if (checker.Check(cond.Conditions[i], context))
						return true;
				}
			}

			return false;
		}
	}
}