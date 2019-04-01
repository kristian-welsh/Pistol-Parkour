using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
	public Vector3 Velocity { get { return rigidbody.velocity; } }
	public Transform GetTransform { get { return transform; } }

	new private GameObject camera;
	new private Rigidbody rigidbody;
	private float originalDrag;
    private Animator anim;

    void Start()
    {
        camera = transform.GetComponentInChildren<CharacterCamera>().gameObject;
        rigidbody = GetComponent<Rigidbody>();
        Kristian.Health health = GetComponent<Kristian.Health>();
        health.OnDeath += Die;
        anim = transform.GetComponentInChildren<Animator>();
        
        // animations currently screwing with game logic, can't walk properly
        anim.enabled = false;
	}

    public void RegisterEvents(CharacterMovement movement)
    {
        movement.collectGunEvent += CollectGun;
        movement.startParkourEvent += StartParkour;
        movement.stopParkourEvent += StopParkour;
        movement.addForceEvent += AddForce;
        movement.startWalkingEvent += StartWalking;
        movement.stopWalkingEvent += StopWalking;
    }

	private void Die(GameObject obj)
	{
		Destroy(obj);
	}

	// attaches a copy of a gun to the view hierarchy and returns that copy
    public void CollectGun(GameObject newGun)
    {
        // disable old gun
    	GameObject gun = camera.transform.GetChild(0).gameObject;
        camera.transform.DetachChildren();
        gun.SetActive(false);
        Destroy(gun);

        // create and enable new gun
        gun = newGun;
        gun.transform.SetParent(camera.transform);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.SetActive(true);
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

    public void StartWalking()
    {
    
        anim.SetBool("IsWalking", true);
    }

    public void StopWalking()
    {
        anim.SetBool("IsWalking", false);
    }
}
