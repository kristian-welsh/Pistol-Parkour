using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	public int respawnSeconds;
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
		GameObject obj = Instantiate(prefab, spawnLocation.transform.position, Quaternion.identity, transform);
		obj.GetComponent<Kristian.Health>().respawner = this;
		return obj;
	}

	private Waypoint RandomValidSpawn()
	{
		List<Waypoint> validSpawns = new List<Waypoint>();
		foreach(Waypoint spawn in spawnPoints)
			if(spawn.IsValidSpawn())
				validSpawns.Add(spawn);
		return Kristian.Util.RandomElement<Waypoint>(validSpawns);
	}

	public void NotifyOfDeath(GameObject deadObject)
	{
		Destroy(deadObject);
		bool ai = isAi(deadObject);
		StartCoroutine(RespawnTimer(ai));
	}

	private bool isAi(GameObject deadObject)
	{
		CharacterMovement movement = deadObject.GetComponent<CharacterMovement>();
		return movement is AIMovement;
	}

    private IEnumerator RespawnTimer(bool ai)
    {
        yield return new WaitForSeconds(respawnSeconds);
        Respawn(ai);
    }

    private void Respawn(bool ai)
    {
    	GameObject prefab = ai ? aiPrefab : playerPrefab;
		Spawn(prefab, RandomValidSpawn());
	}
}
