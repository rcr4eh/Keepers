using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
	public Map map;

	public void Click ()
	{
		//map.cleanupActions ();
		if (map.firstPlayerTurn) {
			gameObject.GetComponent<Image> ().color = new Color (255 / 255f, 140 / 255f, 0 / 255f);
			map.firstPlayerTurn = false;
		} else if (!map.firstPlayerTurn) {
			gameObject.GetComponent<Image> ().color = new Color (0 / 255f, 255 / 255f, 255 / 255f);
			map.firstPlayerTurn = true;
		}
		map.DestroyTiles ();

		foreach (GameObject g in map.player1Units) {
			if (g != null) {
				ClickUnit u = g.GetComponent<ClickUnit> ();
				u.movesLeft = u.maxMoveDistance;
			}
		}
		foreach (GameObject g in map.player2Units) {
			if (g != null) {
				ClickUnit u = g.GetComponent<ClickUnit> ();
				u.movesLeft = u.maxMoveDistance;
			}
		}
	}
}
//INITIAL COMMIT