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
		set {
			destination = value;
			MonoBehaviour.print("setting destination to: " + value);
		}
	}

	public AIMovement(GameObject targetPlayer, float speed, float jumpPower, int climbLength) : base(speed, jumpPower, climbLength)
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
	
    public override void Recalculate(Vector3 velocity, Vector3 position, Vector3 forward)
	{
		base.Recalculate(velocity, position, forward);
		StopJumping();
		AcquireTarget();
		UpdateDestination(position);
		CalculateMovement(position);
		JumpOverPits(velocity, position);
	}

	private void StopJumping()
	{
		jump = false;
	}

	private void AcquireTarget()
	{
		//todo
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
		MonoBehaviour.print(destination);
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
		if(!hit.HasValue)
			jump = true;
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
