using System;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public float maxSpeed, steeringMaxSpeed = 10.0f;
    public bool displayVectors = true;

    #region Behaviour
    public void Seek(Vector3 target, Rigidbody rb)
    {
        var targetDirection = CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb);

        DisplayVectors(rb.linearVelocity,targetDirection, steeringDirection);

        rb.linearVelocity = FinalVelocity(finalDirection);

    }

    public void Flee(Vector3 target, Rigidbody rb)
    {
        var targetDirection = - CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = FinalVelocity(-finalDirection);

    }

    #endregion

    #region Calculations
    private Vector3 CalculateTargetDirection(Vector3 target)
    {
        return (target - transform.position).normalized * maxSpeed * Time.fixedDeltaTime;
    }

    private Vector3 CalculateSteeringDirection(Vector3 targetDirection, Rigidbody rb)
    {
        var steeringDirection = targetDirection - rb.linearVelocity;

        return steeringDirection.sqrMagnitude > Mathf.Pow(steeringMaxSpeed,2) ?
            steeringDirection.normalized * steeringMaxSpeed :
            steeringDirection;
    }

    private Vector3 CalculateFinalDirection(Vector3 steeringDirection, Rigidbody rb)
    {
        return rb.linearVelocity + steeringDirection;
    }

    private Vector3 FinalVelocity(Vector3 finalDirection)
    {
        return finalDirection.sqrMagnitude > Mathf.Pow(maxSpeed, 2) ?
            finalDirection.normalized * maxSpeed :
            finalDirection;
    }
    #endregion

    private void DisplayVectors(Vector3 currentVelocity, Vector3 targetDirection, Vector3 steeringDirection)
    {
        if (!displayVectors) return;

        Debug.DrawRay(transform.position, currentVelocity, Color.blue);
        Debug.DrawRay(transform.position, targetDirection, Color.green);
        Debug.DrawRay(transform.position + currentVelocity, steeringDirection, Color.red);
        
    }

}
