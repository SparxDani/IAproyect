using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugar : StateMove
{


    // Start is called before the first frame update
    void Awake()
    {
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        //stateType = StateType.Jugar;
        base.LoadComponent();
        
    }
    public override void Enter()
    {
        Debug.Log("Jugar Enter ");
    }
    public override void Execute()
    {
         
        Vector3 steeringForce = _SteeringBehavior.Arrive(place);

        _SteeringBehavior.ClampMagnitude(steeringForce);

        _SteeringBehavior.UpdatePosition();

        float distancia = (transform.position - place.position).magnitude;
        if(distancia<1)
        {
            _MachineState.ActiveState(StateType.Jugando);
            return;
        }
        Debug.Log("Jugar Execute ");
    }
    public override void Exit()
    {
        Debug.Log("Jugar Exit ");
    }
    // Update is called once per frame
     
}
