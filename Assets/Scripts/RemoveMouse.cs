using UnityEngine;

/* Attached to an object in the scene to remove the graphical cursor for FPS camera use
 */
public class RemoveMouse : MonoBehaviour
{
	/* Called when gameplay starts, hides the mouse
	 */
	public void Start()
	{
		HideMouse();
	}

	private void HideMouse()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	/* Called every frame, unhides the mouse if the user has pressed escape
	 */
	public void Update()
	{
		if (ExitRequested())
			ShowMouse();
	}

	private bool ExitRequested()
	{
		return Input.GetKeyDown("escape");
	}

	private void ShowMouse()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
