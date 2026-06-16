using UnityEngine;

public class TireLeg : LegBase
{
    public override void Init()
    {
        Animation = GetComponent<RobotAnimation>();
        rb = GetComponent<Rigidbody>();

    }

    public override void CreateSetup()
    {
        this.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = LegData.AllyMaterial;
    }
}
