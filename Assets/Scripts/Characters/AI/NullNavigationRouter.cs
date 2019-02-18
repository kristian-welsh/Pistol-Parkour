using UnityEngine;

public class NullNavigationRouter : NavigationRouter 
{
	public override Waypoint NextDestination(Waypoint current)
	{
		return RandomLink(current);
	}

	private Waypoint RandomLink(Waypoint start)
	{
		return Kristian.Util.RandomElement<Waypoint>(start.links);
	}
}
