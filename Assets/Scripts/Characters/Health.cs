using UnityEngine;

namespace Kristian
{
	public class Health : MonoBehaviour
	{
		public int startingHealth = 100;

		[HideInInspector]
		public delegate void DeathAction(GameObject obj);

		[HideInInspector]
		public event DeathAction OnDeath;

		[HideInInspector]
		public RespawnManager respawner;

		private int health;

		void Start ()
		{
			Reset();
		}

		public void Reset()
		{
			health = startingHealth;
		}

		public void Damage(int amount)
		{
			health -= amount;
			if(health < 0)
			{
				health = 0;
				OnDeath(gameObject);
				Destroy(gameObject);
			}
		}
	}
}