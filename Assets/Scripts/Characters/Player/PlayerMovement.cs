using UnityEngine;

public class PlayerMovement : CharacterMovementModel
{
	private Vector3 movement = Vector3.zero;
	private bool jump = false;

	public PlayerMovement(CharacterView view, float speed, float jumpPower, int climbLength) : base(view, speed, jumpPower, climbLength) {}

	public void UpdateInput()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		movement = input.normalized * speed;
		if(Input.GetKeyDown("space"))
			jump = true;
	}

	protected override bool wantsToJump()
	{
		bool result = jump;
		jump = false;
		return result;
	}

	protected override Vector3 CalculateMovementForce()
	{
		//movement was calculated in world space, transform to player space
		return view.GetTransform.forward * movement.z + view.GetTransform.right * movement.x;
	}
}
