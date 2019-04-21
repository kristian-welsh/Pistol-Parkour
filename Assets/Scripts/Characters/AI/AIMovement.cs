using UnityEngine;

public class AIMovement : CharacterMovement
{
	private static float DISTANCE_THRESHOLD = 1f;
	
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

	public AIMovement(GameObject targetPlayer, float speed, float jumpPower, int climbLength) : base(speed, jumpPower, climbLength)
	{
		if(targetPlayer == null)
			Deaggro(null);
		else
			AcquireTarget(targetPlayer);
		raycaster = new Raycaster(3f, "Shootable");
	}

	// need the argument to be a valid listener for player death
	private void Deaggro(GameObject obj)
	{
		aggroTarget = new NullNavigationRouter();
	}

	// in the future when we have more than one player we may scan the environment for players 
	public void AcquireTarget(GameObject targetPlayer)
	{
		aggroTarget = targetPlayer.GetComponent<NavigationRouter>();
		targetPlayer.GetComponent<Kristian.Health>().OnDeath += Deaggro;
	}
	
	public override void Recalculate(Vector3 velocity, Vector3 position, Vector3 forward)
	{
		base.Recalculate(velocity, position, forward);
		StopJumping();
		UpdateDestination(position);
		CalculateMovement(position);
		JumpOverPits(velocity, position);
	}

	private void StopJumping()
	{
		jump = false;
	}

	private void UpdateDestination(Vector3 position)
	{
		if(ReachedDestination(position))
		{
			if(destination.jump)
				jump = true;
			destination = aggroTarget.NextDestination(destination);
		}
	}

	private bool ReachedDestination(Vector3 position)
	{
		float distance = Vector3.Distance(destination.Position, position);
		return distance < DISTANCE_THRESHOLD;
	}

	private void CalculateMovement(Vector3 position)
	{
		Vector3 difference = destination.Position - position;
		movement = difference.normalized * speed;
		movement.y = 0;
	}

	private void JumpOverPits(Vector3 velocity, Vector3 position)
	{
		Vector3 rayPosition = position + new Vector3(0f, 1f, 0f);
		Vector3 angle = CreateLookDownAngle(velocity);
		RaycastHit? hit = raycaster.CastRay(rayPosition, angle);
		if(!hit.HasValue) {
			jump = true;
		}
	}

	private Vector3 CreateLookDownAngle(Vector3 velocity)
	{
		Vector3 velocity2d = velocity.normalized;
		velocity2d.y = 0;
		Vector3 angle = velocity2d - Vector3.up;
		return angle.normalized;
	}

	protected override bool wantsToJump()
	{
		return jump;
	}

	protected override Vector3 CalculateMovementForce(Vector3 forward)
	{
		return movement;
	}
}