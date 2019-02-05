using UnityEngine;

public class AIMovement : CharacterMovement
{
	public GameObject target;
	public GameObject waypoints;

	private bool jump = false;
	private Vector3 movement = Vector3.zero;
	new private Transform transform;
	private Waypoint[] navRoute;
	private Waypoint destination;

	public override void Start ()
	{
		target = GameObject.FindGameObjectsWithTag("Player")[0];
		transform = GetComponent<Transform>();
		destination = GetNearestWaypoint(target);
		base.Start();
	}
	
	void Update ()
	{
		if(Vector3.Distance(destination.transform.position, transform.position) < 1f)
		{
			movement = Vector3.zero;
			// if we're agroed on a character, set destination to the nearest out of the children of current destination waypoint
		}
		else 
		{
			movement = destination.transform.position - transform.position;
			movement = movement.normalized * speed;
			movement = new Vector3(movement.x, 0f, movement.z);
		}
	}

	private Waypoint GetNearestWaypoint(GameObject obj)
	{
		Waypoint nearest = null;
		float minDistance = float.PositiveInfinity;
		foreach (Transform point in waypoints.transform)
		{
			float distance = Vector3.Distance(point.position, transform.position);
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
