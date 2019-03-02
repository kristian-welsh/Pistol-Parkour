using UnityEngine;

public class PlayerController : Kristian.CharacterController
{
	protected override CharacterMovementModel CreateMovement(CharacterView view)
	{
        return new PlayerMovement(view, speed, jumpPower, climbLength);
    }

    void Update()
    {
        ((PlayerMovement)movement).UpdateInput();
    }
}