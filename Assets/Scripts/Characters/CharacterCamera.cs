using UnityEngine;

/* Represents the 3d direction a character is facing
 * as yet not pulled into the MVC architecture
 */
public class CharacterCamera : MonoBehaviour
{
	// unity physics object
	protected Rigidbody charRigid;

	/* Initialize
	 */
	public virtual void Start()
	{
		charRigid = transform.parent.GetComponent<Rigidbody>();
	}
	
	/* Update based on physics information
	 */
	public virtual void FixedUpdate()
	{
		RotateCamera();
		RotateChar();
	}

	/* updates up/down rotation angle from character's perspective
	 */
	private void RotateCamera()
	{
		Quaternion localRot = NewCameraRotation();
		// don't allow characters to break their necks
		localRot.x = Mathf.Clamp(localRot.x, -0.7f, 0.7f);
		transform.localRotation = localRot;
	}

	/* Get the subclass to calculate the new up/down rotation
	 */
	protected virtual Quaternion NewCameraRotation()
	{
		throw new System.Exception("Invalid base call CameraRotationNeeded()");
	}


	/* updates aligned rotation angle (top down from character)
	 */
	private void RotateChar()
	{
		charRigid.MoveRotation(NewCharRotation());
	}

	/* Get the subclass to calculate the new aligned rotation
	 */
	protected virtual Quaternion NewCharRotation()
	{
		throw new System.Exception("Invalid base call CharacterRotationNeeded()");
	}
}
