using UnityEngine;

public class RobotIdleState : RobotStateBase
{
    public RobotIdleState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        if (robot.head.IsPatrolling)
        {
            robot.body.Animation.SetTrigger("Walk");
            robot.leg.Animation.SetTrigger("Walk");
        }
        else
        {
            robot.body.Animation.SetTrigger("Idle");
            robot.leg.Animation.SetTrigger("Idle");
        }
    }

    public override void Tick(Robot robot)
    {
        if (robot.HandleIdle())
        {
            robot.ChangeState(stateManager.WalkState);
        }
    }

    public override void Exit(Robot robot)
    {

    }
}
