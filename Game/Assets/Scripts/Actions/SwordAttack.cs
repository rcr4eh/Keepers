using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwordAttack : Action
{

	public double range;

	public override bool meetsRequirements (ClickUnit actor, ClickUnit target)
	{
		if (actor.movesLeft == -1 || map.Distance ((int)(actor.transform.position.x), (int)(actor.transform.position.y), (int)(target.transform.position.x), (int)(target.transform.position.y)) > range) 
		{
			return false;
		}
		return true;
	}

	public override void execute(ClickUnit actor, ClickUnit target)
	{
		map.attackObject ((int)target.transform.position.x, (int)target.transform.position.y);

	}
}
