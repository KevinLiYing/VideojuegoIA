using UnityEngine;

public class AIPursuitState : BaseState
{
    [SerializeField] public float pursueMaxSpeed;
    [SerializeField] public float steeringMaxSpeed;

    public override void Construct()
    {
        aiBehaviour.maxSpeed = pursueMaxSpeed;
        aiBehaviour.steeringMaxSpeed = steeringMaxSpeed;
    }

    public override void Transition()
    {
        if (m_enemyAIStateMotor.stateEnum != AIState.Pursue) return;
        base.Transition();
    }

    public override void FixedUpdateState()
    {
        aiBehaviour.Pursue(m_enemyAIStateMotor.target.position, m_enemyAIStateMotor.rb, m_enemyAIStateMotor.targetRb);
    }
}
