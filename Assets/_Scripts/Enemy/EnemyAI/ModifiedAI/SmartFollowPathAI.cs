using UnityEngine;

public class SmartFollowPathAI : AIBehaviour
{
    public override void FollowPath(Rigidbody rb)
    {
        var target = _pathPoints[_currentRouteIndex].position;

        _sqrRemainingDistance = (target - transform.position).sqrMagnitude;

        if (CheckNextPoint())
        {
            target = SetNextRoutePoint();
        }

        Vector3 directionToTarget = (target - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToTarget, out RaycastHit hit, 2f))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 avoidDir = Vector3.Cross(hit.normal, Vector3.up);

                if (Vector3.Dot(avoidDir, directionToTarget) < 0)
                    avoidDir = -avoidDir;

                Vector3 steering = avoidDir * maxSpeed;

                rb.linearVelocity = CalculateFinalVelocity(steering);
                return;
            }
        }

        Seek(target, rb);
    }
}
