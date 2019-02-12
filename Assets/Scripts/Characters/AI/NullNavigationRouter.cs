using UnityEngine;

public class NullNavigationRouter : NavigationRouter 
{
	public override Waypoint NextDestination(Waypoint current)
	{
		return RandomLink(current);
	}

	private Waypoint RandomLink(Waypoint start)
	{
		return randomElement<Waypoint>(start.links);
	}

	private T randomElement<T>(T[] array)
	{
		return array[randInt(0, array.Length)];
	}

	private int randInt(int start, int end)
	{
		System.Random r = new System.Random();
		return r.Next(start, end);
	}
}
