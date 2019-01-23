using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1000f;
    public float jumpPower = 20f;
    
    Rigidbody playerRigidbody;

    void Start ()
    {
        playerRigidbody = GetComponent<Rigidbody>();
	}
    
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool space = Input.GetKeyDown("space");

        Move(h, v);
        if (space && grounded())
            jump();
    }

    // todo: you can only jump while on the ground
    private bool grounded()
    {
        return true;
    }

    private void jump()
    {
        Vector3 impulse = transform.up * jumpPower;
        playerRigidbody.AddForce(impulse, ForceMode.Impulse);
    }

    private void Move(float h, float v)
    {
        Vector3 movement = new Vector3(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        Vector3 vMovement = transform.forward * movement.z;
        Vector3 hMovement = transform.right * movement.x;
        Vector3 fullMovement = vMovement + hMovement;
        playerRigidbody.AddForce(fullMovement, ForceMode.Acceleration);
    }
}
