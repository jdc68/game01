using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject player;
    public GameObject smoke;
    //[HideInInspector]
    public GameObject currentPlayer;

    public void respawnPlayer()
    {
        if (currentPlayer)
        {
            Destroy(currentPlayer);
        }
        //Instantiate(smoke, transform.position, transform.rotation);
        ScoreManager.Instance.currentHealth = ScoreManager.Instance.maxHealth;
        currentPlayer = Instantiate(player, transform.position, transform.rotation);
        ScoreManager.Instance.isDead = false;
        FindObjectOfType<ShowDeathScreen>().hide();
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
