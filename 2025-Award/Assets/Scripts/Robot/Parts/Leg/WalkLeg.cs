using UnityEngine;

public class WalkLeg : LegBase
{
    [Header("ë´Ç…ïKóvÇ»ê›íË")]
    // âÒì]ë¨ìx
    [SerializeField] protected int _rotateSpeed;

    public override void CreateSetup()
    {
        UpdateMaterial(LegData);
    }

    public override void Move(Vector3 targetPos)
    {
        _agent.isStopped = false;
        _agent.destination = targetPos;

        _agent.stoppingDistance = _robot.MoveStoppingDistance;

        Vector3 dir = targetPos - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            // ê˘âÒ
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            Transform robotRoot = transform.parent;

            robotRoot.rotation =
                Quaternion.Slerp(
                    robotRoot.rotation,
                    targetRotation,
                    _rotateSpeed * Time.deltaTime);
        }
    }
}
