using UnityEngine;

public class AICamera : CharacterCamera
{
	public GameObject player;

	private Vector3 characterRotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;

	public override void Start ()
	{
		player = GameObject.FindGameObjectsWithTag("Player")[0];
		base.Start();
	}

	public override void FixedUpdate ()
	{
		Vector3 facingDirection = TargetXZFacingDirection();
		characterRotation = Vector3.up * YRotationNeeded(facingDirection);
		base.FixedUpdate();
	}

	private Vector3 TargetXZFacingDirection()
	{
		Vector3 difference = player.transform.position - transform.position;
		difference.y = 0;
		return difference.normalized;
	}

	private float YRotationNeeded(Vector3 direction)
	{
		float amount = Vector3.Angle(Vector3.forward, direction);
		if (direction.x < 0)
			amount = 360 - amount;
		return amount;
	}
	
	protected override Quaternion NewCharRotation()
	{
		return Quaternion.Euler(characterRotation);
	}

	protected override Quaternion NewCameraRotation()
	{
		return Quaternion.Euler(cameraRotation);
	}
}
