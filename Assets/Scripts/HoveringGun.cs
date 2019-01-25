using System;
using System.Collections;
using UnityEngine;

public class HoveringGun : MonoBehaviour {
    public float rotationSpeed = 0.5f;
    public float bounceSpeed = 0.5f;
    public float bounceMagnitude = 0.5f;
    public float respawnTime = 10;

    GameObject gun;
    Light beaconLight;
    ModularFloat rotationTime;
    ModularFloat bounceTime;

    void Start ()
    {
        gun = transform.GetChild(0).gameObject;
        beaconLight = GetComponentInChildren<Light>();
        rotationTime = new ModularFloat(1 / rotationSpeed);
        bounceTime = new ModularFloat(1 / bounceSpeed);
    }

    void Update()
    {
        Rotate();
        Bounce();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Disapear();
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

    private void Disapear()
    {
        beaconLight.color = Color.cyan;
        gun.SetActive(false);
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        yield return new WaitForSeconds(respawnTime);
        beaconLight.color = Color.magenta;
        gun.SetActive(true);
    }
}
