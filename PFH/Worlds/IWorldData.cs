using System.Collections.Generic;

namespace PFH.Worlds
{
	public interface IWorldData
	{
		List<ILevelData> Levels { get; }
		float CellSize { get; }
		float PreloadRadius { get; }
		float LoadRadius { get; }
		float UnloadRadius { get; }
		float RadiusHorizontal { get; }
		float RadiusOffsetY { get; }
		float CheckTime { get; }
	}
}