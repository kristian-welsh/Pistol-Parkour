using UnityEngine;

public class AIMovement : CharacterMovement
{
	public float distanceThreshold = 1f;
	public Waypoint destination;
	
	private NavigationRouter aggroTarget;
	private Rigidbody rb;
	private bool jump = false;
	private Vector3 movement = Vector3.zero;
	private Raycaster raycaster;

	public override void Start ()
	{
		aggroTarget = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<NavigationRouter>();
		rb = GetComponent<Rigidbody>();
		if(aggroTarget == null)
			aggroTarget = new NullNavigationRouter();
		raycaster = new Raycaster(3f, "Shootable");
		base.Start();
	}
	
	protected override void FixedUpdate ()
	{
		StopJumping();
		UpdateDestination();
		CalculateMovement();
		//JumpOverPits();
		base.FixedUpdate();
	}

	private void StopJumping()
	{
		jump = false;
	}

	private void UpdateDestination()
	{
		if(ReachedDestination())
		{
			if(destination.jump)
				jump = true;
			destination = aggroTarget.NextDestination(destination);
		}
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

	private void JumpOverPits()
	{
		Vector3 position = gameObject.transform.position + new Vector3(0f, 1f, 0f);
		Vector3 angle = CreateLookDownAngle();
		RaycastHit? hit = raycaster.CastRay(position, angle);
		if(!hit.HasValue)
			jump = true;
	}

	private Vector3 CreateLookDownAngle()
	{
		Vector3 velocity2d = rb.velocity.normalized;
		velocity2d.y = 0;
		Vector3 angle = velocity2d - gameObject.transform.up;
		return angle.normalized;
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
