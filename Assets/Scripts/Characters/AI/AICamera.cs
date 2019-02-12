using UnityEngine;

public class AICamera : CharacterCamera
{
	public GameObject player;

	private Vector3 charRot = Vector3.zero;
	private Vector3 camRot = Vector3.zero;

	public override void Start ()
	{
		player = GameObject.FindGameObjectsWithTag("Player")[0];
		base.Start();
	}

	public override void FixedUpdate ()
	{
		Vector3 difference = player.transform.position - transform.position;
		difference.y = 0;
		float amount = Vector3.Angle(Vector3.forward, difference.normalized);
		if (difference.x < 0)
			amount = 360 - amount;
		charRot = Vector3.up * amount;
		base.FixedUpdate();
	}
	
	protected override Quaternion NewCharRotation()
	{
		return Quaternion.Euler(charRot);
	}

	protected override Quaternion NewCameraRotation()
	{
		return Quaternion.Euler(camRot);
	}
}
