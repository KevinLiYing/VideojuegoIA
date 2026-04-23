using UnityEngine;

public class BulletDetector : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask projectileLayer;

    private EnemyAIStateMotor m;

    private void Awake()
    {
        m = GetComponent<EnemyAIStateMotor>();
    }

    private void FixedUpdate()
    {
        if (IsProjectileThreat())
        {
            Debug.Log("Projectile detected");

            m.ChangeState(m.GetComponent<AIEvadeState>());
        }
        else
        {
            m.ChangeState(m.GetComponent<AIFollowPathState>());
        }
    }

    private bool IsProjectileThreat()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, projectileLayer);

        Debug.Log("Hits: " + hits.Length);

        return hits.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}