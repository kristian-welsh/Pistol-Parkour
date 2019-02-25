using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairClimb : MonoBehaviour
{
	public float stepsize = 1f;

	private Raycaster raycaster;
	new private Rigidbody rigidbody;
	private Vector3 rayVector;

	void Start()
	{
		raycaster = new Raycaster(stepsize*5f, "Ground");
		rigidbody = gameObject.GetComponent<Rigidbody>();
		rayVector = new Vector3(0f, -stepsize, 0f);
	}

	void FixedUpdate()
	{
		Vector3 velocity2d = rigidbody.velocity;
		velocity2d.y = 0;

		Vector3 rayStart = transform.position + velocity2d - rayVector;

		print("rayStart: " + rayStart);
		print("Vector3.down: " + Vector3.down);
		RaycastHit? hit = raycaster.CastRay(rayStart, Vector3.down);
		if(hit.HasValue)
		{
			print(hit.Value.point);
		}
	}
}
