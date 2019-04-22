using System;
using UnityEngine;

// Abstract class defining an agent making decisions on when to shoot
// should really be an interface & maybe combined with the agent concept in CharacterMovement
public class ShootingInput : MonoBehaviour
{
	public virtual bool ShouldShoot()
	{
		throw new Exception("Unexpected call of base class ShouldShoot");
	}
}
