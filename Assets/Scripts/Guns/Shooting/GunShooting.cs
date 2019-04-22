using System;
using UnityEngine;

/* Attached in the scene to a point at the tip of a gun to allow them to shoot
 * Delegates specific implementation of displaying shots, getting damaged objects to subclasses
 */
public class GunShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    // For how much of the time between shots should the graphical display last
	[Range(0,1)]
    public float effectsDisplayPercentage = 0.2f;
    
    // Audio to play for shots
    protected AudioSource gunAudio;

    // seconds passed since the last time a shot was fired
    private float timer;
    // AI/Player input deciding whether to shoot on any particular frame
    private ShootingInput shootingInput;

    /* Initialize the object
     */
    protected void Start ()
    {
        gunAudio = GetComponent<AudioSource>();
        shootingInput = GetComponentInParent(typeof(ShootingInput)) as ShootingInput;
    }

	/* Shoot if appropriate & disable shooting graphics if appropriate
     */
	void Update ()
    {
        // add time passed since last Update to timer count
        timer += Time.deltaTime;

        // If the agent wants to, enough time has passed, the game isn't paused, shoot
        if (shootingInput.ShouldShoot() && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            timer = 0f;
            Shoot();
            DamageObjects();
        }

        // If enough time has passed, stop displaying shot effects
        if (timer >= timeBetweenBullets * effectsDisplayPercentage)
        {
            DisableEffects();
        }
    }

    /* Apply damage to any valid object returned by subclass as being damaged by the shot
     */
    private void DamageObjects()
    {
        GameObject[] toDamage = GetObjectsDamaged();
        foreach(GameObject obj in toDamage)
        {
            // if it has a Health object, it can be damaged
            // potential for destructable objects in future
            Kristian.Health health = obj.GetComponentInParent<Kristian.Health>();
            if(health != null)
                health.Damage(damagePerShot);
        }
    }

    // Define an API so subclasses can adhere to it, to allow differences in gun behaviour

    protected virtual void DisableEffects() { }

    protected virtual void Shoot() { }

    protected virtual GameObject[] GetObjectsDamaged()
    {
        throw new Exception("Illegal base method call GetObjectsDamaged()");
    }
}
