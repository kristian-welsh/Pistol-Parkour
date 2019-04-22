using UnityEngine;

/* Shoots while there's a player alive
 */
public class AIShootingInput : ShootingInput
{
	// Should currently shoot
	private bool aggressive = true;

	/* Initialize
	 */
	void Start()
	{
		GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
		// stop shooting when the player sends a death event
		player.GetComponent<Kristian.Health>().OnDeath += Deaggro;
	}

	/* stop shooting
	 */
	private void Deaggro(GameObject obj)
	{
		aggressive = false;
	}

	/* Start shooting
	 */
	public void AcquireTarget(GameObject obj)
	{
		aggressive = true;
	}

	/* Return whether to shoot. Conforms to API & fulfils purpose of class
	 */
	public override bool ShouldShoot()
	{
		return aggressive;
	}
}
