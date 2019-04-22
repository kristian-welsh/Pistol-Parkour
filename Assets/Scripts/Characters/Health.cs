using UnityEngine;

namespace Kristian
{
	/* Manages a health attribute that when 0 determines when an object will "die"
	 * Notifies subscribers of that death when it occurs
	 */
	public class Health : MonoBehaviour
	{
		public int startingHealth = 100;
		// Signature of methods to call when OnDeath occurs
		[HideInInspector]
		public delegate void DeathAction(GameObject obj);
		// Holds a list of methods of a certain signature to call when health reaches 0
		[HideInInspector]
		public event DeathAction OnDeath;
		// Depreciated
		// Used to initiate the respawn mechanic before the event system was implemented
		[HideInInspector]
		public RespawnManager respawner;
		private int health;

		/* Initialize
		 */
		void Start ()
		{
			Reset();
		}

		/* Set health back to its maximum
		 */
		public void Reset()
		{
			health = startingHealth;
		}

		/* Reduce health by some ammount, if 0, send events to subscribers of OnDeath
		 */
		public void Damage(int amount)
		{
			health -= amount;
			if(health < 0)
			{
				health = 0;
				OnDeath(gameObject);
			}
		}
	}
}