using UnityEngine;

public class NavigationRouter : MonoBehaviour
{
	public virtual Waypoint NextDestination(Waypoint current)
	{
		return NearestLink(current);
	}

	private Waypoint NearestLink(Waypoint start)
	{
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
