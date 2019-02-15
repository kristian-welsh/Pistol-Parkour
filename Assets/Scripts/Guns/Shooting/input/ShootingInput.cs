using System;
using UnityEngine;

public class ShootingInput : MonoBehaviour
{
	public virtual bool ShouldShoot()
	{
		throw new Exception("Unexpected call of base class ShouldShoot");
	}
}
