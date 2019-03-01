using System;
using System.Collections;
using UnityEngine;

public class Parkour : MonoBehaviour
{
    public CharacterMovementModel movement;
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

    Rigidbody playerRigidbody;
	Raycaster raycaster;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
		raycaster = new Raycaster(1f, "Climbable");

	}

    void FixedUpdate()
    {
        if (!movement.HasClimbed && MovingForwards())
        {
            MaybeParkour(transform.forward, 0, 1f);
            if(!movement.Grounded)
            {
		        MaybeParkour(transform.right, -1, 0.3f);
		        MaybeParkour(-transform.right, 1, 0.3f);
            }
        }
    }

    private bool MovingForwards()
    {
        Vector3 velocity2D = playerRigidbody.velocity;
        velocity2D.y = 0;
        return Vector3.Angle(transform.forward, velocity2D.normalized) < climbAngleTolerence;
    }

    private void MaybeParkour(Vector3 axis, int sideDir, float upSpeed)
    {
        RaycastHit? hit = raycaster.CastRay(transform.position, axis);
        if (hit.HasValue)
        {
	        Vector3 velocity = CalculateVelocity(hit.Value, sideDir, upSpeed);
        	movement.StartParkour(hit.Value.normal, velocity);
        }
    }

    private Vector3 CalculateVelocity(RaycastHit hit, int sideDir, float foo)
    {
        Vector3 upVel = Vector3.up * climbSpeed * foo;
        Vector3 sideVel = SideAxisFromSurface(hit.normal, hit.transform) * climbSpeed * sideDir;
        return upVel + sideVel;
    }

    private Vector3 SideAxisFromSurface(Vector3 normal, Transform transform)
    {
        Vector3 wallSideAxis = Vector3.Cross(normal, Vector3.up);
        return transform.TransformVector(wallSideAxis).normalized;
    }
}
