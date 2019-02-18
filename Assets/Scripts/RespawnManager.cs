using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	public GameObject playerPrefab;
	public GameObject aiPrefab;
	public Waypoint[] spawnPoints;

	void Start ()
	{
		Spawn(playerPrefab, spawnPoints[0]);
		SpawnAI(aiPrefab, spawnPoints[1]);
	}

	private GameObject SpawnAI(GameObject prefab, Waypoint spawnLocation)
	{
		GameObject ai = Spawn(prefab, spawnLocation);
		ai.GetComponent<AIMovement>().destination = spawnLocation;
		return ai;
	}

	private GameObject Spawn(GameObject prefab, Waypoint spawnLocation)
	{
		return Instantiate(prefab, spawnLocation.transform.position, Quaternion.identity, transform);
	}

	private Waypoint RandomSpawn()
	{
		return Kristian.Util.RandomElement<Waypoint>(spawnPoints);
	}
}
