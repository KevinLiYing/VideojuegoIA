using UnityEngine;
using System.Collections;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text timerText;

    public float roundTime = 30f;

    private int roundNumber = 0;
    private float timer;

    private void Start()
    {
        StartCoroutine(RoundCoroutine());
    }

    IEnumerator RoundCoroutine()
    {
        while (true)
        {
            roundNumber++;
            timer = roundTime;

            roundText.text = "Round: " + roundNumber;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                timerText.text = "Time: " + Mathf.Ceil(timer);

                yield return null;
            }

            timerText.text = "Time: 0";

            Debug.Log("Fin ronda: " + roundNumber);

            yield return new WaitForSeconds(2f);
        }
    }
}