using UnityEngine;
using System.Collections;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] BasePlayer[] players;
    [SerializeField] BaseEnemy[] enemies;

    public float roundTime = 30f;

    private int roundNumber = 0;
    private float timer;

    private void Start()
    {
        players = FindObjectsOfType<BasePlayer>();
        enemies = FindObjectsOfType<BaseEnemy>();

        listCharacters();
        StartCoroutine(RoundCoroutine());
    }

    public IEnumerator RoundCoroutine()
    {
        while (true)
        {
            roundNumber++;
            timer = roundTime;

            roundText.text = "Round: " + roundNumber;

            ResetTagged();
            AssignTaggers();
            info();

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                timerText.text = "Time: " + Mathf.Ceil(timer);

                if (AllPlayersTagged() || AllEnemiesTagged())
                {
                    break;
                }

                yield return null;
            }

            timerText.text = "Time: 0";

            yield return new WaitForSeconds(2f);
        }
    }

    public void AssignTaggers()
    {
        bool playersAreTaggers = roundNumber % 2 != 0;

        foreach (BasePlayer player in players)
        {
            player.SetTagger(playersAreTaggers);
        }

        foreach (BaseEnemy enemy in enemies)
        {
            enemy.SetTagger(!playersAreTaggers);
        }
    }

    private void ResetTagged()
    {
        foreach (BasePlayer player in players)
        {
            player.tagged = false;
        }

        foreach (BaseEnemy enemy in enemies)
        {
            enemy.tagged = false;
        }
    }

    private bool AllPlayersTagged()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].tagged)
                return false;
        }
        return true;
    }

    private bool AllEnemiesTagged()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].tagged)
                return false;
        }
        return true;
    }

    private void info()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isTagger)
            {
                Debug.Log("Player " + players[i] + " is currently the tagger");
            }
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].isTagger)
            {
                Debug.Log("Enemy " + enemies[i] + " is currently the tagger");
            }
        }
    }

    private void listCharacters()
    {
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("Player has " + players[i] + " arrived");
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log("Enemy has " + enemies[i] + " arrived");
        }
    }
}