using System;
using System.Collections;
using UnityEngine;

/* A collectable gun that is lit, hovers up and down in place, and rotates.
 * Respawns shortly after collection.
 */
public class HoveringGun : MonoBehaviour {
    public float rotationSpeed = 0.5f;
    // hover speed
    public float bounceSpeed = 0.5f;
    // hover magnitude
    public float bounceMagnitude = 0.5f;
    public float respawnTime = 10;

    // Actual fireable gun object that will be transfered to the player
    GameObject gun;
    // Light that makes the gun pickup location more easily spotted
    Light beaconLight;
    // Current state of the rotation
    ModularFloat rotationTime;
    // Current state of the hovering
    ModularFloat bounceTime;

    /* Is the gun currently collectable?
     */
    public bool GetActive()
    {

        return gun.activeSelf;
    }

    /* Makes the light shines cyan, and the gun invisible
     */
    public void Disapear()
    {
        beaconLight.color = Color.cyan;
        gun.SetActive(false);
        StartCoroutine(Appear());
    }

    /* Makes the light shine magenta, and the gun visible
     */
    private IEnumerator Appear()
    {
        yield return new WaitForSeconds(respawnTime);
        beaconLight.color = Color.magenta;
        gun.SetActive(true);
    }

    /* Set up initial state
     */
    void Start ()
    {
        gun = transform.GetChild(0).gameObject;
        beaconLight = transform.GetChild(1).GetComponent<Light>();
        rotationTime = new ModularFloat(1 / rotationSpeed);
        bounceTime = new ModularFloat(1 / bounceSpeed);
    }

    /* Updates the gun's display for the new frame
     */
    void Update()
    {
        Rotate();
        Bounce();
    }

    /* Updates the gun's rotation for the new frame
     */
    private void Rotate()
    {
        rotationTime.Add(Time.deltaTime);
        transform.eulerAngles = new Vector3(0, rotationTime.Percent * 360, 0);
    }

    /* Updates the hovering gun's y position for the new frame
     */
    private void Bounce()
    {
        bounceTime.Add(Time.deltaTime);
        float displacement = Mathf.Sin(ToRadians(bounceTime.Percent)) * bounceMagnitude;
        gun.transform.position = gun.transform.position + new Vector3(0, displacement * Time.deltaTime, 0);
    }

    /* Converts an angle in degrees to radians for use with the math libary
     */
    private float ToRadians(float percent)
    {
        return percent * 2 * Mathf.PI;
    }
}
