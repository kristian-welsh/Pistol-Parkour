using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    
    Rigidbody playerRigidbody;

    void Start ()
    {
        playerRigidbody = GetComponent<Rigidbody>();
	}

    // called once per physics update
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Move(h, v);
    }

    private void Move(float h, float v)
    {
        Vector3 movement = new Vector3(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        movement = transform.TransformDirection(movement);
        playerRigidbody.MovePosition(transform.position + movement);
    }
}
