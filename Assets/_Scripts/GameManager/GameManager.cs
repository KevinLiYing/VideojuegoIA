using UnityEngine;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
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

            Debug.Log("Empieza ronda: " + roundNumber);

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null; 
            }

            Debug.Log("Fin de ronda: " + roundNumber);

            yield return new WaitForSeconds(2f);
        }
    }
}