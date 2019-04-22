using UnityEngine;

/* Shoot if the player is holding the fire button (left mouse button)
 */
public class PlayerShootingInput : ShootingInput
{
	public override bool ShouldShoot()
	{
		return Input.GetButton("Fire1");
	}
}
