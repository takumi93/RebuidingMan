using UnityEngine;

public class RobotAttackState : RobotStateBase
{
    public RobotAttackState(RobotStateManager stateManager, Robot robot) 
        : base(stateManager, robot){ }

    public override void Enter(Robot robot)
    {
        robot.Leg.Animation.SetTrigger("Walk");
    }

    public override void Tick(Robot robot)
    {
        robot.Body.Attack();

        bool toChase;
        if (!robot.HandleAttack(out toChase))
        {
            if (toChase)
            {
                Debug.Log("aruki");
                robot.ChangeState(stateManager.WalkState);
            }else
            {
                Debug.Log("aidoru");
                robot.ChangeState(stateManager.IdleState);
            }

            return;
        }

        if (!robot.Body.IsAttacking)
        {
            robot.ChangeState(stateManager.WalkState);
        }
    }

    public override void Exit(Robot robot)
    {

    }
}
