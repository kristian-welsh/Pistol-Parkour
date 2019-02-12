using System;
using UnityEngine;

public class Raycaster
{
	private float range;
	private int mask; // default: all layers (~0)

	public Raycaster(float range, int mask = ~0)
	{
		this.range = range;
		this.mask = mask;
	}

	public Raycaster(float range, String mask)
	{
		this.range = range;
		this.mask = LayerMask.GetMask(mask);
	}

	public RaycastHit? CastRay(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		bool hitSomething = Physics.Raycast(ray, out hit, range, mask);
		return hitSomething ? (RaycastHit?)hit : null;
	}
}
