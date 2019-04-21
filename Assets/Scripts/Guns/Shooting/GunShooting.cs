using System;
using UnityEngine;

/* Attached in the scene to a point at the tip of a gun.
 * Allows guns to shoot
 */
public class GunShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
	[Range(0,1)]
    public float effectsDisplayPercentage = 0.2f;
    
    protected AudioSource gunAudio;

    private float timer;
    private  ShootingInput shootingInput;

    protected void Start ()
    {
        gunAudio = GetComponent<AudioSource>();
        shootingInput = GetComponentInParent(typeof(ShootingInput)) as ShootingInput;
    }
	
	void Update ()
    {
        timer += Time.deltaTime;

        if (shootingInput.ShouldShoot() && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            timer = 0f;
            Shoot();
            DamageObjects();
        }

        if (timer >= timeBetweenBullets * effectsDisplayPercentage)
        {
            DisableEffects();
        }
    }

    private void DamageObjects()
    {
        GameObject[] toDamage = GetObjectsDamaged();
        foreach(GameObject obj in toDamage)
        {
            Kristian.Health health = obj.GetComponentInParent<Kristian.Health>();
            if(health != null)
                health.Damage(damagePerShot);
        }
    }

    protected virtual void DisableEffects() { }

    protected virtual void Shoot() { }

    protected virtual GameObject[] GetObjectsDamaged()
    {
        throw new Exception("Illegal base method call GetObjectsDamaged()");
    }
}
