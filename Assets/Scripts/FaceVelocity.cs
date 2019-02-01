using UnityEngine;

public class FaceVelocity : MonoBehaviour
{
    Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Face();
    }

	void FixedUpdate ()
    {
        Face();
    }

    private void Face()
    {
		Vector3 dir = rb.velocity.normalized;
		if(dir != Vector3.zero)
			transform.forward = dir;
    }
}
