using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Action : MonoBehaviour
{
	public string _name;

	public Map map;

	public void Click ()
	{
		Debug.Log ("do this, please.");
		bool rightTurn = false;
		if (map.selectedPlayer.tag.Equals ("Player 1") && map.firstPlayerTurn == true) {
			rightTurn = true;
		}
		if (map.selectedPlayer.tag.Equals ("Player 2") && map.firstPlayerTurn == false) {
			rightTurn = true;
		}
		if (rightTurn && this.meetsRequirements (map.selectedPlayer, map.target) && map.selectedPlayer.player)
		{
			execute (map.selectedPlayer, map.target);
		}
		map.selectedPlayer.movesLeft = -1;
		map.cleanupActions ();
		map.ChangeUnit (map.target.gameObject);
	}

	public abstract bool meetsRequirements (ClickUnit actor, ClickUnit target);

	public abstract void execute (ClickUnit actor, ClickUnit target);

}
