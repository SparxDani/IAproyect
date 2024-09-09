using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIEye : AIEyeBase
{

    public DataView DataViewAttack = new DataView();
    public DataView DataViewFire = new DataView();


    private void Start()
    {
        LoadComponent();
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public override void UpdateScan()
    {
        base.UpdateScan();

        if (ScanViewObj != null)
        {
            DataViewAttack.IsInSight(ScanViewObj.AimOffset);
            DataViewFire.IsInSight(ScanViewObj.AimOffset);
        }
    }

    private void Update()
    {
        base.UpdateScan();
    }
    private void OnValidate()
    {
        mainDataView.CreateMesh();
        DataViewAttack.CreateMesh();
        DataViewFire.CreateMesh();
    }
    private void OnDrawGizmos()
    {

        mainDataView.OnDrawGizmos();
        DataViewAttack.OnDrawGizmos();
        DataViewFire.OnDrawGizmos();

    }

}