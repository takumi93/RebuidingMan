using UnityEngine;

public class WalkLeg : LegBase
{
    public override void Init()
    {
        Animation = GetComponent<RobotAnimation>();
        rb = GetComponent<Rigidbody>();

    }

    public override void CreateSetup()
    {
        transform.GetComponentInChildren<SkinnedMeshRenderer>().material = LegData.AllyMaterial;
    }
}
