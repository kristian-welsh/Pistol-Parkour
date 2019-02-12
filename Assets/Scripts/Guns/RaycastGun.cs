﻿using UnityEngine;

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
		raycaster = new Raycaster(range, "Shootable");

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
		
		RaycastHit? hit = raycaster.CastRay(transform.position, transform.forward);
		if (hit.HasValue)
        {
            gunLine.SetPosition(1, hit.Value.point);
        }
        else
        {
            gunLine.SetPosition(1, transform.position + transform.forward * range);
        }
    }
}
