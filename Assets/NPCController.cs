using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;  // Asignar el NavMeshAgent desde el inspector
    public Transform player;    // Referencia al jugador o al objeto que el NPC debe evadir
    public float wanderRadius = 10f;  // Radio de deambulaci�n
    public float evadeDistance = 15f; // Distancia de evasi�n

    private bool isWandering = false;  // Estado de si el NPC est� deambulando
    private bool isEvading = false;    // Estado de si el NPC est� evadiendo
    private Vector3 wanderTarget;      // Posici�n objetivo de deambulaci�n

    // Funci�n para mover el NPC a una posici�n espec�fica
    public void MoveToPosition(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    // Funci�n para hacer que el NPC deambule por el entorno
    public void Wander()
    {
        if (RandomPosition(transform.position, wanderRadius, out wanderTarget))
        {
            MoveToPosition(wanderTarget); // Mover al nuevo punto aleatorio
        }
    }

    // Funci�n para calcular una posici�n aleatoria dentro de un rango dado
    public bool RandomPosition(Vector3 origin, float range, out Vector3 result)
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

    // Funci�n para que el NPC evada al jugador
    public void Evade()
    {
        if (player != null)
        {
            Vector3 directionAwayFromPlayer = transform.position - player.position;
            Vector3 evadePosition = transform.position + directionAwayFromPlayer.normalized * evadeDistance;
            MoveToPosition(evadePosition);
        }
    }

    // Update se ejecuta en cada frame
    void Update()
    {
        // Al presionar 'W', el NPC comenzar� a deambular continuamente
        if (Input.GetKeyDown(KeyCode.W))
        {
            isWandering = true;
            isEvading = false;
        }

        // Al presionar 'E', el NPC dejar� de deambular y comenzar� a evadir al jugador
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEvading = true;
            isWandering = false;
        }

        // L�gica de deambulaci�n continua
        if (isWandering && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander(); // Llama a Wander cuando llegue al destino actual
        }

        // L�gica de evasi�n continua
        if (isEvading)
        {
            Wander();
            Evade(); // Evade continuamente al jugador
        }
    }
}