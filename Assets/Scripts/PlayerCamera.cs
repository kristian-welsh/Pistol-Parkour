using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity = 5f;
    public bool inverted = false;

    Rigidbody playerRigid;
    int inversion;

    void Start()
    {
        playerRigid = transform.parent.GetComponent<Rigidbody>();
        inversion = (inverted ? 1 : -1);
        HideMouse();
    }

    // todo: dont allow head to rotate fully up and backwards to break neck
    void FixedUpdate()
    {
        Vector2 md = GetRotationInput();
        RotateCamera(md.y);
        RotatePlayer(md.x);
        EscapeCamera();
    }

    private Vector2 GetRotationInput()
    {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity, sensitivity));
        md.y *= inversion;
        return md;
    }

    private void RotatePlayer(float amount)
    {
        Quaternion yrot = PlayerRotationNeeded(amount);
        playerRigid.MoveRotation(playerRigid.rotation * yrot);
    }

    private Quaternion PlayerRotationNeeded(float amount)
    {
        return Quaternion.AngleAxis(amount, playerRigid.transform.up);
    }

    private void RotateCamera(float amount)
    {
        Quaternion xrot = CameraRotationNeeded(amount);
        transform.rotation = transform.rotation * xrot;
        
        Quaternion localRot = transform.localRotation;
        localRot.x = Mathf.Clamp(localRot.x, -0.7f, 0.7f);
        transform.localRotation = localRot;
        print(transform.localRotation);
    }

    private Quaternion CameraRotationNeeded(float amount)
    {
        return Quaternion.AngleAxis(amount, Vector3.right);
    }

    private void EscapeCamera()
    {
        if (Input.GetKeyDown("escape"))
            ShowMouse();
    }

    private void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}