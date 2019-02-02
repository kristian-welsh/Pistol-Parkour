using UnityEngine;

public class PlayerMovement : CharacterMovement
{
	protected override bool wantsToJump()
	{
		return Input.GetKeyDown("space");
	}

	protected override Vector3 CalculateMovementForce()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Vector3 input = new Vector3(h, 0f, v);
		Vector3 movement = input.normalized * speed * Time.deltaTime;
		return transform.forward * movement.z + transform.right * movement.x;
	}
}
