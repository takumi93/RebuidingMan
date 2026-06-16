using UnityEngine;

public class RobotIdleState : RobotStateBase
{
    public RobotIdleState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        if (robot.Head.IsPatrolling)
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
        if (robot.HandleIdle())
        {
            robot.ChangeState(stateManager.WalkState);
        }
    }

    public override void Exit(Robot robot)
    {

    }
}
