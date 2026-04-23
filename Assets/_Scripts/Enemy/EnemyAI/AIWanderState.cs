using UnityEngine;

public class AIWanderState : BaseState
{
    [SerializeField] private float wanderMaxSpeed;
    [SerializeField] private float steeringMaxSpeed;

    public override void Construct()
    {
        aiBehaviour.maxSpeed = wanderMaxSpeed;
        aiBehaviour.steeringMaxSpeed = steeringMaxSpeed;
    }

    public override void FixedUpdateState()
    {
        aiBehaviour.Evade(m_enemyAIStateMotor.target.position, m_enemyAIStateMotor.rb, m_enemyAIStateMotor.targetRb);
    }


    public override void Transition()
    {
        if (m_enemyAIStateMotor.stateEnum != AIState.Wander) return;
        base.Transition();
    }
}
