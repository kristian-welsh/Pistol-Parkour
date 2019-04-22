using UnityEngine;

/* Provides AI specific coordination of character classes, providing and communicating
 * with an AIMovement agent.
 */
public class AIController : Kristian.CharacterController
{
    // makes decisions about model's actions
    private AIMovement agent;

    /* Agent builder subclass implementation allows us to provide and keep reference to an
     * AIMovement agent with dependancy injection.
     */
    protected override MovementDecisionAgent CreateAgent()
    {
        GameObject targetPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        agent = new AIMovement(targetPlayer);
        return agent;
    }

    /* Pass on a destination to the agent.
     */
    public void SetDestination(Waypoint destination)
    {
        agent.Destination = destination;
    }

    /* Sets a target on various AI components
     */
    public void AggroToTarget(GameObject target)
    {
        AICamera camera = gameObject.GetComponentsInChildren<AICamera>()[0];
        AIShootingInput gun = gameObject.GetComponentsInChildren<AIShootingInput>()[0];
        agent.AcquireTarget(target);
        camera.AcquireTarget(target);
        gun.AcquireTarget(target);
    }
}