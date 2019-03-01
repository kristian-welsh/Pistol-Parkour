using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
	[HideInInspector]
	public delegate void TriggerEnter(Collider other);
	[HideInInspector]
	public event TriggerEnter OnTrigger;
	[HideInInspector]
	public delegate void PhysicsUpdate();
	[HideInInspector]
	public event PhysicsUpdate OnFixedUpdate;

	public Vector3 Velocity { get { return rigidbody.velocity; } }
	public Transform GetTransform { get { return transform; } }

	new private GameObject camera;
	new private Rigidbody rigidbody;
	private float originalDrag;


    void Start()
    {
        camera = transform.GetComponentInChildren<CharacterCamera>().gameObject;
        rigidbody = GetComponent<Rigidbody>();
        Kristian.Health health = GetComponent<Kristian.Health>();
        health.OnDeath += Die;
	}

	private void Die(GameObject obj)
	{
		Destroy(obj);
	}

	void OnTriggerEnter(Collider other)
    {
    	OnTrigger(other);
    }

    void FixedUpdate()
    {
    	OnFixedUpdate();
    }

	// attatches a copy of a gun to the view heirchy and returns that copy
    public GameObject CollectGun(GameObject gunPreset)
    {
        // disable old gun
    	GameObject gun = camera.transform.GetChild(0).gameObject;
        camera.transform.DetachChildren();
        gun.SetActive(false);
        Destroy(gun);

        // create and enable new gun
        gun = Instantiate(gunPreset, camera.transform);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.SetActive(true);

        return gun;
    }

    public void StartParkour(Vector3 velocity)
    {
        originalDrag = rigidbody.drag;
		rigidbody.drag = 0;
        rigidbody.useGravity = false;
        rigidbody.velocity = velocity;
    }

    public void StopParkour()
    {
        rigidbody.useGravity = true;
        rigidbody.drag = originalDrag;
    }

    public void AddForce(Vector3 force, ForceMode mode)
    {
		rigidbody.AddForce(force, mode);
    }
}
