using UnityEngine;

/* A gun notionally that fires one bullet at a time, casts rays to simplify bullet simulation
 */
public class RaycastGun : GunShooting
{
    // Maximum range of each bullet/ray
    public float range = 100f;
    // Set to true in the AI prefab to enable missing
    public bool isAi = false;
    // % chance of the AI hitting any particular shot
    [Range(0, 100)]
    public int aiAccuracy = 10;
	
    // graphical line to display bullet path for player accuracy feedback (like a tracer round)
    LineRenderer gunLine;
    // a light to flash to show the small explosion as the gun fires a bullet
    Light gunLight;
    // an object to model bullets traveling through the air
	Raycaster raycaster;

    // helps the ai remember to apply damage in a seperate function if they hit previously
    private bool lastShotHit;

    /* Initialize the object
     */
    new void Start()
    {
        base.Start();
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
		raycaster = new Raycaster(range);

	}

    /* Shoot a bullet
     */
    protected override void Shoot()
    {
        // Calculate whether the bullet hits anyting and at what point it will stop
        RaycastHit? hit = raycaster.CastRay(transform.position, transform.forward);
        Vector3 farEndPoint = transform.position + transform.forward * range;
        Vector3 endPoint = hit.HasValue ? hit.Value.point : farEndPoint;

        // AI will intentionally miss some of the time to simulate "human" error
        if(isAi)
        {
            // if AI should try to miss this shot
            if(Kristian.Util.RandBool(100 - aiAccuracy))
            {
                // remember to not apply damage
                lastShotHit = false;
                // move the end of the graphical line to show varied distance from the player
                endPoint.x = endPoint.x + Kristian.Util.RandInt(-5, 5);
                endPoint.y = endPoint.y + Kristian.Util.RandInt(-5, 5);
                endPoint.z = endPoint.z + Kristian.Util.RandInt(-5, 5);
            }
            else
            {   
                // if we should try to hit, see whether the AI aimed right without blocking objects
                lastShotHit = hit.HasValue;
            }
        }
        else
        {   
            // if we're a player, see whether we aimed right without blocking objects
            // likely un-needed
            lastShotHit = hit.HasValue;
        }

        // Display graphical effects of shooting
        gunAudio.Play();
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        gunLine.SetPosition(1, endPoint);
    }

    /* Stop showing the bullet path and shot light
     */
    protected override void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    /* get the object to be damaged
     */
    protected override GameObject[] GetObjectsDamaged()
    {
        // cast the ray again
        RaycastHit? hit = raycaster.CastRay(transform.position, transform.forward);
        // force ai to miss when it did graphically
        if(isAi && lastShotHit && hit.HasValue)
            return new GameObject[1]{hit.Value.transform.gameObject};
        // players hit when they shoot accurately
        if(!isAi && hit.HasValue)
            return new GameObject[1]{hit.Value.transform.gameObject};
        // if we didn't hit, return no objects
        return new GameObject[0];
    }
}
