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
	
	void Update ()
	{
		
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
