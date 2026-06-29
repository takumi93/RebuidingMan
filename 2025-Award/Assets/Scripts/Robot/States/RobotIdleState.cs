using UnityEngine;

public class RobotIdleState : RobotStateBase
{
    public RobotIdleState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        // 移動先があるなしでアニメーションの変更
        if (robot.MoveTarget.HasValue)
        {
            robot.Body.Animation.SetBool("IsMoving", true);
            robot.Leg.Animation.SetBool("IsMoving", true);
        }
        else
        {
            robot.Body.Animation.SetBool("IsMoving", false);
            robot.Leg.Animation.SetBool("IsMoving", false);
        }
    }

    public override void Tick(Robot robot)
    {
        // 移動先があるなしでアニメーションの変更
        if (robot.MoveTarget.HasValue)
        {
            robot.Body.Animation.SetBool("IsMoving", true);
            robot.Leg.Animation.SetBool("IsMoving", true);
        }
        else
        {
            robot.Body.Animation.SetBool("IsMoving", false);
            robot.Leg.Animation.SetBool("IsMoving", false);
        }

        // 敵を見つけたときの処理
        if (robot.HandleIdle())
        {
            robot.ChangeState(stateManager.WalkState);
        }
    }

    public override void Exit(Robot robot)
    {

    }
}
