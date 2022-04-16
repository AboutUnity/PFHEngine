using System;

namespace PFH.Conditions
{
	public class SRConditionAttribute : SRAttribute
	{
		public SRConditionAttribute() : base(typeof(AbstractCondition))
		{
		}

		public SRConditionAttribute(Type type) : base(type)
		{
		}

		public SRConditionAttribute(Type[] types) : base(types)
		{
		}

		public override void OnCreate(object instance)
		{
			OnChange(instance);
		}

		public override void OnChange(object instance)
		{
#if UNITY_EDITOR
			if (instance is AbstractCondition)
			{
				var cond = (AbstractCondition)instance;
				cond.UpdateConditionName();
			}
#endif
		}
	}
}
