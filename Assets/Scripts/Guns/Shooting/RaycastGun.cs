using UnityEngine;

public class RaycastGun : GunShooting
{
    public float range = 100f;
	
    LineRenderer gunLine;
    Light gunLight;
	Raycaster raycaster;

    new void Start()
    {
        base.Start();
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
		raycaster = new Raycaster(range);

	}

    protected override void Shoot()
    {
        gunAudio.Play();
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        
        RaycastHit? hit = raycaster.CastRay(transform.position, transform.forward);
        Vector3 farEndPoint = transform.position + transform.forward * range;
        Vector3 endPoint = hit.HasValue ? hit.Value.point : farEndPoint;
        gunLine.SetPosition(1, endPoint);
    }

    protected override void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    protected override GameObject[] GetObjectsDamaged()
    {
        RaycastHit? hit = raycaster.CastRay(transform.position, transform.forward);
        if(hit.HasValue)
            return new GameObject[1]{hit.Value.transform.gameObject};
        return new GameObject[0];
    }
}
