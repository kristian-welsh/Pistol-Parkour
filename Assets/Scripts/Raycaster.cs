using System;
using UnityEngine;

/* Casts rays for clients and returns the result in a way encompasing null as a valid result
 */
public class Raycaster
{
	// how far the ray will travel before determinging no result
	private float range;
	// bitmask showing which layers to interact with. default: all layers (~0)
	private int mask;

	/* Creates a Raycaster that will detect objects on any layer (or combination of layers)
	 */
	public Raycaster(float range, int mask = ~0)
	{
		this.range = range;
		this.mask = mask;
	}

	/* Creates a Raycaster that will detect objects on a specific named layer
	 */
	public Raycaster(float range, String mask)
	{
		this.range = range;
		this.mask = LayerMask.GetMask(mask);
	}

	/* Wraps a raycasting result in a nullable RaycastHit, allowing a no-result return in
	 * the form of null.
	 * Depreciated - bad Object Oriented design, RaycastHit is at Ray's level of abstraction
	 */
	public RaycastHit? CastRay(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		bool hitSomething = Physics.Raycast(ray, out hit, range, mask);
		return hitSomething ? (RaycastHit?)hit : null;
	}

	/* Wraps a raycasting result in an instance of RaycasterResult, listed below
	 */
	public virtual RaycasterResult CastWrappedRay(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		bool hitSomething = Physics.Raycast(ray, out hit, range, mask);
		if(hitSomething)
			return new RaycasterResult(hit);
		return new RaycasterResult();
	}
}

/* Represnts the results of a raycast,
 * hides datapoints about result that the applicaiton doesn't need
 */
public class RaycasterResult
{
	// the low-level result of the raycast
	private RaycastHit? hit;
	public virtual bool HasValue { get { return hit.HasValue; } }
	public virtual Vector3 Normal { get { return hit.Value.normal; } }

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