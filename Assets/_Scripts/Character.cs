using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damage = 10f;

    public bool isTagger = false;
    public bool isTagged = false;

    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetTagger(bool value)
    {
        isTagger = value;
    }

    public void SetTagged(bool value)
    {
        isTagged = value;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Character c = collision.gameObject.GetComponent<Character>();
        //Debug.Log("Hit: " + collision.gameObject.name);
        if (c != null)
        {
            OnCharacterContact(c);
        }
    }

    protected virtual void OnCharacterContact(Character other)
    {
        if (isTagger && !other.isTagged)
        {
            other.SetTagged(true);
            other.SetTagger(true);

            SetTagger(false);
        }
    }
}