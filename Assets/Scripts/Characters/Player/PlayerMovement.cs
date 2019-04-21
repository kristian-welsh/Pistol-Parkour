﻿using UnityEngine;

public class PlayerMovement : MovementDecisionAgent
{
	private Vector3 movement = Vector3.zero;
	private bool jump = false;

	public void UpdateInput(float horizontalInput, float verticalInput, bool spaceDepressed)
	{
		Vector3 input = new Vector3(horizontalInput, 0f, verticalInput);
		movement = input.normalized;
		if(spaceDepressed)
			jump = true;
	}

	public void Recalculate(Vector3 velocity, Vector3 position)
	{
		// conform to interface
	}

	public bool wantsToJump()
	{
		bool result = jump;
		jump = false;
		return result;
	}

	public Vector3 CalculateMovementForce(Vector3 forward)
	{
		//movement was calculated in world space, transform to player space
		Vector3 right = Vector3.Cross(Vector3.up, forward);
		return forward * movement.z + right * movement.x;
	}
}