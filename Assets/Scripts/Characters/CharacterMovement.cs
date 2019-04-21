using System.Collections;
using System;
using UnityEngine;

public class CharacterMovement
{
    public delegate void CollectGunUpdate(GameObject newGun);
    public delegate void StartParkourUpdate(Vector3 velocity);
    public delegate void StopParkourUpdate();
    public delegate void AddForceUpdate(Vector3 force, ForceMode mode);
    public CollectGunUpdate collectGunEvent;
    public StartParkourUpdate startParkourEvent;
    public StopParkourUpdate stopParkourEvent;
    public AddForceUpdate addForceEvent;

    protected bool grounded = true;
    protected float speed;

    private float jumpPower;
    private int climbLength;
    private bool hasClimbed = false;
    private bool climbing = false;
    private Vector3 jumpNormal;
    private Raycaster raycaster;

    private TimedActionFactory timedActionFactory;
    private ITimedAction timer;

    public bool HasClimbed { get { return hasClimbed; } }
    public bool Grounded { get { return grounded; } }

    public CharacterMovement(float speed, float jumpPower, int climbLength, Raycaster raycaster = null, TimedActionFactory timedActionFactory = null)
    {
        jumpNormal = Vector3.up;
        this.speed = speed;
        this.jumpPower = jumpPower;
        this.climbLength = climbLength;
        this.raycaster = raycaster;
        if(this.raycaster == null)
            this.raycaster = new Raycaster(0.1f);
        this.timedActionFactory = timedActionFactory;
        if(this.timedActionFactory == null)
            this.timedActionFactory = new TimedActionFactoryImplementation();
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
            LandOnGround(velocity, position);
        if (!climbing)
            Move(forward);
        if (canJump() && wantsToJump())
            Jump();
    }

    private void LandOnGround(Vector3 velocity, Vector3 position)
    {
        if(FallingDown(velocity))
        {
            RaycasterResult objectBeneath = CastRayDown(position);
            if (IsGround(objectBeneath))
                LandOn(objectBeneath);
        }
    }

    private bool FallingDown(Vector3 velocity)
    {
        return velocity.y < 0;
    }

    private RaycasterResult CastRayDown(Vector3 position)
    {
        return raycaster.CastWrappedRay(position, -Vector3.up);
    }

    private bool IsGround(RaycasterResult result)
    {
        return result.HasValue && result.HasTag("Ground");
    }

    private void LandOn(RaycasterResult landableObject)
    {
        grounded = true;
        hasClimbed = false;
        climbing = false;
        jumpNormal = landableObject.Normal;
    }

    private void Move(Vector3 forward)
    {
        Vector3 movement = CalculateMovementForce(forward);
        addForceEvent(movement, ForceMode.Acceleration);
        movement.y = 0;
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
        timer = timedActionFactory.Create(climbLength);
        timer.AddDelayedAction(stopParkourEarly);
        timer.StartTimer();
    }

    private void stopParkourEarly()
    {
        climbing = false;
        stopParkourEvent();
    }
}