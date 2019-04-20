using System.Collections.Generic;
using UnityEngine;

public class TestableCharacterMovement : CharacterMovement
{
	Queue<Vector3> testMovementForces = new Queue<Vector3>();
	Queue<bool> testJumpInputs = new Queue<bool>();


	public TestableCharacterMovement(float speed, float jumpPower, int climbLength, Raycaster raycaster = null):base(speed, jumpPower, climbLength, raycaster){}

	public void addMovementForce(Vector3 force)
	{
		testMovementForces.Enqueue(force);
	}

	public void addJumpInput(bool jump)
	{
		testJumpInputs.Enqueue(jump);
	}

    protected override Vector3 CalculateMovementForce(Vector3 forward)
    {
		return testMovementForces.Dequeue();
    }

    protected override bool wantsToJump()
    {
		return testJumpInputs.Dequeue();
    }
}
