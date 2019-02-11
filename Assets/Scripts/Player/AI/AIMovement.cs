using UnityEngine;

public class AIMovement : CharacterMovement
{
	public float distanceThreshold = 1f;
	public Waypoint destination;

	private static float GROUND_RAYCAST_RANGE = 300f;

	private NavigationRouter aggroTarget;
	private Rigidbody rb;
	private bool jump = false;
	private Vector3 movement = Vector3.zero;
	private int standableMask;

	public override void Start ()
	{
		aggroTarget = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<NavigationRouter>();
		rb = GetComponent<Rigidbody>();
		if(aggroTarget == null)
			aggroTarget = new NullNavigationRouter();
		standableMask = LayerMask.GetMask("Shootable");
		base.Start();
	}
	
	protected override void FixedUpdate ()
	{
		UpdateDestination();
		CalculateMovement();
		JumpOverPits();
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

	private void JumpOverPits()
	{
		Vector3 startPos = gameObject.transform.position - new Vector3(0f, 0f, 0f);

		Vector3 velocity2d = rb.velocity.normalized;
		velocity2d.y = 0;
		Vector3 angle = velocity2d - gameObject.transform.up;
		angle = angle.normalized;

		Ray ray = new Ray(startPos, angle);
		RaycastHit hit;
		bool result = Physics.Raycast(ray, out hit, GROUND_RAYCAST_RANGE, standableMask);
		if(!result)
		{
			jump = true;
		}
	}

	protected override bool wantsToJump()
	{
		print(jump);
		return jump;
	}

	protected override Vector3 CalculateMovementForce()
	{
		return movement;
	}
}
