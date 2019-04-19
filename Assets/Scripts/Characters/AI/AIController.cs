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

    // todo: introduce interface that all these classes implement with function AcquireTarget
    public void AggroToTarget(GameObject target)
    {
        AICamera camera = gameObject.GetComponentsInChildren<AICamera>()[0];
        AIShootingInput gun = gameObject.GetComponentsInChildren<AIShootingInput>()[0];
        ((AIMovement)movement).AcquireTarget(target);
        camera.AcquireTarget(target);
        gun.AcquireTarget(target);
    }
}