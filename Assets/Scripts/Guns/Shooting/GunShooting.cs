using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
	[Range(0,1)]
    public float effectsDisplayPercentage = 0.2f;
    
    float timer;

    protected AudioSource gunAudio;

    protected void Start ()
    {
        gunAudio = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            timer = 0f;
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayPercentage)
        {
            DisableEffects();
        }
    }

    protected virtual void DisableEffects() { }

    protected virtual void Shoot() { }
}
