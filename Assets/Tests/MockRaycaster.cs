using System;
using System.Collections.Generic;
using UnityEngine;

/* Allows tests to inject raycasting results to be provided to models
 */
public class MockRaycaster : Raycaster
{
	Queue<MockRaycasterResult> testHits;

	public MockRaycaster(float range) : base(range)
	{
		testHits = new Queue<MockRaycasterResult>();
	}

	public MockRaycaster(float range, String mask) : base(range, mask)
	{
		testHits = new Queue<MockRaycasterResult>();
	}

	public void addNullResult()
	{
		testHits.Enqueue(new MockRaycasterResult());
	}

	public void addNonGroundResult()
	{
		testHits.Enqueue(new MockRaycasterResult(true));
	}

	public void addGroundResult(Vector3 normal)
	{
		testHits.Enqueue(new MockRaycasterResult(normal));
	}

	public override RaycasterResult CastWrappedRay(Vector3 position, Vector3 direction)
	{
		return testHits.Dequeue();
	}
}

public class MockRaycasterResult : RaycasterResult
{
	private bool hasValue;
	private bool hasTag = false;
	private Vector3 normal = Vector3.zero;
	public override bool HasValue { get { return hasValue; } }
	public override Vector3 Normal { get { return normal; } }
	
	new public bool HasTag(String tagName)
	{
		return hasTag;
	}

	public MockRaycasterResult()
	{
		this.hasValue = false;
	}

	public MockRaycasterResult(bool hasValue)
	{
		this.hasValue = hasValue;
		this.hasTag = false;
	}

	public MockRaycasterResult(Vector3 normal)
	{
		this.hasValue = true;
		this.hasTag = true;
		this.normal = normal;
	}	
}