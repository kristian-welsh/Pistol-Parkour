using UnityEngine;

public class Shotgun : GunShooting
{
    GameObject coneModel;
    GameObject coneCollider;
    Light gunLight;

    new void Start()
    {
        base.Start();
        coneModel = transform.GetChild(0).gameObject;
        coneCollider = transform.GetChild(1).gameObject;
        gunLight = GetComponent<Light>();
    }

    protected override void DisableEffects()
    {
        coneModel.SetActive(false);
        gunLight.enabled = false;
    }

    protected override void Shoot()
    {
        coneModel.SetActive(true);
        gunAudio.Play();
        gunLight.enabled = true;
    }

    protected override GameObject[] GetObjectsDamaged()
    {
        CollisionLister coneLister = coneCollider.GetComponent<CollisionLister>();
        return coneLister.list.ToArray();
    }
}
