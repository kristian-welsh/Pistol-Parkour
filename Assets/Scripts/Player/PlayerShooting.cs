using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    
    float timer;
    float effectsDisplayTime = 0.2f;

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

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    protected virtual void DisableEffects() { }

    protected virtual void Shoot() { }
}
