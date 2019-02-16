using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLister : MonoBehaviour
{
	public String collisionTag;

	[HideInInspector]
	public List<GameObject> list = new List<GameObject>();

	void OnTriggerExit(Collider collider)
	{
		if(AllowChange(collider))
			list.Remove(collider.transform.gameObject);
	}

	void OnTriggerEnter(Collider collider)
	{
		if(AllowChange(collider))
			list.Add(collider.transform.gameObject);
	}

	private bool AllowChange(Collider collider)
	{
		if(collisionTag == null)
			return true;
		if(collider.CompareTag(collisionTag))
			return true;
		return false;
	}
}
