using UnityEngine;

public class RaycastGun : GunShooting
{
    public float range = 100f;

    public bool isAi = false;
    // % chance of hitting any particular shot
    [Range(0, 100)]
    public int aiAccuracy = 1;
	
    LineRenderer gunLine;
    Light gunLight;
	Raycaster raycaster;

    //helps the ai remember to apply damage
    private bool lastShotHit;

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
        if(isAi)
        {
            // if we want to miss this shot
            if(Kristian.Util.RandBool(100 - aiAccuracy))
            {
                lastShotHit = false;
                endPoint.x = endPoint.x + Kristian.Util.RandInt(-5, 5);
                endPoint.y = endPoint.y + Kristian.Util.RandInt(-5, 5);
                endPoint.z = endPoint.z + Kristian.Util.RandInt(-5, 5);
            }
            else
            {   
                lastShotHit = hit.HasValue;
            }
        }
        else
        {   
            lastShotHit = hit.HasValue;
        }
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
        // force ai miss when i say so
        if(isAi && lastShotHit && hit.HasValue)
            return new GameObject[1]{hit.Value.transform.gameObject};
        // players hit when they shoot accurately
        if(!isAi && hit.HasValue)
            return new GameObject[1]{hit.Value.transform.gameObject};
        return new GameObject[0];
    }
}
