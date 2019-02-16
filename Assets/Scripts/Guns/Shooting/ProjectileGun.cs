using UnityEngine;

public class ProjectileGun : GunShooting
{
    public float projectileSpeed = 1f;

    GameObject templateProjectile;

    new void Start ()
    {
        base.Start();
        templateProjectile = transform.GetChild(0).gameObject;
    }
    
    protected override void Shoot()
    {
        GameObject projectile = Instantiate(templateProjectile, templateProjectile.transform.root);
        projectile.SetActive(true);
        projectile.transform.position = transform.position;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }

    protected override GameObject[] GetObjectsDamaged()
    {
        // nothing is damaged immidiately, damage is triggered by projectile scripts
        return new GameObject[0];
    }
}
