using UnityEngine;

/* Null object pattern allowing null to be sent messages with apropriate responses
 */
public class NullNavigationRouter : NavigationRouter 
{
	/* Next waypoint to move to.
	 */
	public override Waypoint NextDestination(Waypoint current)
	{
		return RandomLink(current);
	}

	/* Choose a waypoint at random, doesn't move too far from
	 * start, as it's a random next node, not a random final destination.
	 */
	private Waypoint RandomLink(Waypoint start)
	{
		return Kristian.Util.RandomElement<Waypoint>(start.links);
	}
}
