using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] CTSpawnPoints;  // Array for Counter-Terrorist spawn points
    public Transform[] TSpawnPoints;   // Array for Terrorist spawn points

    public void RespawnPlayer(GameObject player, string team)
    {
        // Determine which spawn points to use based on the team
        Transform[] spawnPoints = team == "CT" ? CTSpawnPoints : TSpawnPoints;

        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Move player to the chosen spawn point and reset any necessary properties
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        // Optional: Reset health, ammo, etc.
        // player.GetComponent<PlayerStats>().Reset();
    }

    public void OnPlayerDeath(GameObject player, string team)
    {
        // Delay respawn by 3 seconds
        StartCoroutine(RespawnAfterDelay(player, team, 3f));
    }

    private IEnumerator RespawnAfterDelay(GameObject player, string team, float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnPlayer(player, team);
    }

}
