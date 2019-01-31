using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1000f;
    public float jumpPower = 20f;
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

    const float RAY_RANGE = 1;

    Rigidbody rigidbody;
    GameObject myCamera;
    GameObject myGun;
    bool hasClimbed = false;
    bool climbing = false;
    bool grounded = false;
    int climbableMask;
    float originalDrag;
    Vector3 jumpNormal;
    IEnumerator stopCurrentProcess;

    public bool HasClimbed { get { return hasClimbed; } }
    public bool Grounded { get { return grounded; } }

    void Start()
    {
        climbableMask = LayerMask.GetMask("Climbable");
        myCamera = transform.GetComponentInChildren<Camera>().gameObject;
        myGun = myCamera.transform.GetChild(0).gameObject;
        rigidbody = GetComponent<Rigidbody>();
        originalDrag = rigidbody.drag;
        jumpNormal = Vector3.up;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool space = Input.GetKeyDown("space");

        if (!grounded)
            CheckGround();
        if (!climbing)
            Move(h, v);
        if (canJump() && space)
            Jump();
    }

    private void CheckGround()
    {
        if(rigidbody.velocity.y < 0)
        {
            Ray down = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(down, out hit, 0.1f))
            {
                if (hit.transform.gameObject.CompareTag("Ground"))
                {
                    grounded = true;
                    hasClimbed = false;
                    climbing = false;
                    jumpNormal = hit.normal;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable Gun"))
        {
            HoveringGun hover = other.gameObject.GetComponent<HoveringGun>();
            if(hover.GetActive())
                CollectGun(other.gameObject, hover);
        }
    }

    private void CollectGun(GameObject hoveringGun, HoveringGun hover)
    {
        // todo: dont allow when hovering gun inactive
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

        hover.Disapear();
    }
    
    private bool canJump()
    {
        return grounded | climbing;
    }

    private void Jump()
    {
        Vector3 impulse = jumpNormal * jumpPower;
        rigidbody.AddForce(impulse, ForceMode.Impulse);
        grounded = false;
        climbing = false;
        if (stopCurrentProcess != null)
            StopCoroutine(stopCurrentProcess);
        StopWallclimb();
    }

    private void JumpGround()
    {
        Vector3 impulse = transform.up * jumpPower;
        rigidbody.AddForce(impulse, ForceMode.Impulse);
        grounded = false;
    }

    private void Move(float h, float v)
    {
        Vector3 movement = new Vector3(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        Vector3 vMovement = transform.forward * movement.z;
        Vector3 hMovement = transform.right * movement.x;
        Vector3 fullMovement = vMovement + hMovement;
        rigidbody.AddForce(fullMovement, ForceMode.Acceleration);
    }

    public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = normal;
        rigidbody.velocity = velocity;
        rigidbody.drag = 0;
        rigidbody.useGravity = false;
        stopCurrentProcess = ClimbingTimer();
        StartCoroutine(stopCurrentProcess);
    }

    private IEnumerator ClimbingTimer()
    {
        yield return new WaitForSeconds(climbLength);
        StopWallclimb();
    }

    private void StopWallclimb()
    {
        climbing = false;
        rigidbody.useGravity = true;
        rigidbody.drag = originalDrag;
        stopCurrentProcess = null;
    }
}
