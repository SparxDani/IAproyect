using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : StateBase
{
    protected SteeringBehavior _SteeringBehavior;
    public Transform place;

    public override void LoadComponent()
    {
        _SteeringBehavior = GetComponent<SteeringBehavior>();
        base.LoadComponent();
    }

    public virtual void MoveToPlace()
    {
        Vector3 steeringForce = _SteeringBehavior.Arrive(place);

        Vector3 avoidForce = _SteeringBehavior.WallAvoidance();

        Vector3 totalForce = steeringForce + avoidForce;

        _SteeringBehavior.ClampMagnitude(totalForce);

        _SteeringBehavior.UpdatePosition();
    }
}
