using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCing : StateMove
{


    // Start is called before the first frame update
    void Awake()
    {
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        //stateType = StateType.WCing;
        base.LoadComponent();
         
    }
    public override void Enter()
    {
        Debug.Log("WCing Enter ");
    }
    public override void Execute()
    {
        Vector3 steeringForce = _SteeringBehavior.Arrive(place);

        _SteeringBehavior.ClampMagnitude(steeringForce);

        _SteeringBehavior.UpdatePosition();
        Debug.Log("WCing Execute ");
    }
    public override void Exit()
    {
        Debug.Log("WCing Exit ");
    }
     
}
