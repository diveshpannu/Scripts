using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static int playerHP = 100;
    public TextMeshProUGUI playerHPText;
    public GameObject bloodOverlay;
    public Transform[] teamSpawnPoints;  // Array for spawn points

    public static bool isGameOver;

    private void Start()
    {
        if (photonView.IsMine)
        {
            isGameOver = false;
            playerHP = 100;
            UpdateHealthUI();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            playerHPText.text = "+" + playerHP;

            if (isGameOver)
            {
                StartCoroutine(RespawnPlayer());
            }
        }
    }

    public IEnumerator TakeDamage(int damageAmount)
    {
        if (!photonView.IsMine) yield break;  // Only apply damage to the local player

        bloodOverlay.SetActive(true);
        playerHP -= damageAmount;
        UpdateHealthUI();

        if (playerHP <= 0)
        {
            isGameOver = true;
            photonView.RPC("HandlePlayerDeath", RpcTarget.AllBuffered);
        }

        yield return new WaitForSeconds(1);
        bloodOverlay.SetActive(false);
    }


    private void UpdateHealthUI()
    {
        playerHPText.text = "+" + playerHP;
    }

    [PunRPC]
    private void HandlePlayerDeath()
    {
        // Hide player or trigger death animations
        gameObject.SetActive(false);
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(3); // Wait 3 seconds before respawning

        playerHP = 100;
        isGameOver = false;

        // Choose a random spawn point from the array
        Transform spawnPoint = teamSpawnPoints[Random.Range(0, teamSpawnPoints.Length)];

        // Move the player to the spawn point
        gameObject.transform.position = spawnPoint.position;
        gameObject.transform.rotation = spawnPoint.rotation;

        gameObject.SetActive(true); // Reactivate the player
        UpdateHealthUI();
    }
}
