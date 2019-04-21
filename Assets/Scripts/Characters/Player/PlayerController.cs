using UnityEngine;

public class PlayerController : Kristian.CharacterController
{
    private PlayerMovement agent;

    protected override MovementDecisionAgent CreateAgent()
    {
        agent = new PlayerMovement();
        return agent;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool spaceDepressed = Input.GetKeyDown("space");
        agent.UpdateInput(horizontalInput, verticalInput, spaceDepressed);
    }
}