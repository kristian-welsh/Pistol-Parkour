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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // todo: dont allow head to rotate fully up and backwards to break neck
    void FixedUpdate()
    {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity, sensitivity));
        md.y *= inversion;

        Quaternion yrot = Quaternion.AngleAxis(md.y, Vector3.right);
        Quaternion xrot = Quaternion.AngleAxis(md.x, playerRigid.transform.up);
        
        transform.rotation = transform.rotation * yrot;
        playerRigid.MoveRotation(playerRigid.rotation * xrot);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
