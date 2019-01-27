using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1000f;
    public float jumpPower = 20f;
    
    Rigidbody playerRigidbody;
    GameObject myCamera;
    GameObject myGun;

    void Start ()
    {
        myCamera = transform.GetComponentInChildren<Camera>().gameObject;
        myGun = myCamera.transform.GetChild(0).gameObject;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable Gun"))
            CollectGun(other.gameObject);
    }

    private void CollectGun(GameObject hoveringGun)
    {
        // todo: dont allow when hovering gun inactive
        print("hello");
        GameObject gun = hoveringGun.transform.GetChild(0).gameObject;

        // disable old gun
        myCamera.transform.DetachChildren();
        myGun.SetActive(false);

        // create and enable new gun
        myGun = Instantiate(gun, myCamera.transform);
        myGun.transform.localPosition = Vector3.zero;
        myGun.transform.localRotation = Quaternion.identity;
        myGun.SetActive(true);
        myGun.GetComponentInChildren<PlayerShooting>(true).gameObject.SetActive(true);
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
