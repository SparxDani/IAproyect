using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
    public NavMeshAgent agent;  
    public Transform target;    

    // Función para mover el agente hacia una posición objetivo
    public void MoveToTargetPosition(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    // Función para obtener una posición aleatoria dentro del NavMesh
    public bool SamplePosition(Vector3 targetPosition, float range, out Vector3 result)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    // Función para calcular un Path hacia una posición
    public bool CalculatePath(Vector3 targetPosition, out NavMeshPath path)
    {
        path = new NavMeshPath();
        if (agent.CalculatePath(targetPosition, path))
        {
            agent.SetPath(path);
            return true;
        }
        return false;
    }

    // Función para encontrar el borde más cercano en el NavMesh
    public bool FindClosestEdge(Vector3 position, out NavMeshHit hit)
    {
        if (NavMesh.FindClosestEdge(position, out hit, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    // Función para realizar un Raycast en el NavMesh
    public bool RayCast(Vector3 startPosition, Vector3 endPosition, out NavMeshHit hit)
    {
        if (NavMesh.Raycast(startPosition, endPosition, out hit, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    // Ejemplo de uso
    void Update()
    {
        if (target != null)
        {
            MoveToTargetPosition(target.position);

            Vector3 randomPosition;
            if (SamplePosition(target.position, 5f, out randomPosition))
            {
                Debug.Log("Posición aleatoria dentro del NavMesh: " + randomPosition);
            }

            NavMeshPath path;
            if (CalculatePath(target.position, out path))
            {
                Debug.Log("Path calculado con éxito.");
            }

            NavMeshHit edgeHit;
            if (FindClosestEdge(transform.position, out edgeHit))
            {
                Debug.Log("Borde más cercano encontrado en: " + edgeHit.position);
            }

            NavMeshHit rayHit;
            if (RayCast(transform.position, target.position, out rayHit))
            {
                Debug.Log("RayCast colisionó en: " + rayHit.position);
            }
        }
    }
}