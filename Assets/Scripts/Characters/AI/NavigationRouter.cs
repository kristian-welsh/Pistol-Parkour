using UnityEngine;

/* Leads calling object to the gameobject this is attached to, one step at a time
 */
public class NavigationRouter : MonoBehaviour
{
	/* Next waypoint to move to.
	 * virtual to support multiple implementations
	 */
	public virtual Waypoint NextDestination(Waypoint current)
	{
		return NearestLink(current);
	}

	/* find the child link of start that is nearest to the gameobject this script is attached to.
	 * Is only sufficient for simple maps as it results in the AI getting stuck in local minimums.
	 */
	private Waypoint NearestLink(Waypoint start)
	{
		// simple find minimum distance implementation
		Waypoint nearest = null;
		float minDistance = float.PositiveInfinity;
		foreach (Waypoint point in start.links)
		{
			float distance = Vector3.Distance(point.transform.position, gameObject.transform.position);
			if(distance < minDistance)
			{
				minDistance = distance;
				nearest = point;
			}
		}
		return nearest;
	}
}
