using System;
using System.Collections;
using UnityEngine;

// TODO: rename file or class so they're the same
public class Parkour
{
    public CharacterMovement movement;

    private int climbAngleTolerence;
    private int climbSpeed;
	private Raycaster raycaster;

    public Parkour(int climbAngleTolerence, int climbSpeed, Raycaster raycaster = null)
    {
        this.climbAngleTolerence = climbAngleTolerence;
        this.climbSpeed = climbSpeed;
        this.raycaster = raycaster;
        if(this.raycaster == null)
            this.raycaster = new Raycaster(1f, "Climbable");
    }

    public ParkourResult ParkourCheck(Vector3 forward, Vector3 position, Vector3 velocity, bool hasClimbed, bool grounded)
    {
        ParkourResult result;
        if (!hasClimbed && MovingForwards(forward, velocity))
        {
            // vertical wallclimb
            result = MaybeParkour(position, forward, 0, 1f);
            if(result != null)
                return result;
            if(!grounded)
            {
                Vector3 right = Vector3.Cross(Vector3.up, forward);
                result = MaybeParkour(position, right, -1, 0.3f);
                if(result != null)
                    return result;
                result = MaybeParkour(position, -right, 1, 0.3f);
                if(result != null)
                    return result;
            }
        }
        return null;
    }

    private bool MovingForwards(Vector3 forward, Vector3 velocity)
    {
        Vector3 velocity2D = velocity;
        velocity2D.y = 0;
        return Vector3.Angle(forward, velocity2D.normalized) < climbAngleTolerence;
    }

    private ParkourResult MaybeParkour(Vector3 position, Vector3 axis, int sideDir, float upSpeed)
    {
        RaycasterResult result = raycaster.CastWrappedRay(position, axis);
        if (result.HasValue)
        {
	        Vector3 velocity = CalculateVelocity(result, sideDir, upSpeed);
        	return new ParkourResult(result.Normal, velocity);
        }
        return null;
    }

    private Vector3 CalculateVelocity(RaycasterResult result, int sideDir, float upSpeed)
    {
        Vector3 upVel = Vector3.up * climbSpeed * upSpeed;
        Vector3 sideVel = SideAxisFromSurface(result.Normal, result.Transform) * climbSpeed * sideDir;
        return upVel + sideVel;
    }

    private Vector3 SideAxisFromSurface(Vector3 normal, Transform transform)
    {
        Vector3 wallSideAxis = Vector3.Cross(normal, Vector3.up);
        return transform.TransformVector(wallSideAxis).normalized;
    }
}