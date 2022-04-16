namespace PFH.Conditions
{
	public interface IConditionCache
	{
		T Get<T>() where T : IConditionChecker;
		void Release<T>(T checker) where T : IConditionChecker;
	}
}