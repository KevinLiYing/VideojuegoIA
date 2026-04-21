using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    public float maxSpeed, steeringMaxSpeed, stoppingDistance = 10.0f;
    public bool displayVectors = true;

    #region Behaviour
    public virtual void Seek(Vector3 target, Rigidbody rb)
    {
        var distance = Vector3.Distance(transform.position, target);

        float arriveFactor = Arrive(target);

        var targetDirection = CalculateTargetDirection(target) * arriveFactor;

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = FinalVelocity(finalDirection);
    }

    public virtual void Flee(Vector3 target, Rigidbody rb)
    {

        var targetDirection = - CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = FinalVelocity(finalDirection);

    }

    public void Pursue(Vector3 target, Rigidbody rb, Rigidbody targetRb)
    {
        if (rb.linearVelocity.sqrMagnitude < 0.0001f ||
                Vector3.Dot(
                    targetRb.linearVelocity.normalized,
                    CalculateTargetDirection(target).normalized) < -0.0f)
        {
            Seek(target, rb);
        }
        else
        {
            var currentSqrSpeed = rb.linearVelocity.sqrMagnitude;
            var sqrDistanceToTarget = CalculateTargetDirection(target).sqrMagnitude;
            var prediction = CalculatePrediction(sqrDistanceToTarget, currentSqrSpeed);

            var explicitTarget = CalculatePredictionTarget(target, targetRb, prediction);
            Seek(explicitTarget, rb);
        }
    }

    public void Evade(Vector3 target, Rigidbody rb, Rigidbody targetRb)
    {
        if (rb.linearVelocity.sqrMagnitude < 0.0001f ||
                Vector3.Dot(
                    targetRb.linearVelocity.normalized,
                    CalculateTargetDirection(target).normalized) < -0.0f)
        {
            Seek(target, rb);
        }
        else
        {
            var currentSqrSpeed = rb.linearVelocity.sqrMagnitude;
            var sqrDistanceToTarget = CalculateTargetDirection(target).sqrMagnitude;
            var prediction = CalculatePrediction(sqrDistanceToTarget, currentSqrSpeed);

            var explicitTarget = CalculatePredictionTarget(target, targetRb, prediction);
            Seek(explicitTarget, rb);
        }
    }

    public void Wander(Rigidbody rb)
    {
        var displacement = CalculateWanderDisplacement(rb.linearVelocity);

        var wanderDirection = CalculateWanderDirection(rb.linearVelocity.normalized, displacement);

        var steeringDirection = CalculateSteeringDirection(wanderDirection, rb);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb);

        DisplayVectors(rb.linearVelocity, wanderDirection, steeringDirection);

        rb.linearVelocity = FinalVelocity(finalDirection);
    }



    public virtual float Arrive(Vector3 target)
    {
        float distance = Vector3.Distance(transform.position, target);

        if (distance > stoppingDistance)
            return 1f;

        return Mathf.Clamp01(distance / stoppingDistance);
    }

    #endregion

    #region Calculations
    public Vector3 CalculateTargetDirection(Vector3 target)
    {
        return (target - transform.position).normalized * maxSpeed * Time.fixedDeltaTime;
    }

    public Vector3 CalculateSteeringDirection(Vector3 targetDirection, Rigidbody rb)
    {
        var steeringDirection = targetDirection - rb.linearVelocity;

        return steeringDirection.sqrMagnitude > Mathf.Pow(steeringMaxSpeed,2) ?
            steeringDirection.normalized * steeringMaxSpeed :
            steeringDirection;
    }

    public Vector3 CalculateFinalDirection(Vector3 steeringDirection, Rigidbody rb)
    {
        return rb.linearVelocity + steeringDirection;
    }

    public Vector3 FinalVelocity(Vector3 finalDirection)
    {
        return finalDirection.sqrMagnitude > Mathf.Pow(maxSpeed, 2) ?
            finalDirection.normalized * maxSpeed :
            finalDirection;
    }

    private float CalculatePrediction(float sqrDistanceToTarget, float currentSqrSpeed)
    {
        return sqrDistanceToTarget / currentSqrSpeed;
    }

    private Vector3 CalculatePredictionTarget(Vector3 target, Rigidbody targetRb, float prediction)
    {
        return target + targetRb.linearVelocity * prediction;
    }

    private Vector3 CalculateWanderDisplacement(Vector3 linearVelocity)
    {
        var randomPoint = Random.insideUnitCircle;

       return Quaternion.LookRotation(linearVelocity) * new Vector3(randomPoint.x, 0, randomPoint.y);

          
    }

    private Vector3 CalculateWanderDirection(Vector3 circleCenter, Vector3 displacement)
    {
        return (circleCenter + displacement).normalized * (maxSpeed * Time.fixedDeltaTime);
    }
    #endregion

    public void DisplayVectors(Vector3 currentVelocity, Vector3 targetDirection, Vector3 steeringDirection)
    {
        if (!displayVectors) return;

        Debug.DrawRay(transform.position, currentVelocity, Color.blue);
        Debug.DrawRay(transform.position, targetDirection, Color.green);
        Debug.DrawRay(transform.position + currentVelocity, steeringDirection, Color.red);
        
    }

}
