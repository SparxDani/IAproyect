using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;  // Asignar el NavMeshAgent desde el inspector
    public Transform player;    // Referencia al jugador o al objeto que el NPC debe evadir
    public float wanderRadius = 10f;  // Radio de deambulación
    public float evadeDistance = 15f; // Distancia de evasión

    private bool isWandering = false;  // Estado de si el NPC está deambulando
    private bool isEvading = false;    // Estado de si el NPC está evadiendo
    private Vector3 wanderTarget;      // Posición objetivo de deambulación

    // Función para mover el NPC a una posición específica
    public void MoveToPosition(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    // Función para hacer que el NPC deambule por el entorno
    public void Wander()
    {
        if (RandomPosition(transform.position, wanderRadius, out wanderTarget))
        {
            MoveToPosition(wanderTarget); // Mover al nuevo punto aleatorio
        }
    }

    // Función para calcular una posición aleatoria dentro de un rango dado
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

    // Función para que el NPC evada al jugador
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
        // Al presionar 'W', el NPC comenzará a deambular continuamente
        if (Input.GetKeyDown(KeyCode.W))
        {
            isWandering = true;
            isEvading = false;
        }

        // Al presionar 'E', el NPC dejará de deambular y comenzará a evadir al jugador
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEvading = true;
            isWandering = false;
        }

        // Lógica de deambulación continua
        if (isWandering && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander(); // Llama a Wander cuando llegue al destino actual
        }

        // Lógica de evasión continua
        if (isEvading)
        {
            Wander();
            Evade(); // Evade continuamente al jugador
        }
    }
}