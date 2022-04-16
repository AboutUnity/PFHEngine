namespace PFH.Conditions
{
	public interface IConditionChecker
	{
		bool Check(AbstractCondition condition, IConditionContext context = null);
	}
}