using UnityEngine;

/* Provides player specific coordination of character classes, providing and communicating
 * with a PlayerMovement agent.
 */
public class PlayerController : Kristian.CharacterController
{
    // movement input agent controlled by a player
    private PlayerMovement agent;

    /* Agent builder subclass implementation allows us to provide and keep reference to a
     * PlayerMovement agent with dependancy injection.
     */
    protected override MovementDecisionAgent CreateAgent()
    {
        agent = new PlayerMovement();
        return agent;
    }

    /* Updates player input once per drawn frame rather than
     * once per physics update for responsive input.
     */
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool spaceDepressed = Input.GetKeyDown("space");
        agent.UpdateInput(horizontalInput, verticalInput, spaceDepressed);
    }
}