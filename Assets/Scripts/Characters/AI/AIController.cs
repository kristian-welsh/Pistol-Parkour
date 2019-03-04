using UnityEngine;

public class AIController : Kristian.CharacterController
{
	protected override CharacterMovement CreateMovement()
	{
        GameObject targetPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        return new AIMovement(targetPlayer, speed, jumpPower, climbLength);
    }

    public void SetDestination(Waypoint destination)
    {
        ((AIMovement)movement).Destination = destination;
    }
}