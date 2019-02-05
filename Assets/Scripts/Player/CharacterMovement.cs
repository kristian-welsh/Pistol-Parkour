using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 1000f;
    public float jumpPower = 20f;
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

    const float RAY_RANGE = 1;

    new Rigidbody rigidbody;
    GameObject myCamera;
    GameObject myGun;
    bool hasClimbed = false;
    bool climbing = false;
    bool grounded = false;
    float originalDrag;
    Vector3 jumpNormal;
    IEnumerator stopCurrentProcess;

    public bool HasClimbed { get { return hasClimbed; } }
    public bool Grounded { get { return grounded; } }

    public virtual void Start()
    {
        myCamera = transform.GetComponentInChildren<CharacterCamera>().gameObject;
        myGun = myCamera.transform.GetChild(0).gameObject;
        rigidbody = GetComponent<Rigidbody>();
        originalDrag = rigidbody.drag;
        jumpNormal = Vector3.up;
    }

    void FixedUpdate()
    {
        if (!grounded)
            CheckGround();
        if (!climbing)
            Move();
        if (canJump() && wantsToJump())
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

    private void Move()
    {
		Vector3 movement = CalculateMovementForce();
		rigidbody.AddForce(movement, ForceMode.Acceleration);
    }

	protected virtual Vector3 CalculateMovementForce()
	{
		throw new System.Exception("Illegal base method call wantsToJump()");
	}

	private bool canJump()
	{
		return grounded | climbing;
	}

	protected virtual bool wantsToJump()
	{
		throw new System.Exception("Illegal base method call wantsToJump()");
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
        Destroy(myGun);
        
        // create and enable new gun
        myGun = Instantiate(gun, myCamera.transform);
        myGun.transform.localPosition = Vector3.zero;
        myGun.transform.localRotation = Quaternion.identity;
        myGun.SetActive(true);
        myGun.GetComponentInChildren<GunShooting>(true).gameObject.SetActive(true);

        hover.Disapear();
    }
	
	public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = normal;
        rigidbody.velocity = velocity;
		originalDrag = rigidbody.drag;
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
