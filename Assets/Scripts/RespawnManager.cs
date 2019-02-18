using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	public GameObject playerPrefab;
	public GameObject aiPrefab;
	public Waypoint[] spawnPoints;

	void Start ()
	{
		Spawn(playerPrefab, RandomValidSpawn());
		SpawnAI(aiPrefab, RandomValidSpawn());
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

	private Waypoint RandomValidSpawn()
	{
		List<Waypoint> validSpawns = new List<Waypoint>();
		foreach(Waypoint spawn in spawnPoints)
			if(spawn.IsValidSpawn())
				validSpawns.Add(spawn);
		return Kristian.Util.RandomElement<Waypoint>(validSpawns);
	}
}
