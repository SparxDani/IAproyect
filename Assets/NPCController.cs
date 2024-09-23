using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;  
    public Transform player;    
    public Vector3 targetPosition; 
    public float wanderRadius = 10f;  
    public float evadeDistance = 15f; 
    public float stoppingDistance = 2f; 

    private bool isWandering = false;  
    private bool movingToTarget = false;  
    private Vector3 wanderTarget;      

    public void MoveToPosition(Vector3 targetPosition)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(targetPosition);
        }

    }

    public void Wander()
    {
        if (SamplePosition(transform.position, wanderRadius, out wanderTarget))
        {
            MoveToPosition(wanderTarget); 
        }
        Debug.Log("Wandering");
    }

    public bool SamplePosition(Vector3 origin, float range, out Vector3 result)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += origin; 

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true; 
        }
        result = Vector3.zero; 
        return false;
    }

    public void EvadeAndWander()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) < evadeDistance)
        {
            Vector3 directionAwayFromPlayer = transform.position - player.position;
            wanderTarget = transform.position + directionAwayFromPlayer.normalized * evadeDistance; 
            MoveToPosition(wanderTarget);
        }
        else
        {
            Wander(); 
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isWandering)
            {
                isWandering = false;  
                movingToTarget = true;
                MoveToPosition(targetPosition);  
            }
            else
            {
                isWandering = true;  
                movingToTarget = false;
            }
        }

        if (isWandering && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            EvadeAndWander(); 
        }
    }
}
