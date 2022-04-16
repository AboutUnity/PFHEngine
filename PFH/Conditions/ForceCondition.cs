namespace PFH.Conditions
{
	public class ForceCondition : SelfCheckerCondition
	{
		public bool Value;

		public override bool Check(AbstractCondition condition, IConditionContext context)
		{
			return Value != Not;
		}
	}
}
