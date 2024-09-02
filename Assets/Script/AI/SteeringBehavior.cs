using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    // Atributo que representa el objetivo al que se desea acercar
    //public Transform Target;

    // Velocidad máxima del objeto
    public float maxSpeed = 10f;

    // Fuerza máxima de la aceleración
    public float maxForce = 5f;

    // Velocidad actual del objeto
    public Vector3 velocity;
    // Radio de desaceleración
    public float slowingRadius = 5f;

    public void UpdatePosition()
    {
        transform.position += velocity * Time.deltaTime;
    }
    public void  ClampMagnitude(Vector3 steeringForce)
    {
        // Aplica la fuerza a la velocidad
        velocity = Vector3.ClampMagnitude(velocity + steeringForce * Time.deltaTime, maxSpeed);

    }
    // Función de Seek que calcula la fuerza de dirección hacia el objetivo
    public Vector3 Seek(Transform target)
    {
        // Calcula el vector deseado hacia el objetivo
        Vector3 desired = target.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        // Calcula la fuerza de dirección
        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }
    public Vector3 Flee(Transform target)
    {
        // Calcula el vector deseado alejándose del objetivo
        Vector3 desired = transform.position - target.position;
        desired.Normalize();
        desired *= maxSpeed;

        // Calcula la fuerza de dirección
        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }
    public Vector3 Arrive(Transform target)
    {
        // Calcula el vector hacia el objetivo
        Vector3 desired = target.position - transform.position;
        float distance = desired.magnitude;

        // Si el objeto está dentro del radio de desaceleración
        if (distance < slowingRadius)
        {
            // Escala la velocidad proporcionalmente a la distancia
            desired = desired.normalized * maxSpeed * (distance / slowingRadius);
        }
        else
        {
            // Aplica la velocidad máxima fuera del radio de desaceleración
            desired = desired.normalized * maxSpeed;
        }

        // Calcula la fuerza de dirección
        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }
    //void Update()
    //{
    //    // Calcula la fuerza de dirección
    //    Vector3 steeringForce = Seek(Target);

    //    // Aplica la fuerza a la velocidad
    //    velocity = Vector3.ClampMagnitude(velocity + steeringForce * Time.deltaTime, maxSpeed);

    //    // Actualiza la posición del objeto
    //    transform.position += velocity * Time.deltaTime;
    //}
}
