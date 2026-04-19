using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damage = 10f;
    public bool tagged = false;
    public bool isTagger = false;

    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void DealDamage(Character target)
    {
        if (target == null) return;
        target.TakeDamage(damage, this);
    }

    public virtual void TakeDamage(float amount, Character attacker)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Character other = collision.gameObject.GetComponent<Character>();

        if (other != null)
        {
            OnCharacterContact(other);
        }
    }

    protected virtual void OnCharacterContact(Character other)
    {
    }

    public Character GetCharacterFromCollision(Collision collision)
    {
        return collision.gameObject.GetComponent<Character>();
    }
}