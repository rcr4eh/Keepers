using UnityEngine;
using System.Collections;

public class SwordAttack : Action
{

	public double range;
	public Map map;

	public void Click () {
		//if (this.meetsRequirements (map.selectedUnit, )) {
		//}

	}

	public override bool meetsRequirements (ClickUnit actor, ClickUnit target)
	{
		return true;
	}

	public override void execute(ClickUnit actor, ClickUnit target)
	{

	}
}
