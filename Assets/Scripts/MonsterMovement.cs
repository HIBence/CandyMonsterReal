using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets; 

    private NavMeshAgent Monster;


    // Start is called before the first frame update
    void Start()
    {
        Monster = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveOverPathOne()
    {
        Monster.SetDestination(targets[0].transform.position);
    }
}
