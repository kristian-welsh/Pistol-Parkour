using UnityEngine;

public class RemoveMouse : MonoBehaviour
{
	public void Start()
	{
		HideMouse();
	}

	private void HideMouse()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

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
