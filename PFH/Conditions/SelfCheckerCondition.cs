namespace PFH.Conditions
{
	public abstract class SelfCheckerCondition : AbstractCondition, IConditionChecker
	{
		public override IConditionChecker Create(IConditionCache cache)
		{
			return this;
		}

		public abstract bool Check(AbstractCondition condition, IConditionContext context = null);
	}
}