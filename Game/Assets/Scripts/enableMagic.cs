using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enableMagic : MonoBehaviour
{
	public Map map;

	public void Click ()
	{
		map.selectedPlayer.magicEnabled = !map.selectedPlayer.magicEnabled;
	}
}