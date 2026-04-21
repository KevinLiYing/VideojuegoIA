using System;
using UnityEngine;

public class TaunterAI : AIBehaviour
{
    [Header("Taunter Settings")]
    public float farSpeedMultiplier = 0.3f;
    public float closeSpeedMultiplier = 1.5f;

    public float farSteeringMultiplier = 1.5f;
    public float closeSteeringMultiplier = 0.3f;

    public float detectionRange = 20f;

    public override void Flee(Vector3 target, Rigidbody rb)
    {
        float distance = Vector3.Distance(transform.position, target);

        float t = Mathf.InverseLerp(detectionRange, 0f, distance);

        // aplicamos modificadores de velocidad
        float speedMul = Mathf.Lerp(farSpeedMultiplier, closeSpeedMultiplier, t);
        float steerMul = Mathf.Lerp(farSteeringMultiplier, closeSteeringMultiplier, t);

        float originalMaxSpeed = maxSpeed;
        float originalSteering = steeringMaxSpeed;

        maxSpeed *= speedMul;
        steeringMaxSpeed *= steerMul;

        var targetDirection = -CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = FinalVelocity(finalDirection);

        // hacemos los cambios
        maxSpeed = originalMaxSpeed;
        steeringMaxSpeed = originalSteering;
    }
}