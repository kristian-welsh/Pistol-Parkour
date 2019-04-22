using UnityEngine;  

/* Depreciated: use Events instead
 * A data transfer object to return from ParkourModel to give to CharacterMovement
 */
public class ParkourResult
{
    // normal of the object character is climbing on
    public Vector3 normal;
    // velocity at which they're climbing
    public Vector3 velocity;

    public ParkourResult(Vector3 normal, Vector3 velocity)
    {
        this.normal = normal;
        this.velocity = velocity;
    }
}