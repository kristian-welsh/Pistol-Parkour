using System.Collections.Generic;
using UnityEngine;

public class MockMovementDecisionAgent : MovementDecisionAgent
{
	Queue<Vector3> testMovementForces = new Queue<Vector3>();
	Queue<bool> testJumpInputs = new Queue<bool>();
	
    public void Recalculate(Vector3 velocity, Vector3 position)
	{
		// conform to interface
	}

	public void addMovementForce(Vector3 force)
	{
		testMovementForces.Enqueue(force);
	}

	public void addJumpInput(bool jump)
	{
		testJumpInputs.Enqueue(jump);
	}

	public Vector3 CalculateMovementForce(Vector3 forward)
	{
		return testMovementForces.Dequeue();
	}

	public bool wantsToJump()
	{
		return testJumpInputs.Dequeue();
	}
}
