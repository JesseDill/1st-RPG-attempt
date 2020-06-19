using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    [SerializeField] Transform target;
    Ray lastRay;
    // Start is called before the first frame update
    void Start()
    {   
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();

        }
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray,out hit); //hit = whatever the ray collides with in real world
        if (hasHit)
        {
            navMeshAgent.SetDestination(hit.point);
        }
    }
}
