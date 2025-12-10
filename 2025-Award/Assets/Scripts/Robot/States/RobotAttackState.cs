using UnityEngine;

public class RobotAttackState : RobotStateBase
{
    public RobotAttackState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        robot.leg.Animation.SetTrigger("Walk");
        robot.body.Attack();
    }

    public override void Tick(Robot robot)
    {
        bool toChase;
        if (!robot.HandleAttack(out toChase))
        {
            if (toChase)
            {
                robot.ChangeState(stateManager.WalkState);
            }else
            {
                robot.ChangeState(stateManager.IdleState);
            }
        }
    }

    public override void Exit(Robot robot)
    {

    }
}
