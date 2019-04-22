using UnityEngine;

/* Decides which 3D direction an AI will look in, toward the player if one exists
 * This is logical as well as graphical, as it will ensure bullets shot by the AI will be on target
 */
public class AICamera : CharacterCamera
{
	// Transform on the current target to look at, used to fetch position data
	private Transform enemyMidpoint;
	// Current top down rotation of the AI
	private Vector3 characterRotation = Vector3.zero;
	// Current up/down rotation of the AI
	private Vector3 cameraRotation = Vector3.zero;
	// True if there's a player to be looking at currently, false otherwise
	private bool aggro;

	/* Initialize
	 */
	public override void Start ()
	{
		AcquireTarget(GameObject.FindGameObjectsWithTag("Player")[0]);
		base.Start();
	}

	/* Stop looking at the player, they died.
	 * Needs the argument to be a valid Health.OnDeath subscriber
	 */
	private void Deaggro(GameObject obj)
	{
		aggro = false;
	}

	/* Start looking at a player's midpoint, but stop when they die
	 */
	public void AcquireTarget(GameObject targetPlayer)
	{
		// sanity check
		if(targetPlayer != null)
		{
			aggro = true;
			targetPlayer.GetComponent<Kristian.Health>().OnDeath += Deaggro;
			enemyMidpoint = targetPlayer.transform.GetChild(0);
		}
		else
		{
			Deaggro(null);
		}
	}

	/* update camera for new positions of player and AI
	 */
	public override void FixedUpdate ()
	{
		if(aggro)
			CalculateRotations();
		base.FixedUpdate();
	}

	/* Update both rotations
	 */
	private void CalculateRotations()
	{
		characterRotation = Vector3.up * RotationNeeded(Vector3.forward, Direction2D(), -Direction2D().x);
		cameraRotation = Vector3.right * RotationNeeded(Direction2D(), Direction(), Direction().y);
	}

	/* Returns the Euler rotation that needs to be performed on sourceDirection to reach targetDirection
	 * this rotation needs to be performed in the axis sourceDirection cross targetDirection.
	 * For any two vectors in 3d space, there exists some axis in sourceDirection's frame of reference
	 * which contains the only difference between source and target, currentDisplacement is that difference
	 * currentDisplacement: current displacement on the axis that differentiates the two vectors,
	 * this is necessary to differentiate between 270 degrees and 90 degrees from the Vector3.Angle result.
	 * Todo: This is conceptually difficult, and the core functionality of the class, improve clarity.
	 * It may be that this is the general case for any axes, and two simpler functions would be preferable
	 * I think this may be a re-implementation of Quaternion.AngleAxis on euler vectors, maybe use AngleAxis instead
	 */
	private float RotationNeeded(Vector3 sourceDirection, Vector3 targetDirection, float currentDisplacement)
	{
		// smallest rotation needed to get from sourceDirection to targetDirection.
		float amount = Vector3.Angle(sourceDirection, targetDirection);
		if(currentDisplacement > 0)
			amount = 360 - amount;
		return amount;
	}

	/* direction to player from AI without any y component
	 */
	private Vector3 Direction2D()
	{
		Vector3 direction = Direction();
		direction.y = 0;
		return direction;
	}

	/* 3d direction to player from AI
	 */
	private Vector3 Direction()
	{
		return enemyMidpoint.position - transform.position;
	}
	
	/* Returns rotation to set top-down angle to
	 */
	protected override Quaternion NewCharRotation()
	{
		return Quaternion.Euler(characterRotation);
	}

	/* Returns rotation to set up/down (neck) angle to
	 */
	protected override Quaternion NewCameraRotation()
	{
		return Quaternion.Euler(cameraRotation);
	}
}
