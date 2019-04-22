using System;
using System.Collections;
using UnityEngine;

// TODO: rename file or class so they're the same
/* Decides whether a character should lock into a parkour action in the current curcumstance
 * Calculates data required for the specific parkour action required, publishing it to observers
 * Knows whether to stop a presently occuring action, and publishes any stop occurence to observers
 */
public class Parkour
{
    // Signatures of methods to call when events occur
    public delegate void StartParkourUpdate(Vector3 normal, Vector3 velocity);
    public delegate void StopParkourUpdate();
    // These hold lists of methods of a certain signature to call when events occur
    public StartParkourUpdate startParkourEvent;
    public StopParkourUpdate stopParkourEvent;

    // STARTING:
    // max range of sideways movement when initiating parkour (can't climb when you're facing away)
    private int climbAngleTolerence;
    private int climbSpeed;
    // time in seconds to parkour for before falling
    private int climbLength;
    // detects when climbable objects are close in specific directions
	private Raycaster raycaster;

    // STOPPING:
    // Creates TimedActions in a manner satisfying the Open/Closed principle
    private TimedActionFactory timedActionFactory;
    // Keeps track of when to stop parkour after starting
    private ITimedAction timer;
    // Are we currently engaged in parkour?
    private bool climbing = false;
    // The last time we started parkour, in which direction did we find the climbable object?
    private Vector3 lastClimbAxis = Vector3.zero;

    /* Initialize & recieve dependancies
     */
    public Parkour(int climbAngleTolerence, int climbSpeed, int climbLength, Raycaster raycaster = null, TimedActionFactory timedActionFactory = null)
    {
        this.climbAngleTolerence = climbAngleTolerence;
        this.climbSpeed = climbSpeed;
        this.climbLength = climbLength;
        this.raycaster = raycaster;
        if(this.raycaster == null)
            this.raycaster = new Raycaster(1f, "Climbable");
        this.timedActionFactory = timedActionFactory;
        if(this.timedActionFactory == null)
            this.timedActionFactory = new TimedActionFactoryImplementation();
    }

    /* Stop climbing if the character jumps.
     * Subscribes to movement to be notified when the character jumps.
     */
    public void RegisterEvents(CharacterMovement movement)
    {
        /* re-broadcasts jumpEvents as stopParkourEvents
         * this is needed because only Parkour should know that jumping stops parkour
         * if other objects stopped parkour on jump events, we wouldn't have a seperation of concerns
         */
        movement.jumpEvent += StopParkourEarly;
    }

    /* Check whether the parkour status should change, fulfills the class's purpose
     */
    public void ParkourCheck(Vector3 forward, Vector3 position, Vector3 velocity, bool hasClimbed, bool grounded)
    {
        // Stop climbing/running if we run out of wall
        // if we're already climbing
        if(climbing)
        {
            // re-test last direction to see if we need to crest the obstacle
            RaycasterResult result = raycaster.CastWrappedRay(position, lastClimbAxis);
            // if we don't hit anything
            if (!result.HasValue)
            {
                StopParkourEarly();
            }
        }
        else if (!hasClimbed && MovingForwards(forward, velocity))
        {
            // if we're not climbing, haven't climbed this jump, and are moving, try to start climbing
            // accept the first parkour that is valid in the order: front climb, right run, left run

            // front wallclimb
            if(MaybeParkour(position, forward, 0, 1f))
                return;
            if(!grounded)
            {
                Vector3 right = Vector3.Cross(Vector3.up, forward);
                // right wallrun
                if(MaybeParkour(position, right, -1, 0.3f))
                    return;
                // left wallrun
                if(MaybeParkour(position, -right, 1, 0.3f))
                    return;
            }
        }
    }

    /* You can't climb if you're facing away from the direction you want to move
     */
    private bool MovingForwards(Vector3 forward, Vector3 velocity)
    {
        Vector3 velocity2D = velocity;
        velocity2D.y = 0;
        // angle between facing direction & movement direction < max angle difference
        return Vector3.Angle(forward, velocity2D.normalized) < climbAngleTolerence;
    }

    /* Starts parkour on a wall if there's one in range of position on the specified axis
     * This is the essential core of the class
     * axis: direciton we're looking in to find a climbable wall
     * sideProportion: how much of our climb speed will be horizontal along the wall? [-1, 1]
     * upProportion: how much of our climb speed will be vertical along the wall? [0, 1]
     * returns: bool, did we parkour?
     */
    private bool MaybeParkour(Vector3 position, Vector3 axis, int sideProportion, float upProportion)
    {
        // look for wall
        RaycasterResult result = raycaster.CastWrappedRay(position, axis);
        if (result.HasValue)
        {
            // remember we found a wall in this direction
            lastClimbAxis = axis;
            // decide how to move, and starts that process
	        Vector3 velocity = CalculateVelocity(result.Normal, sideProportion, upProportion);
            StartParkour(result.Normal, velocity);
        }
        // did we start parkour with this call? 
        return result.HasValue;
    }

    /* How will we move in response to this wall?
     */
    private Vector3 CalculateVelocity(Vector3 normal, int sideProportion, float upProportion)
    {
        // multiply climb speed by the proportion for the action being calculated and apply
        // to the appropriate movement axis, the sideways axis must be calculated
        Vector3 upVel = Vector3.up * climbSpeed * upProportion;
        Vector3 sideVel = SideAxisFromSurface(normal) * climbSpeed * sideProportion;
        return upVel + sideVel;
    }

    /* Which direction is sideways from a wall?
     */
    private Vector3 SideAxisFromSurface(Vector3 normal)
    {
        Vector3 wallSideAxis = Vector3.Cross(normal, Vector3.up);
        return wallSideAxis.normalized;
    }

    /* Sets internal state to be able to stop correctly, and publishes startParkourEvent
     */
    private void StartParkour(Vector3 normal, Vector3 velocity)
    {
        climbing = true;
        startParkourEvent(normal, velocity);
        timer = timedActionFactory.Create(climbLength);
        timer.AddDelayedAction(StopParkour);
        timer.StartTimer();
    }

    /* sends a stopParkourEvent if parkour is presently occuring
     */
    private void StopParkour()
    {
        // Avoid sending extraneous stop events, otherwise jumping on the ground would cause havock
        if(climbing)
        {
            stopParkourEvent();
            climbing = false;
        }
    }

    /* Calls the stop function in timer
     * We need this as well as StopParkour to avoid a memory leak in TimedAction
     */
    private void StopParkourEarly()
    {
        if(timer != null)
            timer.PerformActionEarly();
        else
            StopParkour();
    }
}