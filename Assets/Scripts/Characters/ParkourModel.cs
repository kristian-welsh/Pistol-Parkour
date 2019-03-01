using System;
using System.Collections;
using UnityEngine;

public class ParkourModel : MonoBehaviour
{
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

    private CharacterMovementModel movement;
    private CharacterView view;
	private Raycaster raycaster;

    void Start()
    {
        movement = gameObject.GetComponent<CharacterMovementModel>();
        view = gameObject.GetComponent<CharacterView>();
        view.OnFixedUpdate += ParkourCheck;
		raycaster = new Raycaster(1f, "Climbable");
	}

    private void ParkourCheck()
    {
        if (!movement.HasClimbed && MovingForwards())
        {
            MaybeParkour(view.GetTransform.forward, 0, 1f);
            if(!movement.Grounded)
            {
		        MaybeParkour(view.GetTransform.right, -1, 0.3f);
		        MaybeParkour(-view.GetTransform.right, 1, 0.3f);
            }
        }
    }

    private bool MovingForwards()
    {
        Vector3 velocity2D = view.Velocity;
        velocity2D.y = 0;
        return Vector3.Angle(view.GetTransform.forward, velocity2D.normalized) < climbAngleTolerence;
    }

    private void MaybeParkour(Vector3 axis, int sideDir, float upSpeed)
    {
        RaycastHit? hit = raycaster.CastRay(view.GetTransform.position, axis);
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
