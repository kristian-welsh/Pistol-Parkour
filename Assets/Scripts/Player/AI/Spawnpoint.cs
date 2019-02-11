using UnityEngine;

public class Spawnpoint : Waypoint
{
	public GameObject AIprefab;

	void Start ()
	{
		SpawnAICharacter();
	}

	public void SpawnAICharacter()
	{
		GameObject character = GameObject.Instantiate(AIprefab);
		character.GetComponent<AIMovement>().destination = this;
		character.transform.position = gameObject.transform.position;
	}
}
