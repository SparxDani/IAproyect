using System.Collections;
using UnityEngine;

public class Jugando : StateWait
{
    private float maxWaitTime = 5f; // Tiempo máximo de espera sin detectar juguetes
    private float currentWaitTime = 0f;
    private bool toyDetected = false;
    private GameObject targetToy; // El juguete detectado
    public GameObject[] toyPrefabs; // Prefabs de juguetes a generar
    public Transform roomBounds; // Límites de la habitación
    public float moveSpeed = 5f; // Velocidad de movimiento del NPC

    void Awake()
    {
        this.LoadComponent();
    }

    public override void LoadComponent()
    {
        stateType = StateType.Jugando;
        base.LoadComponent();
    }

    public override void Enter()
    {
        base.Enter();
        stateNode = StateNode.MoveTo;
        Debug.Log("Jugando Enter");
        GenerateRandomToys();
        currentWaitTime = 0f;
        toyDetected = false;
        targetToy = null;
    }

    public override void Execute()
    {
        base.Execute();
        currentWaitTime += Time.deltaTime;

        switch (stateNode)
        {
            case StateNode.MoveTo:
                // Movimiento aleatorio por la habitación mientras no se detecte un juguete
                if (!toyDetected)
                {
                    base.MoveToPlace();
                    float distance = (transform.position - place.position).magnitude;
                    if (distance < 1)
                    {
                        stateNode = StateNode.StartStay;
                    }
                }
                else if (targetToy != null)
                {
                    MoveToToy(); // Se dirige al juguete detectado
                }
                break;

            case StateNode.StartStay:
                StartCoroutineWait();
                stateNode = StateNode.Stay;
                break;

            case StateNode.Stay:
                if (!WaitTime)
                {
                    _MachineState.ActiveState(GetRandomStateType());
                    return;
                }

                if (currentWaitTime > maxWaitTime)
                {
                    _MachineState.ActiveState(GetRandomStateType()); // Cambia al siguiente estado si no detecta juguetes
                    return;
                }

                Debug.Log("Jugando Execute");
                break;

            case StateNode.Finish:
                break;

            default:
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
        stateNode = StateNode.MoveTo;
        Debug.Log("Jugando Exit");
    }

    private void GenerateRandomToys()
    {
        // Genera juguetes en posiciones aleatorias dentro de los límites de la habitación
        for (int i = 0; i < toyPrefabs.Length; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(roomBounds.position.x - roomBounds.localScale.x / 2, roomBounds.position.x + roomBounds.localScale.x / 2),
                0, // Suponiendo que los juguetes están en el suelo
                Random.Range(roomBounds.position.z - roomBounds.localScale.z / 2, roomBounds.position.z + roomBounds.localScale.z / 2)
            );

            Instantiate(toyPrefabs[i], randomPosition, Quaternion.identity);
        }
    }

    private void MoveToToy()
    {
        // Mueve al NPC hacia el juguete detectado
        if (targetToy != null)
        {
            Vector3 direction = (targetToy.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            float distanceToToy = Vector3.Distance(transform.position, targetToy.transform.position);
            if (distanceToToy < 1f) // Suponiendo que al estar a menos de 1 unidad de distancia, el juguete se recoge
            {
                Debug.Log("Juguete recogido: " + targetToy.name);
                Destroy(targetToy);
                toyDetected = false; // Resetea la detección del juguete
                currentWaitTime = 0f; // Reinicia el tiempo de espera
                targetToy = null; // Resetea el objetivo del juguete
            }
        }
    }

    public void OnToyDetected(GameObject toy)
    {
        // Método llamado cuando un juguete es detectado por el sensor
        toyDetected = true;
        targetToy = toy; // Asigna el juguete detectado como objetivo
        currentWaitTime = 0f; // Reinicia el temporizador cuando se detecta un juguete
    }
}
