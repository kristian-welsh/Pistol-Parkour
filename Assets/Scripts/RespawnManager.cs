using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	public int respawnSeconds;
	public GameObject playerPrefab;
	public GameObject aiPrefab;
	public Waypoint[] spawnPoints;
	
	private AIController aiController;

	void Start ()
	{
		SpawnPlayer(RandomValidSpawn());
		SpawnAI(RandomValidSpawn());
	}

	private GameObject SpawnAI(Waypoint spawnLocation)
	{
		GameObject ai = Instantiate(aiPrefab, spawnLocation.transform.position, Quaternion.identity, transform);
		ai.GetComponent<Kristian.Health>().OnDeath += NotifyOfDeath;
		aiController = ai.GetComponent<AIController>();
		aiController.Initialize();
		aiController.SetDestination(spawnLocation);
		return ai;
	}

	private GameObject SpawnPlayer(Waypoint spawnLocation)
	{
		GameObject obj = Instantiate(playerPrefab, spawnLocation.transform.position, Quaternion.identity, transform);
		obj.GetComponent<Kristian.Health>().OnDeath += NotifyOfDeath;
		if(aiController != null)
			aiController.AggroToTarget(obj);
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

	public void NotifyOfDeath(GameObject obj)
	{
		bool ai = isAi(obj);
		StartCoroutine(RespawnTimer(ai));
	}

	private bool isAi(GameObject deadObject)
	{
		AIController controller = (AIController)deadObject.GetComponent(typeof(AIController));
		return controller != null;
	}

    private IEnumerator RespawnTimer(bool ai)
    {
        yield return new WaitForSeconds(respawnSeconds);
        Respawn(ai);
    }

    private void Respawn(bool ai)
    {
    	if(ai)
    	{
			SpawnAI(RandomValidSpawn());
    	}
    	else
    	{
			SpawnPlayer(RandomValidSpawn());
    	}
	}
}
