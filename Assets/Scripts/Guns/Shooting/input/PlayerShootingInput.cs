using UnityEngine;

public class PlayerShootingInput : ShootingInput
{
	public override bool ShouldShoot()
	{
		return Input.GetButton("Fire1");
	}
}
