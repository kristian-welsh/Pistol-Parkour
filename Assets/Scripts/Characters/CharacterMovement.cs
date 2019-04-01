using System.Collections;
using UnityEngine;

public class CharacterMovement
{
    public delegate void CollectGunUpdate(GameObject newGun);
    public delegate void StartParkourUpdate(Vector3 velocity);
    public delegate void StopParkourUpdate();
    public delegate void StartWalkingUpdate();
    public delegate void StopWalkingUpdate();
    public delegate void AddForceUpdate(Vector3 force, ForceMode mode);
    public CollectGunUpdate collectGunEvent;
    public StartParkourUpdate startParkourEvent;
    public StopParkourUpdate stopParkourEvent;
    public AddForceUpdate addForceEvent;
    public StartWalkingUpdate startWalkingEvent;
    public StopWalkingUpdate stopWalkingEvent;

    protected bool grounded = true;
    protected float speed;

    private float jumpPower;
    private int climbLength;
    private bool hasClimbed = false;
    private bool climbing = false;
    private Vector3 jumpNormal;
    private IEnumerator stopCurrentProcess;
	private Raycaster raycaster;

    private TimedAction timer;

    public bool HasClimbed { get { return hasClimbed; } }
    public bool Grounded { get { return grounded; } }

    public CharacterMovement(float speed, float jumpPower, int climbLength)
    {
        this.speed = speed;
        this.jumpPower = jumpPower;
        this.climbLength = climbLength;
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
        GameObject myGun = GameObject.Instantiate(gunPreset);
        collectGunEvent(myGun);

        myGun.GetComponentInChildren<GunShooting>(true).gameObject.SetActive(true);
        hoverScript.Disapear();
    }

    public virtual void Recalculate(Vector3 velocity, Vector3 position, Vector3 forward)
    {
        if (!grounded)
            CheckGround(velocity, position);
        if (!climbing)
            Move(forward);
        if (canJump() && wantsToJump())
            Jump();
	}

	private void CheckGround(Vector3 velocity, Vector3 position)
    {
        if(velocity.y < 0)
        {
            RaycastHit? hit = raycaster.CastRay(position, -Vector3.up);
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

    private void Move(Vector3 forward)
    {
		Vector3 movement = CalculateMovementForce(forward);
        //MonoBehaviour.print("addForceEvent: " + addForceEvent);
		addForceEvent(movement, ForceMode.Acceleration);
        movement.y = 0;
        if(grounded && movement.sqrMagnitude > 0)
            startWalkingEvent();
        else
            stopWalkingEvent();
    }

	protected virtual Vector3 CalculateMovementForce(Vector3 forward)
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
		addForceEvent(impulse, ForceMode.Impulse);
		grounded = false;
		climbing = false;
		if (timer != null)
			timer.PerformActionEarly();
	}
	
	public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = normal;
        startParkourEvent(velocity);
        timer = TimedAction.Create(climbLength);
        timer.delayedAction += StopWallclimb;
        timer.StartTimer();
    }

    private void StopWallclimb()
    {
        climbing = false;
        stopParkourEvent();
        stopCurrentProcess = null;
    }
}
