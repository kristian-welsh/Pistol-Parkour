using System;
using UnityEngine;

/* A grenade that bounces a number of times and then explodes when it next touches anything
 * This explosion damages combatants
 * Will explode early if directly touches a combatant
 */
public class Grenade : MonoBehaviour
{
	// number of non-exploding bounces
	public int numBounces = 2;
	// how long to display the explosion for
	public float explosionDisplaylength = 0.5f;
	// how much damage to do (default 50/100)
	public int damagePerExplosion = 50;

	// graphical explosion gameobject
	GameObject explosion;
	// logical explosion gameobject
	GameObject explosionCollider;
	// graphical & logical grenade gameobject
	GameObject model;
	int timesBounced;
	// number of seconds passed since the explosion started displaying
	float timer;

	/* Called when the object is created
	 */
	void Start()
	{
		explosion = transform.GetChild(0).gameObject;
		explosionCollider = transform.GetChild(1).gameObject;
		model = transform.GetChild(2).gameObject;
	}

	/* Explodes when numBounces exceeded
	 */
	void OnCollisionEnter(Collision other)
	{
		if (!Exploded())
		{
			if (other.collider.CompareTag("Player"))
			{
				Explode();
			}
			else
			{
				timesBounced++;
				if (timesBounced >= numBounces + 1)
					Explode();
			}
		}
	}

	/* Hide the graphical explosion after a certain length of time
	 */
	void Update()
	{
		if (Exploded())
		{
			timer += Time.deltaTime;
			if (timer >= explosionDisplaylength)
				HideExplosion();
		}
	}

	/* true if exploded, false otherwise
	 */
	private bool Exploded()
	{
		return !model.activeSelf;
	}

	/* transitions from unexploded state to exploded state & damages players in area
	 */
	private void Explode()
	{
		Freeze();
		model.SetActive(false);
		explosion.SetActive(true);
		DamageObjects();
	}

	/* damages any combatents in the explosion area, potential futurre support for breakable objects
	 */
    private void DamageObjects()
    {
		// lists present gameobjects
        CollisionLister lister = explosionCollider.GetComponent<CollisionLister>();
        GameObject[] toDamage = lister.list.ToArray();
        foreach(GameObject obj in toDamage)
        {
			// if it has a health object, it can be damaged
            Kristian.Health health = obj.GetComponentInParent<Kristian.Health>();
            if(health != null)
                health.Damage(damagePerExplosion);
        }
    }

	/* Stop moving
	 */
	private void Freeze()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
	}

	private void HideExplosion()
	{
		explosion.SetActive(false);
		explosionCollider.SetActive(false);
		model.SetActive(false);
		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
