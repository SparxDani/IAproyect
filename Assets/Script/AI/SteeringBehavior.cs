using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float maxForce = 5f;
    public Vector3 velocity;
    public float slowingRadius = 5f;

    public float wallAvoidanceForce = 20f; 
    public float wallDetectionDistance = 5f; 
    public LayerMask wallLayerMask; 

    public void UpdatePosition()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void ClampMagnitude(Vector3 steeringForce)
    {
        velocity = Vector3.ClampMagnitude(velocity + steeringForce * Time.deltaTime, maxSpeed);
    }

    public Vector3 Seek(Transform target)
    {
        Vector3 desired = target.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steering = desired - velocity;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    public Vector3 Flee(Transform target)
    {
        Vector3 desired = transform.position - target.position;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steering = desired - velocity;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    public Vector3 Arrive(Transform target)
    {
        Vector3 desired = target.position - transform.position;
        float distance = desired.magnitude;

        if (distance < slowingRadius)
        {
            desired = desired.normalized * maxSpeed * (distance / slowingRadius);
        }
        else
        {
            desired = desired.normalized * maxSpeed;
        }

        Vector3 steering = desired - velocity;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    public Vector3 WallAvoidance()
    {
        RaycastHit hit;
        Vector3 avoidForce = Vector3.zero;

        Vector3 rayDirection = velocity.normalized;

        Debug.DrawRay(transform.position, rayDirection * wallDetectionDistance, Color.red);

        if (Physics.Raycast(transform.position, rayDirection, out hit, wallDetectionDistance, wallLayerMask))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                avoidForce = hit.normal * wallAvoidanceForce;

                Debug.DrawRay(hit.point, hit.normal * wallAvoidanceForce, Color.green);
            }
        }

        return avoidForce;
    }
}
