﻿using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1000f;
    public float jumpPower = 20f;
    public int climbAngleTolerence = 10;
    public int climbSpeed = 3;
    public int climbLength = 3;

    const float RAY_RANGE = 1;

    Rigidbody playerRigidbody;
    GameObject myCamera;
    GameObject myGun;
    bool hasClimbed = false;
    bool climbing = false;
    bool grounded = false;
    int climbableMask;
    float originalDrag;
    Vector3 jumpNormal;
    IEnumerator stopCurrentProcess;

    void Start()
    {
        climbableMask = LayerMask.GetMask("Climbable");
        myCamera = transform.GetComponentInChildren<Camera>().gameObject;
        myGun = myCamera.transform.GetChild(0).gameObject;
        playerRigidbody = GetComponent<Rigidbody>();
        originalDrag = playerRigidbody.drag;
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
        MaybeParkour();
    }

    private void CheckGround()
    {
        if(playerRigidbody.velocity.y < 0)
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
        playerRigidbody.AddForce(impulse, ForceMode.Impulse);
        grounded = false;
        climbing = false;
        if (stopCurrentProcess != null)
            StopCoroutine(stopCurrentProcess);
        StopWallclimb();
    }

    private void JumpGround()
    {
        Vector3 impulse = transform.up * jumpPower;
        playerRigidbody.AddForce(impulse, ForceMode.Impulse);
        grounded = false;
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

    private void MaybeParkour()
    {

        if (!hasClimbed && MovingForwards())
        {
            MaybeWallClimb();
            //todo: stop climbing when you crest the top of climbable
            //todo: allow player to quickly jump off walls without using climbing and using charges
            //todo: wallrun
            //raycast to sides
            //    if you're in the air and raycast detects wall: wallrun
            //once you're climbing or running: allow turning, stop after pre-determined time or on jump
        }
    }

    private bool MovingForwards()
    {
        Vector3 velocity2D = playerRigidbody.velocity;
        velocity2D.y = 0;
        return Vector3.Angle(transform.forward, velocity2D.normalized) < climbAngleTolerence;
    }

    private void MaybeWallClimb()
    {
        Ray forward = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(forward, out hit, RAY_RANGE, climbableMask))
            Wallclimb(hit);
    }

    private void Wallclimb(RaycastHit hit)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = hit.normal;
        playerRigidbody.velocity = CalculateWallclimbVelocity(hit);
        playerRigidbody.drag = 0;
        playerRigidbody.useGravity = false;
        stopCurrentProcess = ClimbingTimer();
        StartCoroutine(stopCurrentProcess);
    }

    private Vector3 CalculateWallclimbVelocity(RaycastHit hit)
    {
        Vector3 velocity = playerRigidbody.velocity;
        velocity.y = climbSpeed;
        velocity = velocity - ExtractSidewaysMovement(velocity, hit);
        return velocity;
    }
    
    private Vector3 ExtractSidewaysMovement(Vector3 velocity, RaycastHit hit)
    {
        Vector3 wallSideAxis = Vector3.Cross(hit.normal, Vector3.up);
        Vector3 worldWallSideAxis = hit.transform.TransformVector(wallSideAxis);
        return Vector3.Project(velocity, worldWallSideAxis);
    }

    private IEnumerator ClimbingTimer()
    {
        print(0);
        yield return new WaitForSeconds(climbLength);
        print(1);
        StopWallclimb();
    }

    private void StopWallclimb()
    {
        print(2);
        climbing = false;
        playerRigidbody.useGravity = true;
        playerRigidbody.drag = originalDrag;
        stopCurrentProcess = null;
    }
}
