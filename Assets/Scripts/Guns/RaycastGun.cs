using UnityEngine;

public class RaycastGun : GunShooting
{
    public float range = 100f;

    int shootableMask;
    LineRenderer gunLine;
    Light gunLight;

    new void Start()
    {
        base.Start();
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
    }

    protected override void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    protected override void Shoot()
    {
        gunAudio.Play();
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        Ray shootRay = new Ray(transform.position, transform.forward);
        RaycastHit shootHit;
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
