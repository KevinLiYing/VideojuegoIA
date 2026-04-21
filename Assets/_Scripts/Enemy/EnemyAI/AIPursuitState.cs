using UnityEngine;

public class AIPursuitState : BaseState
{
    [SerializeField] public float pursueMaxSpeed;
    [SerializeField] public float steeringMaxSpeed;

    public override void Construct()
    {
        aIBehaviour.maxSpeed = pursueMaxSpeed;
        aIBehaviour.steeringMaxSpeed = steeringMaxSpeed;
    }

    public override void Transition()
    {
        if (m_enemyAIStateMotor.stateEnum != AIState.Pursue) return;
        base.Transition();
    }

    public override void FixedUpdateState()
    {
        aIBehaviour.Pursue(m_enemyAIStateMotor.target.position, m_enemyAIStateMotor.rb, m_enemyAIStateMotor.targetRb);
    }
}
