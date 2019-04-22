using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles the graphical display of a character, subscribes to character's models for update events
 * Not purely a view, because updating graphics changes the state of unity's physics system,
 * which informs the models, in turn updating the view. This class's actions are, however,
 * the last stage of my code's participation in that process.
 */
public class CharacterView : MonoBehaviour
{
    // Find velocity in physics engine
	public Vector3 Velocity { get { return rigidbody.velocity; } }
	public Transform GetTransform { get { return transform; } }
    // camera reference needed to pick up guns
	new private GameObject camera;
    // Unity physics object representing this character
	new private Rigidbody rigidbody;
    // holds drag physics setting while it's zeroed during parkour
	private float originalDrag;
    // Other objects have two stage initialization for which they need to know whether
    // this object has been initialized yet
    public bool initialized = false;

    /* Initialize
     */
    void Start()
    {
        camera = transform.GetComponentInChildren<CharacterCamera>().gameObject;
        rigidbody = GetComponent<Rigidbody>();
        // subscribe to monobehaviours on object
        Kristian.Health health = GetComponent<Kristian.Health>();
        health.OnDeath += Die;
        
        initialized = true;
	}

    /* Subscribe to non-monobehavior models 
     */
    public void RegisterEvents(CharacterMovement movement, Parkour parkour)
    {
        movement.collectGunEvent += CollectGun;
        movement.addForceEvent += AddForce;
        parkour.startParkourEvent += StartParkour;
        parkour.stopParkourEvent += StopParkour;
    }

    /* Disapear when dead
     */
	private void Die(GameObject obj)
	{
		Destroy(obj);
	}

	/* Attach a copy of a gun to the view hierarchy and returns that copy
     */
    public void CollectGun(GameObject newGun)
    {
        // disable & remove old gun
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

    /* Move the character using unity physics
     * Used for jumping and walking currently
     */
    public void AddForce(Vector3 force, ForceMode mode)
    {
		rigidbody.AddForce(force, mode);
    }

    /* Lock in to a certain movement velocity until told otherwise
     * turns off gravity & drag to keep movement consistent
     */
    public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        originalDrag = rigidbody.drag;
		rigidbody.drag = 0;
        rigidbody.useGravity = false;
        rigidbody.velocity = velocity;
    }

    /* Return to moving freely
     */
    public void StopParkour()
    {
        rigidbody.useGravity = true;
        rigidbody.drag = originalDrag;
    }
}
