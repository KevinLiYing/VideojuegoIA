using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    [Header("Follow Path Settings")]
    [SerializeField] private PathRoutesSO patrolRoutes;
    [SerializeField] private PatrolRoute patrolRoute;

    [Header("Steering")]
    public float maxSpeed;
    public float steeringMaxSpeed;
    public float stoppingDistance;

    [Header("Display Settings")]
    [SerializeField] private bool areVectorsOnDisplay;

    public Transform[] _pathPoints;
    public int _currentRouteIndex;
    public float _sqrRemainingDistance;
    public bool _pathPending;

    private void Start()
    {
        SetNewRoute(patrolRoute);
    }

    #region Behaviours

    public virtual void Seek(Vector3 target, Rigidbody rb)
    {
        var targetDirection = CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb.linearVelocity);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb.linearVelocity);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = CalculateFinalVelocity(finalDirection) * Arrive(target);
    }

    public virtual void Flee(Vector3 target, Rigidbody rb)
    {
        var targetDirection = -CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb.linearVelocity);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb.linearVelocity);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = CalculateFinalVelocity(finalDirection);
    }

    public virtual float Arrive(Vector3 target)
    {
        var sqrDistance = (target - transform.position).sqrMagnitude;
        if (sqrDistance > Mathf.Pow(stoppingDistance, 2))
            return 1;

        _pathPending = false;

        return (sqrDistance / Mathf.Pow(stoppingDistance, 2));
    }

    public virtual void Pursue(Vector3 target, Rigidbody rb, Rigidbody targetRb)
    {
        if (rb.linearVelocity.sqrMagnitude < 0.0001f || Vector3.Dot(targetRb.linearVelocity.normalized, CalculateTargetDirection(target).normalized) < -0.8f)
            Seek(target, rb);
        else
        {
            var currentSqrSpeed = rb.linearVelocity.sqrMagnitude;
            var sqrDistanceToTarget = CalculateTargetDirection(target).sqrMagnitude;
            var prediction = CalculatePrediction(sqrDistanceToTarget, currentSqrSpeed);

            var explicitTarget = CalculatePredictionExplicitTarget(target, targetRb, prediction);
            Seek(explicitTarget, rb);
        }
    }

    public virtual void Evade(Vector3 target, Rigidbody rb, Rigidbody targetRb)
    {
        if (rb.linearVelocity.sqrMagnitude < 0.0001f || Vector3.Dot(targetRb.linearVelocity.normalized, CalculateTargetDirection(target).normalized) < -0.8f)
            Flee(target, rb);
        else
        {
            var currentSqrSpeed = rb.linearVelocity.sqrMagnitude;
            var sqrDistanceToTarget = CalculateTargetDirection(target).sqrMagnitude;
            var prediction = CalculatePrediction(sqrDistanceToTarget, currentSqrSpeed);

            var explicitTarget = CalculatePredictionExplicitTarget(target, targetRb, prediction);
            Flee(explicitTarget, rb);
        }
    }

    public virtual void Wander(Rigidbody rb)
    {
        var displacement = CalculateWanderDisplacement(rb.linearVelocity);

        var wanderDirection = CalculateWanderDirection(rb.linearVelocity.normalized, displacement);

        var steeringDirection = CalculateSteeringDirection(wanderDirection, rb.linearVelocity);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb.linearVelocity);

        DisplayVectors(rb.linearVelocity, wanderDirection, steeringDirection);

        rb.linearVelocity = CalculateFinalVelocity(finalDirection);
    }

    public virtual void FollowPath(Rigidbody rb)
    {
        var target = _pathPoints[_currentRouteIndex].position;

        _sqrRemainingDistance = (target - transform.position).sqrMagnitude;

        if (CheckNextPoint())
        {
            target = SetNextRoutePoint();
        }

        Vector3 origin = transform.position + Vector3.up * 1f;
        Vector3 directionToTarget = (target - transform.position).normalized;

        // Debug raycast visual
        Debug.DrawRay(origin, directionToTarget * 2f, Color.red);

        if (Physics.Raycast(origin, directionToTarget, out RaycastHit hit, 2f))
        {
            Debug.DrawLine(origin, hit.point, Color.yellow);

            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 avoidDir = Vector3.Cross(hit.normal, Vector3.up);

                if (Vector3.Dot(avoidDir, directionToTarget) < 0)
                    avoidDir = -avoidDir;

                Vector3 steering = avoidDir * maxSpeed;

                rb.linearVelocity = new Vector3(
                    steering.x,
                    rb.linearVelocity.y,
                    steering.z
                );

                return;
            }
        }

        Seek(target, rb);
    }

    /*
    public virtual void FollowPath(Rigidbody rb)
    {
        var target = _pathPoints[_currentRouteIndex].position;

        _sqrRemainingDistance = (target - transform.position).sqrMagnitude;

        if (CheckNextPoint())
        {
            target = SetNextRoutePoint();
        }

        Seek(target, rb);
    }
    */
    #endregion

    #region Path Functions

    public virtual void InitializePatrolPoints()
    {
        _pathPoints = patrolRoutes.GetPatrolRoute(patrolRoute).ToArray();
    }

    public virtual void SetNewRoute(PatrolRoute newRoute)
    {
        patrolRoute = newRoute;
        _pathPoints = patrolRoutes.GetPatrolRoute(patrolRoute).ToArray();
        _currentRouteIndex = 0;
    }

    public virtual Vector3 SetNextRoutePoint()
    {
        _currentRouteIndex = ++_currentRouteIndex % _pathPoints.Length;
        return _pathPoints.Length == 0
            ? Vector3.zero
            : SetTarget(_pathPoints[_currentRouteIndex].position);
    }

    public virtual Vector3 SetTarget(Vector3 target)
    {
        _pathPending = true;
        return target;
    }

    public virtual bool CheckNextPoint()
    {
        return !_pathPending && _sqrRemainingDistance <= Mathf.Pow(stoppingDistance, 2);
    }

    #endregion

    #region Calculations

    public virtual Vector3 CalculateTargetDirection(Vector3 target)
    {
        return (target - transform.position).normalized * (maxSpeed * Time.fixedDeltaTime);
    }

    public virtual Vector3 CalculateSteeringDirection(Vector3 targetDirection, Vector3 currentVelocity)
    {
        var steeringDirection = targetDirection - currentVelocity;

        return steeringDirection.sqrMagnitude > Mathf.Pow(steeringMaxSpeed, 2)
            ? steeringDirection.normalized * steeringMaxSpeed
            : steeringDirection;
    }

    public virtual float CalculatePrediction(float sqrDistanceToTarget, float currentSqrSpeed)
    {
        return sqrDistanceToTarget / currentSqrSpeed;
    }

    public virtual Vector3 CalculatePredictionExplicitTarget(Vector3 target, Rigidbody targetRb, float prediction)
    {
        return target + targetRb.linearVelocity * prediction;
    }

    public virtual Vector3 CalculateWanderDisplacement(Vector3 rbVelocity)
    {
        var randomPoint = Random.insideUnitCircle;

        return Quaternion.LookRotation(rbVelocity) * new Vector3(randomPoint.x, 0, randomPoint.y);
    }

    public virtual Vector3 CalculateWanderDirection(Vector3 circleCenter, Vector3 displacement)
    {
        return (circleCenter + displacement).normalized * (maxSpeed * Time.fixedDeltaTime);
    }

    public virtual Vector3 CalculateFinalDirection(Vector3 steeringDirection, Vector3 currentVelocity)
    {
        return currentVelocity + steeringDirection;
    }

    public virtual Vector3 CalculateFinalVelocity(Vector3 finalDirection)
    {
        return finalDirection.sqrMagnitude > Mathf.Pow(maxSpeed, 2)
            ? finalDirection.normalized * maxSpeed
            : finalDirection;
    }

    public virtual void DisplayVectors(Vector3 currentVelocity, Vector3 targetDirection, Vector3 steeringDirection)
    {
        if (!areVectorsOnDisplay) return;

        Debug.DrawRay(transform.position, currentVelocity, Color.blue);
        Debug.DrawRay(transform.position, targetDirection, Color.green);
        Debug.DrawRay(transform.position + currentVelocity, steeringDirection * 10, Color.red);
    }

    #endregion
}
