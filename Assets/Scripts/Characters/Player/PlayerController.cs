using UnityEngine;

public class PlayerController : Kristian.CharacterController
{
	protected override CharacterMovement CreateMovement()
	{
        return new PlayerMovement(speed, jumpPower, climbLength);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool spaceDepressed = Input.GetKeyDown("space");
        ((PlayerMovement)movement).UpdateInput(horizontalInput, verticalInput, spaceDepressed);
    }
}