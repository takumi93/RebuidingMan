using UnityEngine;

public class RobotWalkState : RobotStateBase
{
    public RobotWalkState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        robot.Body.Animation.SetTrigger("Walk");
        robot.Leg.Animation.SetTrigger("Walk");
    }

    public override void Tick(Robot robot)
    {
        bool toAttack;
        if (!robot.HandleChase(out toAttack))
        {
            if (toAttack)
            {
                robot.ChangeState(stateManager.AttackState);
            }
            else
            {
                robot.ChangeState(stateManager.IdleState);
            }
        }
    }

    public override void Exit(Robot robot) 
    {
        
    }
}
