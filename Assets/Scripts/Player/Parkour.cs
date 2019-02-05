using System;
using System.Collections;
using UnityEngine;

public class Parkour : MonoBehaviour
{
    public CharacterMovement movement;
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

    const float RAY_RANGE = 1;

    Rigidbody playerRigidbody;
    int climbableMask;

    void Start()
    {
        climbableMask = LayerMask.GetMask("Climbable");
        playerRigidbody = GetComponent<Rigidbody>();
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
        RaycastHit? hit = CastRay(axis);
        if (hit.HasValue)
        {
	        Vector3 velocity = CalculateVelocity(hit.Value, sideDir, upSpeed);
        	movement.StartParkour(hit.Value.normal, velocity);
        }
    }

    private RaycastHit? CastRay(Vector3 axis)
    {
        Ray ray = new Ray(transform.position, axis);
        RaycastHit hit;
    	bool hitSomething = Physics.Raycast(ray, out hit, RAY_RANGE, climbableMask);
    	return hitSomething ? (RaycastHit?)hit : null;
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
