using UnityEngine;

public class AIMovementModel : CharacterMovementModel
{
	private static float DISTANCE_THRESHOLD = 1f;
	
	private CharacterView view;
	private NavigationRouter aggroTarget;
	private bool jump = false;
	private Vector3 movement = Vector3.zero;
	private Raycaster raycaster;
	private Waypoint destination;

	public Waypoint Destination
	{
		get { return destination; }
		set { destination = value; }
	}

	public AIMovementModel(CharacterView view, GameObject targetPlayer, float speed, float jumpPower, int climbLength) : base(view, speed, jumpPower, climbLength)
	{
		aggroTarget = targetPlayer.GetComponent<NavigationRouter>();
		targetPlayer.GetComponent<Kristian.Health>().OnDeath += Deaggro;
		if(aggroTarget == null)
			Deaggro(null);
		raycaster = new Raycaster(3f, "Shootable");
	}

	private void Deaggro(GameObject obj)
	{
		aggroTarget = new NullNavigationRouter();
	}
	
    public override void Recalculate()
	{
		base.Recalculate();
		StopJumping();
		AcquireTarget();
		UpdateDestination();
		CalculateMovement();
		JumpOverPits();
	}

	private void StopJumping()
	{
		jump = false;
	}

	private void AcquireTarget()
	{
		//todo
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
		float distance = Vector3.Distance(destination.Position, view.GetTransform.position);
		return distance < DISTANCE_THRESHOLD;
	}

	private void CalculateMovement()
	{
		Vector3 difference = destination.Position - view.GetTransform.position;
		movement = difference.normalized * speed;
		movement.y = 0;
	}

	private void JumpOverPits()
	{
		Vector3 position = view.GetTransform.position + new Vector3(0f, 1f, 0f);
		Vector3 angle = CreateLookDownAngle();
		RaycastHit? hit = raycaster.CastRay(position, angle);
		if(!hit.HasValue)
			jump = true;
	}

	private Vector3 CreateLookDownAngle()
	{
		Vector3 velocity2d = view.Velocity.normalized;
		velocity2d.y = 0;
		Vector3 angle = velocity2d - view.GetTransform.up;
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
