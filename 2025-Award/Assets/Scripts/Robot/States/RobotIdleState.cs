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
            robot.Body.Animation.SetTrigger("Walk");
            robot.Leg.Animation.SetTrigger("Walk");
        }
        else
        {
            robot.Body.Animation.SetTrigger("Idle");
            robot.Leg.Animation.SetTrigger("Idle");
        }
    }

    public override void Tick(Robot robot)
    {
        // 移動先があるなしでアニメーションの変更
        if (robot.MoveTarget.HasValue)
        {
            robot.Body.Animation.SetTrigger("Walk");
            robot.Leg.Animation.SetTrigger("Walk");
        }
        else
        {
            robot.Body.Animation.SetTrigger("Idle");
            robot.Leg.Animation.SetTrigger("Idle");
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
