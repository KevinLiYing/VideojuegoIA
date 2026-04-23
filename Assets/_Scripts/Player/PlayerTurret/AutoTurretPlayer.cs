using UnityEngine;

public class AutoTurretPlayer : MonoBehaviour
{
    [Header("Targeting")]
    public float detectionRadius = 20f;
    public LayerMask enemyLayer;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    private float _fireTimer;

    private void Update()
    {
        _fireTimer += Time.deltaTime;

        Character target = GetClosestTarget();

        if (target != null && _fireTimer >= 1f / fireRate)
        {
            Shoot(target);
            _fireTimer = 0f;
        }
    }

    private Character GetClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        float minDist = Mathf.Infinity;
        Character closest = null;

        foreach (var hit in hits)
        {
            Character c = hit.GetComponent<Character>();
            if (c == null) continue;

            float dist = (c.transform.position - transform.position).sqrMagnitude;

            if (dist < minDist)
            {
                minDist = dist;
                closest = c;
            }
        }

        return closest;
    }

    private void Shoot(Character target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb == null) return;
        float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;

        Vector3 predictedPosition = PredictTargetPosition(target, targetRb, bulletSpeed);

        Vector3 direction = (predictedPosition - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().Initialize(direction);
    }

    private Vector3 PredictTargetPosition(Character target, Rigidbody targetRb, float bulletSpeed)
    {
        Vector3 targetPos = target.transform.position;
        Vector3 targetVel = targetRb.linearVelocity;

        Vector3 toTarget = targetPos - transform.position;

        float distance = toTarget.magnitude;

        if (targetVel.sqrMagnitude < 0.01f)
            return targetPos;

        float time = distance / bulletSpeed;

        return targetPos + targetVel * time;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}