using UnityEngine;

/* The shooting behaviour of a shotgun, uses a transparent cone to indicate affected area
 */
public class Shotgun : GunShooting
{
    // The visual cone of effect
    GameObject coneModel;
    // The logical cone of effect
    // continually maintain a list of combatants that will be hurt if shot using CollisionLister
    GameObject coneCollider;
    // The light that shows while shooting
    Light gunLight;

    /* instantiate the object
     */
    new void Start()
    {
        base.Start();
        coneModel = transform.GetChild(0).gameObject;
        coneCollider = transform.GetChild(1).gameObject;
        gunLight = GetComponent<Light>();
    }

    /* Remove shooting graphical effects
     */
    protected override void DisableEffects()
    {
        coneModel.SetActive(false);
        gunLight.enabled = false;
    }

    /* Add shooting graphical effects
     */
    protected override void Shoot()
    {
        coneModel.SetActive(true);
        gunAudio.Play();
        gunLight.enabled = true;
    }

    /* Get a list of combatants to hurt
     */
    protected override GameObject[] GetObjectsDamaged()
    {
        // grab the collision lister and return the current list
        CollisionLister coneLister = coneCollider.GetComponent<CollisionLister>();
        return coneLister.list.ToArray();
    }
}
