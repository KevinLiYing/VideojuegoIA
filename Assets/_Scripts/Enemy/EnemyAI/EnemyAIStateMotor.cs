using UnityEngine;


public enum AIState
{
    Flee,
    Seek,
    Evade,
    Pursue,
    FollowPath,
    Wander
}

[RequireComponent(typeof(AIPatrolState),typeof(AIIdleState),typeof(AISeekState))]
[RequireComponent(typeof(AIFleeState),typeof(Rigidbody),typeof(Animator))]
[RequireComponent(typeof(AIEvadeState),typeof(AIPursuitState),typeof(AIWanderState))]
[RequireComponent(typeof(AIFollowPathState), typeof(AIBehaviour))]
public class EnemyAIStateMotor : MonoBehaviour
{
    [Header("State")] 
    public AIState stateEnum;
    [Header("AI Components")]
    public Animator anim;
    public Rigidbody rb;
    //public EnemyAIBehaviour enemyBehaviour;
    //public FPSPlayer player;
    [Header("Target Components")]
    public Transform target;
    public Rigidbody targetRb;
    [Header("Booleans")]
    public bool isPlayerOnSight, isIdleDone;

    private BaseState m_state;

    private void Awake()
    {
        //enemyBehaviour = GetComponent<EnemyAIBehaviour>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_state = GetComponent<AISeekState>();
    }

    private void Start()
    {
        targetRb = target.GetComponent<Rigidbody>();
        m_state.Construct();
    }

    private void Update()
    {
        Debug.Log("Estado actual: " + m_state);
        //if(!GameManager.Instance.isPaused)
        UpdateMotor();
    }

    private void FixedUpdate()
    {
        m_state.FixedUpdateState();
    }

    private void UpdateMotor()
    {
        m_state.Transition();
        //m_state.UpdateState();
    }

    public void ChangeState(BaseState newState)
    {
        m_state.Destruct();
        m_state = newState;
        
        m_state.Construct();
    }
}
