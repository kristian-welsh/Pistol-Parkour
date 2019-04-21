using UnityEngine;

public class AIController : Kristian.CharacterController
{
    private AIMovement agent;

    protected override MovementDecisionAgent CreateAgent()
    {
        GameObject targetPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        agent = new AIMovement(targetPlayer);
        return agent;
    }

    public void SetDestination(Waypoint destination)
    {
        agent.Destination = destination;
    }

    // todo: introduce interface that all these classes implement with function AcquireTarget
    public void AggroToTarget(GameObject target)
    {
        AICamera camera = gameObject.GetComponentsInChildren<AICamera>()[0];
        AIShootingInput gun = gameObject.GetComponentsInChildren<AIShootingInput>()[0];
        agent.AcquireTarget(target);
        camera.AcquireTarget(target);
        gun.AcquireTarget(target);
    }
}