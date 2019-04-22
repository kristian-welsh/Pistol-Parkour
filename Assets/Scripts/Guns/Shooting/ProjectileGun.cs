using UnityEngine;

/* Simulates a gun that fires slowmoving projectiles (like the plasma gun in quake)
 * It's up to the projectiles to simulate themselves
 */
public class ProjectileGun : GunShooting
{
    // how fast the projectiles should move
    public float projectileSpeed = 1f;

    // prefab object to spawn for the projectiles
    GameObject templateProjectile;

    /* Initialize
     */
    new void Start ()
    {
        base.Start();
        templateProjectile = transform.GetChild(0).gameObject;
    }
    
    /* Spawn a projectile with velocity it in the direciton the gun is facing
     */
    protected override void Shoot()
    {
        // create a new projectile based on template
        GameObject projectile = Instantiate(templateProjectile, templateProjectile.transform.root);
        projectile.SetActive(true);
        // position it at the barrel of the gun
        projectile.transform.position = transform.position;

        // give it a push
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }

    /* Conform to the subclass required override API
     * Returns nothing because nothing is damaged immidiately,
     * damage is triggered by projectile scripts
     */
    protected override GameObject[] GetObjectsDamaged()
    {
        return new GameObject[0];
    }
}
