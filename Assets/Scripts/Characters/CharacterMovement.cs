using System.Collections;
using System;
using UnityEngine;

public class CharacterMovement
{
    public delegate void CollectGunUpdate(GameObject newGun);
    public delegate void AddForceUpdate(Vector3 force, ForceMode mode);
    public delegate void JumpUpdate();
    public CollectGunUpdate collectGunEvent;
    public AddForceUpdate addForceEvent;
    public JumpUpdate jumpEvent;

    protected bool grounded = true;
    protected float speed;

    private float jumpPower;
    private bool hasClimbed = false;
    private bool climbing = false;
    private Vector3 jumpNormal;
    private Raycaster raycaster;
    private MovementDecisionAgent agent;

    public bool HasClimbed { get { return hasClimbed; } }
    public bool Grounded { get { return grounded; } }

    public CharacterMovement(MovementDecisionAgent agent, float speed, float jumpPower, Raycaster raycaster = null)
    {
        jumpNormal = Vector3.up;
        this.agent = agent;
        this.speed = speed;
        this.jumpPower = jumpPower;
        this.raycaster = raycaster;
        if(this.raycaster == null)
            this.raycaster = new Raycaster(0.1f);
    }

    public void RegisterEvents(Parkour parkour)
    {
        parkour.startParkourEvent += StartParkour;
        parkour.stopParkourEvent += StopParkour;
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
        if (canJump() && agent.wantsToJump())
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
        Vector3 movement = agent.CalculateMovementForce(forward) * speed;
        addForceEvent(movement, ForceMode.Acceleration);
        movement.y = 0;
    }

    private bool canJump()
    {
        return grounded | climbing;
    }

    private void Jump()
    {
        grounded = false;
        Vector3 impulse = jumpNormal * jumpPower;
        addForceEvent(impulse, ForceMode.Impulse);
        jumpEvent();
    }
    
    public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = normal;
    }

    private void StopParkour()
    {
        climbing = false;
    }
}