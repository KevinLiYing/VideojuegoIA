using System;
using UnityEngine;

// Literally bot will never touch the player, as if there was an invisible barrier, unless the player touches the bot
public class InfinityAI : AIBehaviour
{
    public override void Seek(Vector3 target, Rigidbody rb)
    {
        var targetDirection = CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb.linearVelocity);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb.linearVelocity);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = CalculateFinalVelocity(finalDirection) * Arrive(target);

    }
    public override float Arrive(Vector3 target)
    {
        var sqrDistance = (target - transform.position).sqrMagnitude;
        if (sqrDistance > Mathf.Pow(stoppingDistance, 2)) return 1;

        // nos devolverá un número entre 1 y 0
        return (sqrDistance / MathF.Pow(stoppingDistance, 2));
    }
}
