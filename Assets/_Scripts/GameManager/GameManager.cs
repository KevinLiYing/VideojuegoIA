using UnityEngine;
using System.Collections;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text winnerText;

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
            winnerText.text = "";

            ResetTagged();
            AssignTaggers();
            UpdateEnemyStates();

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

            SetWinner();

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

    public void ResetTagged()
    {
        foreach (BasePlayer player in players)
            player.SetTagged(false);

        foreach (BaseEnemy enemy in enemies)
            enemy.SetTagged(false);
    }

    private bool AllPlayersTagged()
    {
        for (int i = 0; i < players.Length; i++)
            if (!players[i].isTagged)
                return false;

        return true;
    }

    private bool AllEnemiesTagged()
    {
        for (int i = 0; i < enemies.Length; i++)
            if (!enemies[i].isTagged)
                return false;

        return true;
    }

    private void SetWinner()
    {
        if (AllPlayersTagged())
        {
            winnerText.text = "Enemies win the round";
        }
        else if (AllEnemiesTagged())
        {
            winnerText.text = "Players win the round";
        }
        else
        {
            winnerText.text = "Player wins the round";
        }
    }

    private void UpdateEnemyStates()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyAIStateMotor m = enemies[i].GetComponent<EnemyAIStateMotor>();
            MeshRenderer mr = enemies[i].GetComponent<MeshRenderer>();

            if (!enemies[i].isTagger)
            {
                mr.material.color = Color.green;
                m.ChangeState(m.GetComponent<AIFleeState>());
            }
            else
            {
                mr.material.color = Color.cyan;
                m.ChangeState(m.GetComponent<AISeekState>());
            }
        }
    }

    private void info()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isTagger)
                Debug.Log("Player " + players[i] + " is currently the tagger");

            if (players[i].isTagged)
                Debug.Log("Player " + players[i] + " is tagged");
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].isTagger)
                Debug.Log("Enemy " + enemies[i] + " is currently the tagger");

            if (enemies[i].isTagged)
                Debug.Log("Enemy " + enemies[i] + " is tagged");
        }
    }

    private void listCharacters()
    {
        for (int i = 0; i < players.Length; i++)
            Debug.Log("Player has " + players[i] + " arrived");

        for (int i = 0; i < enemies.Length; i++)
            Debug.Log("Enemy has " + enemies[i] + " arrived");
    }
}