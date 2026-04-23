using UnityEngine;

public class ChaserAI : AIBehaviour
{
    public override void Seek(Vector3 target, Rigidbody rb)
    {
        var targetDirection = CalculateTargetDirection(target);

        var steeringDirection = CalculateSteeringDirection(targetDirection, rb.linearVelocity);

        var finalDirection = CalculateFinalDirection(steeringDirection, rb.linearVelocity);

        DisplayVectors(rb.linearVelocity, targetDirection, steeringDirection);

        rb.linearVelocity = CalculateFinalVelocity(finalDirection);
    }
}
