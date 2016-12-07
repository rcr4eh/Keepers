using UnityEngine;
using System.Collections;

public class CleaveAttack : Action {

	public override bool meetsRequirements (ClickUnit actor, ClickUnit target)
	{
		if (actor.movesLeft == -1 || map.Distance ((int)(actor.transform.position.x), (int)(actor.transform.position.y), (int)(target.transform.position.x), (int)(target.transform.position.y)) > 1.8) 
		{
			return false;
		}
		return true;
	}

	public override void execute(ClickUnit actor, ClickUnit target)
	{
		int originalAttack = actor.damage;
		actor.damage = (int)(originalAttack * 2 / 3);
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (!(x == 0 && y == 0)) {
					actor.movesLeft = 1;
					map.attackObject (((int)(actor.transform.position.x + x)), ((int)(actor.transform.position.y) + y));
				}
			}
		}
		actor.damage = originalAttack;
	}
}