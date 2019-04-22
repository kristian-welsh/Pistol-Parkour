using UnityEngine;

/* Allows players to control the 3D direction their character is facing
 */
public class PlayerCamera : CharacterCamera
{
	// how much the camera responds to player inputs
	public float sensitivity = 5f;
	// whether to reverse the direction of up/down rotation, a common FPS user preference
	public bool inverted = false;
	// Simplifies inversion calculation by seperating the conditional from input
	int inversion;

	/* Initialize
	 */
	public override void Start()
	{
		base.Start();
		// calculate inversion multiplier
		inversion = (inverted ? 1 : -1);
	}
	
	/* Calculate the new up/down rotation
	 */
	protected override Quaternion NewCharRotation()
	{
		float amount = Input.GetAxisRaw("Mouse X") * sensitivity;
		// calculate rotation action required to change rotation by amount degrees
		Quaternion rotationAction = Quaternion.AngleAxis(amount, charRigid.transform.up);
		return charRigid.rotation * rotationAction;
	}

	/* Calculate the new aligned rotation
	 */
	protected override Quaternion NewCameraRotation()
	{
		float amount = Input.GetAxisRaw("Mouse Y") * sensitivity * inversion;
		// calculate rotation action required to change rotation by amount degrees
		Quaternion rotationAction = Quaternion.AngleAxis(amount, Vector3.right);
		return transform.localRotation * rotationAction;
	}
}