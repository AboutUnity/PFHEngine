using System;
using PFH.GameActions;

namespace PFH.GameActions
{
	public class SRGameActionAttribute : SRAttribute
	{
		public SRGameActionAttribute() : base(typeof(AbstractGameAction))
		{
		}

		public SRGameActionAttribute(Type type) : base(type)
		{
		}

		public SRGameActionAttribute(Type[] types) : base(types)
		{
		}

		public override void OnCreate(object instance)
		{
			if (instance is AbstractGameAction gameAction)
			{
				gameAction.ActionName = instance.GetType().Name;
			}
		}
	}
}
