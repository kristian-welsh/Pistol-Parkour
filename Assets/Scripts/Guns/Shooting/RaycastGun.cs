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

    protected override void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    protected override void Shoot()
    {
        DrawEffects();
        DamageObjects();
	}

    private void DrawEffects()
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

    private void DamageObjects()
    {
        RaycastHit? hit = raycaster.CastRay(transform.position, transform.forward);
        if(hit.HasValue)
        {
            GameObject objectHit = hit.Value.transform.gameObject;
            print(objectHit.name);
            FPSHealth health = objectHit.GetComponentInParent<FPSHealth>();
            if(health != null)
                health.Damage(damagePerShot);
        }
    }
}
