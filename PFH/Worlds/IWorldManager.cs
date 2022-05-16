using System;
using UnityEngine;

namespace PFH.Worlds
{
	public interface IWorldManager : IDisposable
	{
		event Action<string> TargetLevelChangeEvent;
		
		IWorldData Data { get; }
		bool Active { get; set; }
		Transform Target { get; set; }
		string TargetLevelName { get; }

		void Tick();
	}
}