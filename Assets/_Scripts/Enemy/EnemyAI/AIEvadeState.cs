using UnityEngine;

public class AIEvadeState : BaseState
{
    [SerializeField] private float evadeMaxSpeed;
    [SerializeField] private float steeringMaxSpeed;

    public override void Construct()
    {
        aIBehaviour.maxSpeed = evadeMaxSpeed;
        aIBehaviour.steeringMaxSpeed = steeringMaxSpeed;
    }

    public override void FixedUpdateState()
    {
        aIBehaviour.Evade(m_enemyAIStateMotor.target.position, m_enemyAIStateMotor.rb, m_enemyAIStateMotor.targetRb);
    }


    public override void Transition()
    {
        if (m_enemyAIStateMotor.stateEnum != AIState.Evade) return;
        base.Transition();
    }
}
