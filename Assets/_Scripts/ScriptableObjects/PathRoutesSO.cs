using System.Collections.Generic;
using UnityEngine;

public enum PatrolRoute
{
    Route1,
    Route2,
    Route3,
    Route4
}

[CreateAssetMenu(fileName = "PathRoutesSO", menuName = "Scriptable Objects/PathRoutesSO")]
public class PathRoutesSO : ScriptableObject
{
    [SerializeField] private List<Transform> patrolRoute01;
    [SerializeField] private List<Transform> patrolRoute02;
    [SerializeField] private List<Transform> patrolRoute03;
    [SerializeField] private List<Transform> patrolRoute04;

    public List<Transform> GetPatrolRoute(PatrolRoute patrolRoute)
    {
        switch (patrolRoute)
        {
            case PatrolRoute.Route1:
                return patrolRoute01;
            case PatrolRoute.Route2:
                return patrolRoute02;
            case PatrolRoute.Route3:
                return patrolRoute03;
            case PatrolRoute.Route4:
                return patrolRoute04;
            default:
                return patrolRoute01; 
        }
    }
}
