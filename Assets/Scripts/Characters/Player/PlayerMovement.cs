using UnityEngine;

public class PlayerMovement : CharacterMovement
{
	private Vector3 movement = Vector3.zero;
	private bool jump = false;

	public PlayerMovement(float speed, float jumpPower, int climbLength) : base(speed, jumpPower, climbLength) {}

	public void UpdateInput(float horizontalInput, float verticalInput, bool spaceDepressed)
	{
		Vector3 input = new Vector3(horizontalInput, 0f, verticalInput);
		movement = input.normalized * speed;
		if(spaceDepressed)
			jump = true;
	}

	protected override bool wantsToJump()
	{
		bool result = jump;
		jump = false;
		return result;
	}

	protected override Vector3 CalculateMovementForce(Vector3 forward)
	{
		//movement was calculated in world space, transform to player space
		Vector3 right = Vector3.Cross(Vector3.up, forward);
		return forward * movement.z + right * movement.x;
	}
}
