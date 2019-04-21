using System;
using System.Collections;
using UnityEngine;

// TODO: rename file or class so they're the same
public class Parkour
{
    public delegate void StartParkourUpdate(Vector3 normal, Vector3 velocity);
    public delegate void StopParkourUpdate();
    public StartParkourUpdate startParkourEvent;
    public StopParkourUpdate stopParkourEvent;

    private int climbAngleTolerence;
    private int climbSpeed;
    private int climbLength;
	private Raycaster raycaster;
    private TimedActionFactory timedActionFactory;
    private ITimedAction timer;
    private bool climbing = false;
    private Vector3 lastClimbAxis = Vector3.zero;

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

    public void RegisterEvents(CharacterMovement movement)
    {
        // this redirect is needed because only Parkour should know that jumping stops parkour
        movement.jumpEvent += StopParkour;
    }

    public void ParkourCheck(Vector3 forward, Vector3 position, Vector3 velocity, bool hasClimbed, bool grounded)
    {
        // if we're already climbing
        if(climbing)
        {
            // re-test last direction to see if we need to crest the obstacle
            RaycasterResult result = raycaster.CastWrappedRay(position, lastClimbAxis);
            // if we don't hit anything
            if (!result.HasValue)
            {
                // fall
                StopParkourEarly();
            }
        }
        else if (!hasClimbed && MovingForwards(forward, velocity))
        {
            // Otherwise, try to start climbing
            // vertical wallclimb
            if(MaybeParkour(position, forward, 0, 1f))
                return;
            if(!grounded)
            {
                Vector3 right = Vector3.Cross(Vector3.up, forward);
                if(MaybeParkour(position, right, -1, 0.3f))
                    return;
                if(MaybeParkour(position, -right, 1, 0.3f))
                    return;
            }
        }
    }

    private bool MovingForwards(Vector3 forward, Vector3 velocity)
    {
        Vector3 velocity2D = velocity;
        velocity2D.y = 0;
        return Vector3.Angle(forward, velocity2D.normalized) < climbAngleTolerence;
    }

    private bool MaybeParkour(Vector3 position, Vector3 axis, int sideDir, float upSpeed)
    {
        RaycasterResult result = raycaster.CastWrappedRay(position, axis);
        if (result.HasValue)
        {
            lastClimbAxis = axis;
	        Vector3 velocity = CalculateVelocity(result.Normal, sideDir, upSpeed);
            StartParkour(result.Normal, velocity);
        }
        return result.HasValue;
    }

    private Vector3 CalculateVelocity(Vector3 normal, int sideDir, float upSpeed)
    {
        Vector3 upVel = Vector3.up * climbSpeed * upSpeed;
        Vector3 sideVel = SideAxisFromSurface(normal) * climbSpeed * sideDir;
        return upVel + sideVel;
    }

    private Vector3 SideAxisFromSurface(Vector3 normal)
    {
        Vector3 wallSideAxis = Vector3.Cross(normal, Vector3.up);
        return wallSideAxis.normalized;
    }

    private void StartParkour(Vector3 normal, Vector3 velocity)
    {
        climbing = true;
        startParkourEvent(normal, velocity);
        timer = timedActionFactory.Create(climbLength);
        timer.AddDelayedAction(StopParkour);
        timer.StartTimer();
    }

    private void StopParkour()
    {
        if(climbing)
        {
            stopParkourEvent();
            climbing = false;
        }
    }

    // we need this too to avoid a memory leak in TimedAction
    private void StopParkourEarly()
    {
        if(timer != null)
            timer.PerformActionEarly();
        else
            StopParkour();
    }
}