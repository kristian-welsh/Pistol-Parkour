using UnityEngine;

public class AIShootingInput : ShootingInput
{
	private bool aggressive = true;

	void Start()
	{
		GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
		player.GetComponent<Kristian.Health>().OnDeath += Deaggro;
	}

	private void Deaggro(GameObject obj)
	{
		aggressive = false;
	}

	public void AcquireTarget(GameObject obj)
	{
		aggressive = true;
	}

	public override bool ShouldShoot()
	{
		return aggressive;
	}
}
