using System;
using UnityEngine;

namespace PFH.Conditions
{
	public class PlatformCondition : SelfCheckerCondition
	{
		public RuntimePlatform[] Platforms;

		public override bool Check(AbstractCondition condition, IConditionContext context = null)
		{
			var result = Array.Exists(Platforms, p => p == Application.platform);
			return result != Not;
		}
	}
}