using UnityEngine;
using System.Collections;

public class HealthDisplay : MonoBehaviour {

	float originalScale;

	// Use this for initialization
	void Start () 
	{
		originalScale = this.gameObject.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void change(ClickUnit c)
	{
		this.gameObject.GetComponent<MeshRenderer>().enabled = true;

		(this.gameObject.GetComponent<Transform> ()).transform.localScale = new Vector3(
			(this.gameObject.GetComponent<Transform> ()).transform.localScale.x,
			originalScale*c.health/c.maxHealth,
			(this.gameObject.GetComponent<Transform> ()).transform.localScale.z);
	}
}
