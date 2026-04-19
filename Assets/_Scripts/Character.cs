using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damage = 10f;

    public bool isTagger = false;
    public bool tagged = false;

    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetTagger(bool value)
    {
        isTagger = value;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Character c = other.GetComponent<Character>();

        if (c != null)
        {
            OnCharacterContact(c);
        }
    }

    protected virtual void OnCharacterContact(Character other)
    {
        if (isTagger && !other.isTagger)
        {
            other.tagged = true;
        }
    }
}