using UnityEngine;

public class AIWanderState : BaseState
{
    [SerializeField] private float wanderMaxSpeed;
    [SerializeField] private float steeringMaxSpeed;

    public override void Construct()
    {
        aIBehaviour.maxSpeed = wanderMaxSpeed;
        aIBehaviour.steeringMaxSpeed = steeringMaxSpeed;
    }

    public override void FixedUpdateState()
    {
        aIBehaviour.Evade(m_enemyAIStateMotor.target.position, m_enemyAIStateMotor.rb, m_enemyAIStateMotor.targetRb);
    }


    public override void Transition()
    {
        if (m_enemyAIStateMotor.stateEnum != AIState.Wander) return;
        base.Transition();
    }
}
