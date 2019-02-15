using UnityEngine;

public class PlayerCamera : CharacterCamera
{
	public float sensitivity = 5f;
	public bool inverted = false;
	int inversion;

	public override void Start()
	{
		base.Start();
		inversion = (inverted ? 1 : -1);
	}
	
	protected override Quaternion NewCharRotation()
	{
		float amount = Input.GetAxisRaw("Mouse X") * sensitivity;
		Quaternion rotationAction = Quaternion.AngleAxis(amount, charRigid.transform.up);
		return charRigid.rotation * rotationAction;
	}

	protected override Quaternion NewCameraRotation()
	{
		float amount = Input.GetAxisRaw("Mouse Y") * sensitivity * inversion;
		Quaternion rotationAction = Quaternion.AngleAxis(amount, Vector3.right);
		return transform.localRotation * rotationAction;
	}
}