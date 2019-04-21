using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A complicated class that handles the creation and re-creation of players and AI combatants
 */
public class RespawnManager : MonoBehaviour
{
	// number of seconds to wait after death until a combatant is respawned
	public int respawnSeconds;
	// gameobject to spawn for players
	public GameObject playerPrefab;
	// gameobject to spawn for AI combatants
	public GameObject aiPrefab;
	// AI pathfinding nodes that we may spawn combatants on
	public Waypoint[] spawnPoints;
	// Stores the last spawned AI so we can tell it to attack the player whenever they spawn
	private AIController aiController;

	/* Spawns one player and one AI combatant
	 */
	void Start ()
	{
		SpawnPlayer(RandomValidSpawn());
		SpawnAI(RandomValidSpawn());
	}

	/* Spawns and initializes the AI
	 */
	private GameObject SpawnAI(Waypoint spawnLocation)
	{
		GameObject ai = Instantiate(aiPrefab, spawnLocation.transform.position, Quaternion.identity, transform);
		ai.GetComponent<Kristian.Health>().OnDeath += NotifyOfDeath;
		aiController = ai.GetComponent<AIController>();
		aiController.Initialize();
		aiController.SetDestination(spawnLocation);
		return ai;
	}

	/* Spawns a player and tells the AI to attack it
	 */
	private GameObject SpawnPlayer(Waypoint spawnLocation)
	{
		GameObject obj = Instantiate(playerPrefab, spawnLocation.transform.position, Quaternion.identity, transform);
		obj.GetComponent<Kristian.Health>().OnDeath += NotifyOfDeath;
		if(aiController != null)
			aiController.AggroToTarget(obj);
		return obj;
	}

	/* Chooses a random unocupied spawn point, this avoids one combatant spawning inside the other
	 */
	private Waypoint RandomValidSpawn()
	{
		List<Waypoint> validSpawns = new List<Waypoint>();
		foreach(Waypoint spawn in spawnPoints)
			if(spawn.IsValidSpawn())
				validSpawns.Add(spawn);
		return Kristian.Util.RandomElement<Waypoint>(validSpawns);
	}

	/* Called when a combatant reaches 0 health
	 */
	public void NotifyOfDeath(GameObject obj)
	{
		bool ai = isAi(obj);
		StartCoroutine(RespawnTimer(ai));
	}

	/* Determines whether a combatant is AI or not so we can respawn the right combatant
	 */
	private bool isAi(GameObject deadObject)
	{
		// if a combatant has an AIController, they are an AI
		AIController controller = (AIController)deadObject.GetComponent(typeof(AIController));
		return controller != null;
	}

	/* Waits for a certain period of time then respawns the required combatant
	 */
    private IEnumerator RespawnTimer(bool ai)
    {
        yield return new WaitForSeconds(respawnSeconds);
        Respawn(ai);
    }

	/* Respanws the required combatant
	 */
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
