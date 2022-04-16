namespace PFH.GameActions
{
	public interface IGameActionRunnerCache
	{
		T Get<T>() where T : IGameActionRunner;
		void Release<T>(T runner) where T : IGameActionRunner;
	}
}