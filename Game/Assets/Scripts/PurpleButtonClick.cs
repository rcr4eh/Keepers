using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurpleButtonClick : MonoBehaviour
{
	public Map map;

	public void Click ()
	{
		GameObject[] players2 = GameObject.FindGameObjectsWithTag ("Player 2 Color");
		foreach (GameObject play in players2) {
			MeshRenderer color = play.GetComponent<MeshRenderer> ();
			GameObject colorChangeUnit = GameObject.Find ("PurpleTeam");
			MeshRenderer colorChange = colorChangeUnit.GetComponent<MeshRenderer> ();
			color.material = colorChange.material;
		}
	}
}