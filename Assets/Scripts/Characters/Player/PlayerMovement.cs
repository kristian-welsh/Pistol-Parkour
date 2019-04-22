using UnityEngine;

/* Allows a human player to control a character by giving instructions to the models
 */
public class PlayerMovement : MovementDecisionAgent
{
	// a direction to move in
	private Vector3 movement = Vector3.zero;
	// whether to jump when next asked
	private bool jump = false;

	/* Calculating the values here ahead of time allows for player updates to be fetched
	 * more frequently than required by models, making the game feel more responsive.
	 */
	public void UpdateInput(float horizontalInput, float verticalInput, bool spaceDepressed)
	{
		Vector3 input = new Vector3(horizontalInput, 0f, verticalInput);
		movement = input.normalized;
		if(spaceDepressed)
			jump = true;
	}

	/* Conform to interface, allows AI to make decisions polymorphically
	 */
	public void Recalculate(Vector3 velocity, Vector3 position)
	{
		// intentionally blank
	}

	public bool wantsToJump()
	{
		bool result = jump;
		jump = false;
		return result;
	}

	public Vector3 CalculateMovementForce(Vector3 forward)
	{
		// movement was calculated in world space, transform to player space
		Vector3 right = Vector3.Cross(Vector3.up, forward);
		return forward * movement.z + right * movement.x;
	}
}