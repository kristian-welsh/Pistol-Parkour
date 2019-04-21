using UnityEngine;  

public class ParkourResult
{
    public Vector3 normal;
    public Vector3 velocity;

    public ParkourResult(Vector3 normal, Vector3 velocity)
    {
        this.normal = normal;
        this.velocity = velocity;
        MonoBehaviour.print("result: normal:" + normal + ", velocity: " + velocity);
    }
}