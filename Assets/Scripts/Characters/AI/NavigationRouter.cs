using UnityEngine;

// todo: make this and its null object models to avoid instantiating MonoBehaviours in AIMovement
public class NavigationRouter : MonoBehaviour
{
	//leads calling object to the gameobject this is attached to, one step at a time
	public virtual Waypoint NextDestination(Waypoint current)
	{
		return NearestLink(current);
	}

	// find the child link of start that is nearest to the gameobject this script is attached to
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
