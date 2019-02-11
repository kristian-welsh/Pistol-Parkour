using UnityEngine;

public class AIMovement : CharacterMovement
{
	public GameObject waypoints;
	public float distanceThreshold = 1f;

	private GameObject aggro;
	private bool jump = false;
	private Vector3 movement = Vector3.zero;
	new private Transform transform;
	private Waypoint[] navRoute;
	private Waypoint destination;

	public override void Start ()
	{
		//aggro = GameObject.FindGameObjectsWithTag("Player")[0];
		transform = GetComponent<Transform>();
		destination = GetNearestWaypoint(gameObject);
		base.Start();
	}
	
	void FixedUpdate ()
	{
		if(Vector3.Distance(destination.transform.position, transform.position) < distanceThreshold)
		{
			movement = Vector3.zero;
			UpdateDestination();
		}
		else 
		{
			movement = destination.transform.position - transform.position;
			movement = movement.normalized * speed;
			print("enemy movement.normalized: " + movement.normalized);
			movement = new Vector3(movement.x, 0f, movement.z);
		}
	}

	private void UpdateDestination()
	{
		if(aggro != null)
		{
			destination = GetNearestWaypointLink(destination, aggro);
		}
		else
		{
			destination = GetRandomLink(destination);
		}
	}

	private Waypoint GetRandomLink(Waypoint start)
	{
		return randomElement<Waypoint>(start.links);
	}

	private T randomElement<T>(T[] array)
	{
		return array[randInt(0, array.Length)];
	}

	private int randInt(int start, int end)
	{
		System.Random r = new System.Random();
		return r.Next(start, end);
	}

	private Waypoint GetNearestWaypointLink(Waypoint start, GameObject targetObj)
	{
		Waypoint nearest = null;
		float minDistance = float.PositiveInfinity;
		foreach (Waypoint point in start.links)
		{
			float distance = Vector3.Distance(point.transform.position, targetObj.transform.position);
			if(distance < minDistance)
			{
				minDistance = distance;
				nearest = point;
			}
		}
		return nearest;
	}

	private Waypoint GetNearestWaypoint(GameObject obj)
	{
		Waypoint nearest = null;
		float minDistance = float.PositiveInfinity;
		foreach (Transform point in waypoints.transform)
		{
			float distance = Vector3.Distance(point.position, obj.transform.position);
			if(distance < minDistance)
			{
				minDistance = distance;
				nearest = point.GetComponent<Waypoint>();
			}
		}
		return nearest;
	}

	protected override bool wantsToJump()
	{
		return jump;
	}

	protected override Vector3 CalculateMovementForce()
	{
		return movement;
	}
}
