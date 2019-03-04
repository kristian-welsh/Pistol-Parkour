using System;
using System.Collections;
using UnityEngine;

public class ParkourModel
{
    public CharacterMovement movement;

    private int climbAngleTolerence;
    private int climbSpeed;
	private Raycaster raycaster = new Raycaster(1f, "Climbable");

    public ParkourModel(int climbAngleTolerence, int climbSpeed)
    {
        this.climbAngleTolerence = climbAngleTolerence;
        this.climbSpeed = climbSpeed;
    }

    public void ParkourCheck(Vector3 forward, Vector3 position, Vector3 velocity)
    {
        if (!movement.HasClimbed && MovingForwards(forward, velocity))
        {
            MaybeParkour(position, forward, 0, 1f);
            if(!movement.Grounded)
            {
                Vector3 right = Vector3.Cross(Vector3.up, forward);
		        MaybeParkour(position, right, -1, 0.3f);
		        MaybeParkour(position, -right, 1, 0.3f);
            }
        }
    }

    private bool MovingForwards(Vector3 forward, Vector3 velocity)
    {
        Vector3 velocity2D = velocity;
        velocity2D.y = 0;
        return Vector3.Angle(forward, velocity2D.normalized) < climbAngleTolerence;
    }

    private void MaybeParkour(Vector3 position, Vector3 axis, int sideDir, float upSpeed)
    {
        RaycastHit? hit = raycaster.CastRay(position, axis);
        if (hit.HasValue)
        {
	        Vector3 velocity = CalculateVelocity(hit.Value, sideDir, upSpeed);
        	movement.StartParkour(hit.Value.normal, velocity);
        }
    }

    private Vector3 CalculateVelocity(RaycastHit hit, int sideDir, float upSpeed)
    {
        Vector3 upVel = Vector3.up * climbSpeed * upSpeed;
        Vector3 sideVel = SideAxisFromSurface(hit.normal, hit.transform) * climbSpeed * sideDir;
        return upVel + sideVel;
    }

    private Vector3 SideAxisFromSurface(Vector3 normal, Transform transform)
    {
        Vector3 wallSideAxis = Vector3.Cross(normal, Vector3.up);
        return transform.TransformVector(wallSideAxis).normalized;
    }
}
