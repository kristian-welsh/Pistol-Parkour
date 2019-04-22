using System.Collections;
using System;
using UnityEngine;

/* Handles character movement and jumping based on intentions from a decision agent
 * Coordinates interactions for collecting hovering guns
 * This was the first class written and used to be a god object, which explains why gun code is here
 */
public class CharacterMovement
{
    // Signatures of methods to call when events occur
    // I may in the future combine ForceUpdate and JumpUpdate somehow, seems somewhat redundent
    public delegate void CollectGunUpdate(GameObject newGun);
    public delegate void AddForceUpdate(Vector3 force, ForceMode mode);
    public delegate void JumpUpdate();
    // These hold lists of methods of a certain signature to call when events occur
    public CollectGunUpdate collectGunEvent;
    public AddForceUpdate addForceEvent;
    public JumpUpdate jumpEvent;

    // is the character currently standing on solid ground
    private bool grounded = true;
    private float speed;
    private float jumpPower;
    // has the character climbed since jumping last?
    private bool hasClimbed = false;
    private bool climbing = false;
    // normal of the surface that the character may jump from
    private Vector3 jumpNormal;
    // detects relevant objects withing a certain distance in a certain direction of the character
    private Raycaster raycaster;
    // provides intentions to execute for movement direction and jump timing
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

    /* Allow or disallow movements & jumps based on parkour events
     */
    public void RegisterEvents(Parkour parkour)
    {
        parkour.startParkourEvent += StartParkour;
        parkour.stopParkourEvent += StopParkour;
    }

    /* If the gun can currently be picked up, do so.
     */
    public void TouchHoveringGun(GameObject hoveringGun)
    {
        HoveringGun hoverScript = hoveringGun.GetComponent<HoveringGun>();
        if(hoverScript.GetActive())
            CollectGun(hoveringGun, hoverScript);
    }

    /*  Collect a gun and change various states to reflect the change
     */
    private void CollectGun(GameObject hoveringGun, HoveringGun hoverScript)
    {
        // Create a copy of a the usable gun and publish it in an event
        GameObject gunPreset = hoveringGun.transform.GetChild(0).gameObject;
        GameObject myGun = GameObject.Instantiate(gunPreset);
        collectGunEvent(myGun);

        // arm it for use by the character
        myGun.GetComponentInChildren<GunShooting>(true).gameObject.SetActive(true);
        // hide the collectable
        hoverScript.Disapear();
    }

    /* Calculates movements & jumps needed this tick and publish them as events.
     */
    public virtual void Recalculate(Vector3 velocity, Vector3 position, Vector3 forward)
    {
        if (!grounded)
            LandOnGround(velocity, position);
        if (!climbing)
            Move(forward);
        if (canJump() && agent.wantsToJump())
            Jump();
    }

    /* If the character has fallen to the ground, remember they've landed
     * Self documenting
     */
    private void LandOnGround(Vector3 velocity, Vector3 position)
    {
        // optimisaiton by doing intensive math less frequently
        if(FallingDown(velocity))
        {
            RaycasterResult objectBeneath = CastRayDown(position);
            if (IsGround(objectBeneath))
                LandOn(objectBeneath);
        }
    }

    /* Are we moving downwards?
     */
    private bool FallingDown(Vector3 velocity)
    {
        return velocity.y < 0;
    }

    /* Cast a ray downwards and return the result
     */
    private RaycasterResult CastRayDown(Vector3 position)
    {
        return raycaster.CastWrappedRay(position, -Vector3.up);
    }

    /* Is there ground beneath us?
     */
    private bool IsGround(RaycasterResult result)
    {
        return result.HasValue && result.HasTag("Ground");
    }

    /* change state to allow jumping from the object & climbing 
     */
    private void LandOn(RaycasterResult landableObject)
    {
        grounded = true;
        hasClimbed = false;
        climbing = false;
        jumpNormal = landableObject.Normal;
    }

    /* Move in a direction determined by the decision agent
     * using the forward vector as a frame of reference, and publish an event to that effect.
     */
    private void Move(Vector3 forward)
    {
        Vector3 movement = agent.CalculateMovementForce(forward) * speed;
        addForceEvent(movement, ForceMode.Acceleration);
        // characters cannot fly, unity physics handles slopes, jumping & climbing are seperate
        movement.y = 0;
    }

    /* Is the character allowed to jump in their current state?
     */
    private bool canJump()
    {
        return grounded | climbing;
    }

    /* Jump and publish events to that effect
     */
    private void Jump()
    {
        grounded = false;
        Vector3 impulse = jumpNormal * jumpPower;
        addForceEvent(impulse, ForceMode.Impulse);
        jumpEvent();
    }

    /* Set state in response to parkour starting
     */
    public void StartParkour(Vector3 normal, Vector3 velocity)
    {
        hasClimbed = true;
        climbing = true;
        grounded = false;
        jumpNormal = normal;
    }

    /* Set state in response to parkour stopping
     */
    private void StopParkour()
    {
        climbing = false;
    }
}