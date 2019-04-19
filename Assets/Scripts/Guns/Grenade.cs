using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	public int numBounces = 2;
	public float explosionDisplaylength = 0.5f;
	public int damagePerExplosion = 50;

	GameObject explosion;
	GameObject explosionCollider;
	GameObject model;
	int timesBounced;
	float timer;

	void Start()
	{
		explosion = transform.GetChild(0).gameObject;
		explosionCollider = transform.GetChild(1).gameObject;
		model = transform.GetChild(2).gameObject;
	}

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

	void Update()
	{
		if (Exploded())
		{
			timer += Time.deltaTime;
			if (timer >= explosionDisplaylength)
				HideExplosion();
		}
	}

	private bool Exploded()
	{
		return !model.activeSelf;
	}

	private void Explode()
	{
		Freeze();
		model.SetActive(false);
		explosion.SetActive(true);
		DamageObjects();
	}

    private void DamageObjects()
    {
        CollisionLister lister = explosionCollider.GetComponent<CollisionLister>();
        GameObject[] toDamage = lister.list.ToArray();
        foreach(GameObject obj in toDamage)
        {
            Kristian.Health health = obj.GetComponentInParent<Kristian.Health>();
            if(health != null)
                health.Damage(damagePerExplosion);
        }
    }

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
