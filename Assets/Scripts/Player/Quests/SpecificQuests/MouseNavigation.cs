using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private List<Transform> _positions = new List<Transform>();
    private int _indexTargetPoint = 0; 
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>(); 
        RunTo();
    }
    
    private void RunTo()
    {
        _navMeshAgent.SetDestination(_positions[_indexTargetPoint].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _positions[_indexTargetPoint].gameObject)
        {
            _indexTargetPoint = (_indexTargetPoint + 1) % _positions.Count; 
            RunTo();
        }
    }
}
