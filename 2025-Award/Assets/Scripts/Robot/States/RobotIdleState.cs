using UnityEngine;

public class RobotIdleState : RobotStateBase
{
    public RobotIdleState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
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

        if (robot.HandleIdle())
        {
            Debug.Log("hennkou");
            robot.ChangeState(stateManager.WalkState);
        }
    }

    public override void Exit(Robot robot)
    {

    }
}
