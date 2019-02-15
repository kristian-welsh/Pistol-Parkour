using UnityEngine;

public class AICamera : CharacterCamera
{
	public GameObject player;
	public Transform enemyMidpoint;

	private Vector3 characterRotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;

	public override void Start ()
	{
		player = GameObject.FindGameObjectsWithTag("Player")[0];
		enemyMidpoint = player.transform.GetChild(0);
		base.Start();
	}

	public override void FixedUpdate ()
	{
		characterRotation = Vector3.up * RotationNeeded(Vector3.forward, Direction2D(), -Direction2D().x);
		cameraRotation = Vector3.right * RotationNeeded(Direction2D(), Direction(), Direction().y);
		base.FixedUpdate();
	}

	/* Returns the Euler rotation that needs to be performed on direction2d to reach direction3d
	 * this rotation needs to be performed in the axis direction2d cross direction3d
	 * currentDisplacement: current displacement on the axis that differentiates the two vectors,
	 * this is nessecary to differentiate between 270 degrees and 90 degrees from the Vector3.Angle result.
	 */
	private float RotationNeeded(Vector3 direction2d, Vector3 direction3d, float currentDisplacement)
	{
		float amount = Vector3.Angle(direction2d, direction3d);
		if(currentDisplacement > 0)
			amount = 360 - amount;
		return amount;
	}

	private Vector3 Direction2D()
	{
		Vector3 direction = Direction();
		direction.y = 0;
		return direction;
	}

	private Vector3 Direction()
	{
		return enemyMidpoint.position - transform.position;
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
