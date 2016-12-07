using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviour
{
	public string _name;

	public bool enabled;

	public abstract bool meetsRequirements (ClickUnit actor, ClickUnit target);

	public abstract void execute (ClickUnit actor, ClickUnit target);

}
