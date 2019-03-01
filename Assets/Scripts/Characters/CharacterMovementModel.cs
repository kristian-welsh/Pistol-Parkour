using System.Collections;
using UnityEngine;

public class CharacterMovementModel : MonoBehaviour
{
    public float speed = 1000f;
    public float jumpPower = 20f;
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

    public bool HasClimbed { get { return hasClimbed; } }
    public bool Grounded { get { return grounded; } }
	
    protected bool grounded = true;

    private CharacterView view;
    private bool hasClimbed = false;
    private bool climbing = false;
    private Vector3 jumpNormal;
    private IEnumerator stopCurrentProcess;
	private Raycaster raycaster;

    public virtual void Start()
    {
    	view = gameObject.GetComponent<CharacterView>();
        jumpNormal = Vector3.up;
		raycaster = new Raycaster(0.1f);
	}

    public void TouchHoveringGun(GameObject hoveringGun)
    {
        HoveringGun hoverScript = hoveringGun.GetComponent<HoveringGun>();
        if(hoverScript.GetActive())
            CollectGun(hoveringGun, hoverScript);
    }

    private void CollectGun(GameObject hoveringGun, HoveringGun hoverScript)
    {
        GameObject gunPreset = hoveringGun.transform.GetChild(0).gameObject;
        GameObject myGun = view.CollectGun(gunPreset);

        myGun.GetComponentInChildren<GunShooting>(true).gameObject.SetActive(true);
        hoverScript.Disapear();
    }

    public void Recalculate()
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
        if(view.Velocity.y < 0)
        {
            RaycastHit? hit = raycaster.CastRay(transform.position, -transform.up);
            if (hit.HasValue)
            {
                if (hit.Value.transform.gameObject.CompareTag("Ground"))
                {
                    grounded = true;
                    hasClimbed = false;
                    climbing = false;
                    jumpNormal = hit.Value.normal;
                }
            }
        }
    }

    private void Move()
    {
		Vector3 movement = CalculateMovementForce();
		view.AddForce(movement, ForceMode.Acceleration);
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
		view.AddForce(impulse, ForceMode.Impulse);
		grounded = false;
		climbing = false;
		if (stopCurrentProcess != null)
			StopCoroutine(stopCurrentProcess);
		StopWallclimb();
	}
	
	public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = normal;
        view.StartParkour(velocity);
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
        view.StopParkour();
        stopCurrentProcess = null;
    }
}
