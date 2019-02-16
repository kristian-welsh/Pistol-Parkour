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
    
    protected override void DrawEffects()
    {
        GameObject projectile = Instantiate(templateProjectile, templateProjectile.transform.root);
        projectile.SetActive(true);
        projectile.transform.position = transform.position;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }
}
