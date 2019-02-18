﻿using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public Waypoint[] links;
	public bool jump = false;

	public bool IsValidSpawn()
	{
		return !Physics.CheckSphere(transform.position + new Vector3(0f, 2f, 0f), 1.5f);
	}
}
