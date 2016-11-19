using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorButtonClick : MonoBehaviour
{
	public Map map;

	public void Click ()
	{
		GameObject[] play2col = GameObject.FindGameObjectsWithTag ("Player 2 Color");
		foreach (GameObject color in play2col) {
			MeshRenderer Mesh = color.GetComponent<MeshRenderer> ();
			GameObject greenTeam = GameObject.Find ("GreenTeam");
			MeshRenderer Mesh2 = greenTeam.GetComponent<MeshRenderer> ();
			Mesh.material = Mesh2.material;
		}
	}
}