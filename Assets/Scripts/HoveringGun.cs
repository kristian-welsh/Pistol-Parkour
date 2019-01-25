using UnityEngine;

public class HoveringGun : MonoBehaviour {
    public float rotationSpeed = 2f;
    public float bounceSpeed = 2f;
    public float bounceMagnitude = 0.5f;

    float rotationTime;
    float bounceTime;
    GameObject gun;
    Vector3 startingPosition;

    void Start () {
        gun = transform.GetChild(0).gameObject;
        startingPosition = gun.transform.position;
    }
    
    void FixedUpdate()
    {
        // todo: detect collision with player
    }

    void Update()
    {
        rotationTime += Time.deltaTime;
        rotationTime %= rotationSpeed;
        float rotationAmount = (rotationTime / rotationSpeed) * 360;
        transform.eulerAngles = new Vector3(0, rotationAmount, 0);

        // deltabounce is nessecary because the gun's origin isn't centere on the object
        // meaning setting its position relative to start position nullifies change in
        // position from parent's rotation
        bounceTime += Time.deltaTime;
        bounceTime %= bounceSpeed;
        float bounceAmount = Mathf.Sin((bounceTime / bounceSpeed) * 2 * Mathf.PI) * bounceMagnitude;
        //float deltaBounce = 
        gun.transform.position = gun.transform.position + new Vector3(0, bounceAmount * Time.deltaTime, 0);
    }
}
