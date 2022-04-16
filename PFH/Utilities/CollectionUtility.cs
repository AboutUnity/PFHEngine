using System.Collections.Generic;
using UnityEngine;

namespace PFH.Utilities
{
	public static class CollectionUtility
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				int idx = Random.Range(0, list.Count);
				(list[i], list[idx]) = (list[idx], list[i]);
			}
		}
	}
}
