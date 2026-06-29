using UnityEngine;

public class RobotWalkState : RobotStateBase
{
    public RobotWalkState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        robot.Body.Animation.SetBool("IsMoving", true);
        robot.Leg.Animation.SetBool("IsMoving", true);
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
