using System;
using System.Collections.Generic;
using UnityEngine;

/* Maintains a list of all GameObjects with a certian tag currently colliding with this GameObject
 */
public class CollisionLister : MonoBehaviour
{
	public String collisionTag;

	[HideInInspector]
	public List<GameObject> list = new List<GameObject>();

	/* Called by Unity when an object leaves this object's collision area
	 * collider: collider attached to the object leaving
	 */
	void OnTriggerExit(Collider collider)
	{
		if(AllowChange(collider))
			list.Remove(collider.transform.gameObject);
	}

	/* Called by Unity when an object enters this object's collision area
	 * collider: collider attached to the object entering
	 */
	void OnTriggerEnter(Collider collider)
	{
		if(AllowChange(collider))
			list.Add(collider.transform.gameObject);
	}

	/* Checks whether collider has the appropriate tag to be included in the list
	 */
	private bool AllowChange(Collider collider)
	{
		if(collisionTag == null)
			return true;
		if(collider.CompareTag(collisionTag))
			return true;
		return false;
	}
}
