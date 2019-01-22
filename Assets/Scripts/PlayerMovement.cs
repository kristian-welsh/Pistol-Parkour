using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Vector3 movement = new Vector3();
    Rigidbody playerRigidbody;

    void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
        
	}

    // called once per physics update
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Move(h, v);
        Animate(h, v);
    }

    private void Move(float h, float v)
    {
        // todo: move relative to local z axis
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    private void Animate(float h, float v)
    {
        //throw new NotImplementedException();
    }
}
