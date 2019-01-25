using UnityEngine;

public class HoveringGun : MonoBehaviour {
    public float rotationSpeed = 0.5f;
    public float bounceSpeed = 0.5f;
    public float bounceMagnitude = 0.5f;

    ModularFloat rotationTime;
    ModularFloat bounceTime;
    GameObject gun;

    void Start () {
        gun = transform.GetChild(0).gameObject;
        rotationTime = new ModularFloat(1/rotationSpeed);
        bounceTime = new ModularFloat(1/bounceSpeed);
    }
    
    void FixedUpdate()
    {
        // todo: detect collision with player
    }

    void Update()
    {
        Rotate();
        Bounce();
    }

    private void Rotate()
    {
        rotationTime.Add(Time.deltaTime);
        transform.eulerAngles = new Vector3(0, rotationTime.Percent * 360, 0);
    }

    private void Bounce()
    {
        bounceTime.Add(Time.deltaTime);
        float displacement = Mathf.Sin(ToRadians(bounceTime.Percent)) * bounceMagnitude;
        gun.transform.position = gun.transform.position + new Vector3(0, displacement * Time.deltaTime, 0);
    }

    private float ToRadians(float percent)
    {
        return percent * 2 * Mathf.PI;
    }
}
