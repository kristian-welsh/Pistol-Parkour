using UnityEngine;

/* points the attached gameobject in the direction it's traveling
 */
public class FaceVelocity : MonoBehaviour
{
    // the rigidbody of the attached object
    Rigidbody rb;

    /* Called when the game starts
     */
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Face();
    }

    /* Called once per physics update
     */
	void FixedUpdate ()
    {
        Face();
    }

    /* points the attached gameobject in the direction it's traveling
     */
    private void Face()
    {
		Vector3 dir = rb.velocity.normalized;
        // only interpret the velocity as a diection if it's a valid direction
		if(dir != Vector3.zero)
			transform.forward = dir;
    }
}
