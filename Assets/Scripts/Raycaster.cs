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

	// depreciated
	public RaycastHit? CastRay(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		bool hitSomething = Physics.Raycast(ray, out hit, range, mask);
		return hitSomething ? (RaycastHit?)hit : null;
	}

	// return wrapped result
	public RaycasterResult CastWrappedRay(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		bool hitSomething = Physics.Raycast(ray, out hit, range, mask);
		if(hitSomething)
			return new RaycasterResult(hit);
		return new RaycasterResult();
	}
}

public class RaycasterResult
{
	private RaycastHit? hit;
	public bool HasValue { get { return hit.HasValue; } }
	public Vector3 Normal { get { return hit.Value.normal; } }

	public bool HasTag(String tagName)
	{
		if(hit.HasValue)
			return hit.Value.transform.gameObject.CompareTag(tagName);
		MonoBehaviour.print("WARNING: calling HasTag on a RaycastResult without a value, returning false");
		return false;
	}

	public RaycasterResult()
	{
		// intentionally blank
	}

	public RaycasterResult(RaycastHit hit)
	{
		this.hit = hit;
	}
}