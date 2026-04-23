using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public float damage = 10f;

    private Vector3 _direction;

    public Vector3 Velocity => _direction * speed;

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += Velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character != null)
        {
            character.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}