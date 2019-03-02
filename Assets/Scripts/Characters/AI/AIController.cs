using UnityEngine;

public class AIController : Kristian.CharacterController
{
	protected override CharacterMovementModel CreateMovement(CharacterView view)
	{
        GameObject targetPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        return new AIMovementModel(view, targetPlayer, speed, jumpPower, climbLength);
    }

    public void SetDestination(Waypoint destination)
    {
        ((AIMovementModel)movement).Destination = destination;
    }
}