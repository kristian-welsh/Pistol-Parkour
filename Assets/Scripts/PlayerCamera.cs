using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity = 5f;
    public bool inverted = false;
    Transform playerTransform;
    int inv;

    // Use this for initialization
    void Start () {
        playerTransform = transform.parent;
        inv = (inverted ? 1 : -1);
        Cursor.visible = false;
    }
	
	void FixedUpdate () {
        // todo: dont allow head to rotate fully up and backwards to break neck
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity, sensitivity));
        md.y *= inv;

        Quaternion yrot = Quaternion.AngleAxis(-md.y, Vector3.right);
        Quaternion xrot = Quaternion.AngleAxis(md.x, playerTransform.up);
        transform.Rotate(yrot.eulerAngles);
        playerTransform.Rotate(xrot.eulerAngles);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
    }
}
