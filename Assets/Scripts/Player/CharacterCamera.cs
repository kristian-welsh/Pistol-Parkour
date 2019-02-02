using UnityEngine;

public class CharacterCamera : MonoBehaviour
{

	protected Rigidbody charRigid;

	public virtual void Start()
	{
		charRigid = transform.parent.GetComponent<Rigidbody>();
	}
	
	public virtual void FixedUpdate()
	{
		RotateCamera();
		RotateChar();
	}

	private void RotateCamera()
	{
		transform.rotation = NewCameraRotation();
		Quaternion localRot = transform.localRotation;
		localRot.x = Mathf.Clamp(localRot.x, -0.7f, 0.7f);
		transform.localRotation = localRot;
	}

	protected virtual Quaternion NewCameraRotation()
	{
		throw new System.Exception("Invalid base call CameraRotationNeeded()");
	}

	private void RotateChar()
	{
		charRigid.MoveRotation(NewCharRotation());
	}

	protected virtual Quaternion NewCharRotation()
	{
		throw new System.Exception("Invalid base call CharacterRotationNeeded()");
	}
}
