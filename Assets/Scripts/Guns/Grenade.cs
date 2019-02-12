using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	public int numBounces = 2;
	public float explosionDisplaylength = 0.5f;

	GameObject explosion;
	GameObject model;
	int timesBounced;
	float timer;

	void Start()
	{
		explosion = transform.GetChild(0).gameObject;
		model = transform.GetChild(1).gameObject;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag("Player"))
		{
			if (Exploded())
			{
				Damage(other.collider.gameObject);
			}
			else
			{
				Explode();
			}
		}
		else
		{
			if (!Exploded())
			{
				timesBounced++;
				if (timesBounced >= numBounces + 1)
					Explode();
			}
		}
	}

	private void Damage(GameObject player)
	{
		//todo: health system
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
		freeze();
		model.SetActive(false);
		explosion.SetActive(true);
	}

	private void freeze()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
	}

	private void HideExplosion()
	{
		Destroy(gameObject);
	}
}
