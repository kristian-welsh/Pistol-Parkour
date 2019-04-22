using UnityEngine;

/* Makes decisions about how to move as an AI character
 * Accepts another character as a target, and makes decisions to try to kill them.
 * Relies on a navigation system traversing waypoints placed in the unity editor
 */
public class AIMovement : MovementDecisionAgent
{
	// distance from a waypoint at which it'll stop trying to move towards it,
	// larger values create smoother but less accurate paths
	private static float DISTANCE_THRESHOLD = 1f;
	
	// Router to provide waypoints that lead to a target
	private NavigationRouter aggroTarget;
	// should attempt to jump
	private bool jump = false;
	// direction it will decide to move
	private Vector3 movement = Vector3.zero;
	// detects objects in different direcitons to know when to jump
	private Raycaster raycaster;
	// current destination from router
	private Waypoint destination;

	public Waypoint Destination
	{
		get { return destination; }
		set { destination = value; }
	}

	public AIMovement(GameObject targetPlayer)
	{
		// if we have a target, move go to them, otherwise 
		if(targetPlayer == null)
			Deaggro(null);
		else
			AcquireTarget(targetPlayer);
		raycaster = new Raycaster(3f, "Shootable");
	}

	/* Stops chasing a target
	 * The argument is required for this to be a valid listener for player death
	 */
	private void Deaggro(GameObject obj)
	{
		aggroTarget = new NullNavigationRouter();
	}

	/* Chase a target, but stop when it dies, defaulting to some other behaviour
	 * in the future when we have more than one player we may scan the environment for players
	 */
	public void AcquireTarget(GameObject targetPlayer)
	{
		aggroTarget = targetPlayer.GetComponent<NavigationRouter>();
		targetPlayer.GetComponent<Kristian.Health>().OnDeath += Deaggro;
	}
	
    /* Calculates movement decision needed this tick, updating state to reflect decisions made.
	 */
	public void Recalculate(Vector3 velocity, Vector3 position)
	{
		StopJumping();
		UpdateDestination(position);
		CalculateMovement(position);
		JumpOverPits(velocity, position);
	}

	/* Only one jump when it's set to true, don't continuously jump.
	 */
	private void StopJumping()
	{
		jump = false;
	}

	/* If we reach the destination, request a new one from the router
	 */
	private void UpdateDestination(Vector3 position)
	{
		if(ReachedDestination(position))
		{
			// jump if the waypoint tells us to
			if(destination.jump)
				jump = true;
			destination = aggroTarget.NextDestination(destination);
		}
	}

	/* Calculates whether our distance from the destionation waypoint is below the threshold
	 */
	private bool ReachedDestination(Vector3 position)
	{
		float distance = Vector3.Distance(destination.Position, position);
		return distance < DISTANCE_THRESHOLD;
	}

	/* Calculates the direction we need to move to reach our desination.
	 */
	private void CalculateMovement(Vector3 position)
	{
		// Create a vector from us to our destination
		Vector3 difference = destination.Position - position;
		movement = difference.normalized;
		// we cannot fly.
		movement.y = 0;
	}

	/* if there's no ground where we're about to step, jump
	 */
	private void JumpOverPits(Vector3 velocity, Vector3 position)
	{
		// ray from around our hips towards where we'll be stepping next update
		Vector3 rayPosition = position + new Vector3(0f, 1f, 0f);
		Vector3 angle = CreateLookDownAngle(velocity);
		RaycastHit? hit = raycaster.CastRay(rayPosition, angle);
		// if there's nothing there, jump
		if(!hit.HasValue)
			jump = true;
	}

	/* Creates an angle that points towards where we'll be stepping next update
	 */
	private Vector3 CreateLookDownAngle(Vector3 velocity)
	{
		Vector3 velocity2d = velocity.normalized;
		velocity2d.y = 0;
		Vector3 angle = velocity2d - Vector3.up;
		return angle.normalized;
	}

	// The whole class exists to calculate the variables returned here

	/* Do we want to jump this update?
	 */
	public bool wantsToJump()
	{
		return jump;
	}

	/* In which direction do we want to move this update?
	 */
	public Vector3 CalculateMovementForce(Vector3 forward)
	{
		return movement;
	}
}