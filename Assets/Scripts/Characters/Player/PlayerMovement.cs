using UnityEngine;

public class PlayerMovement : CharacterMovementModel
{
	private Vector3 movement = Vector3.zero;
	private bool jump = false;

	protected override bool wantsToJump()
	{
		bool result = jump;
		jump = false;
		return result;
	}

	void Update()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		movement = input.normalized * speed;
		if(Input.GetKeyDown("space"))
			jump = true;
	}

	protected override Vector3 CalculateMovementForce()
	{
		//movement was calculated in world space, transform to player space
		return transform.forward * movement.z + transform.right * movement.x;
	}
}
