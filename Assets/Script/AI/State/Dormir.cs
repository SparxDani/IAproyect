using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormir : StateMove
{
    // Start is called before the first frame update
    void Awake()
    {
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        //stateType = StateType.Dormir;
        base.LoadComponent();
         
    }
    public override void Enter()
    {
        Debug.Log("Dormir Enter ");
    }
    public override void Execute()
    {
        Vector3 steeringForce = _SteeringBehavior.Arrive(place);

        _SteeringBehavior.ClampMagnitude(steeringForce);

        _SteeringBehavior.UpdatePosition();

        float distancia = (transform.position - place.position).magnitude;
        if (distancia < 1)
        {
            //_MachineState.ActiveState(StateType.Dormir);
            return;
        }
        Debug.Log("Dormir Execute ");
    }
    public override void Exit()
    {
        Debug.Log("Dormir Exit ");
    }
    // Update is called once per frame
    
}
