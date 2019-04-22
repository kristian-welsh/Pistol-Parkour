using UnityEngine;

/* A point in space that the AI may navigate to & characters may spawn at
 */
public class Waypoint : MonoBehaviour
{
	/* Other waypoints navigable via this waypoint.
	 * Build a mesh using this variable in the unity editor for the AI to traverse
	 * supports one-way links, particularly useful for parkour that only works in one direction
	 */
	public Waypoint[] links;
	// Should AI jump when it reaches this waypoint? (used to provoke jumping chasms, and parkour)
	public bool jump = false;

	public Vector3 Position { get { return transform.position; } }

	/* Is the space above this waypoint unoccupied?
	 */
	public bool IsValidSpawn()
	{
		return !Physics.CheckSphere(transform.position + new Vector3(0f, 2f, 0f), 1.5f);
	}
}
