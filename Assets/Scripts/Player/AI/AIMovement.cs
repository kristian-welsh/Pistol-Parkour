using UnityEngine;

public class AIMovement : CharacterMovement
{
	public float distanceThreshold = 1f;
	public Waypoint destination;

	private NavigationRouter aggroTarget;
	private bool jump = false;
	private Vector3 movement = Vector3.zero;

	public override void Start ()
	{
		aggroTarget = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<NavigationRouter>();
		if(aggroTarget == null)
			aggroTarget = new NullNavigationRouter();
		base.Start();
	}
	
	protected override void FixedUpdate ()
	{
		UpdateDestination();
		CalculateMovement();
		base.FixedUpdate();
	}

	private void UpdateDestination()
	{
		if(ReachedDestination())
			destination = aggroTarget.NextDestination(destination);
	}

	private bool ReachedDestination()
	{
		float distance = Vector3.Distance(destination.transform.position, gameObject.transform.position);
		return distance < distanceThreshold;
	}

	private void CalculateMovement()
	{
		Vector3 difference = destination.transform.position - gameObject.transform.position;
		movement = difference.normalized * speed;
		movement.y = 0;
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
