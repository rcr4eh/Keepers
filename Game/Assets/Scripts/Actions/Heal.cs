using UnityEngine;
using System.Collections;

public class Heal : Action 
{
	public double range;

	public override bool meetsRequirements (ClickUnit actor, ClickUnit target)
	{
		if (actor.movesLeft > -1 && map.Distance ((int)actor.transform.position.x, (int)actor.transform.position.y, (int)target.transform.position.x, (int)target.transform.position.y) <= range) {
			return true;
		} else {
			return false;
		}
	}

	public override void execute (ClickUnit actor, ClickUnit target)
	{
		target.health += actor.heal;
		if (target.health > target.maxHealth)
			target.health = target.maxHealth;
	}
}
