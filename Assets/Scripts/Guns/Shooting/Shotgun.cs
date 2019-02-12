using UnityEngine;

public class Shotgun : GunShooting
{
    GameObject cone;
    Light gunLight;

    new void Start()
    {
        base.Start();
        cone = transform.GetChild(0).gameObject;
        gunLight = GetComponent<Light>();
    }

    protected override void DisableEffects()
    {
        cone.SetActive(false);
        gunLight.enabled = false;
    }

    protected override void Shoot()
    {
        cone.SetActive(true);
        gunAudio.Play();
        gunLight.enabled = true;
    }
}
