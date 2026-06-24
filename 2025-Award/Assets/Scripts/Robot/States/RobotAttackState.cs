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

        // çUåÇç≈íÜÇ»ÇÁñ≥éã
        if (robot.HandleAttack()) return;

        // ìGÇ™Ç¢ÇÈéû
        if (robot.Target)
        {
            robot.ChangeState(stateManager.WalkState);
        }
        else
        {
            robot.ChangeState(stateManager.IdleState);
        }

        //if (!robot.Body.IsAttacking)
        //{
        //    if(robot.Target)
        //    {
        //        robot.MoveTarget = robot.Target.position;
        //    }

        //    robot.ChangeState(stateManager.WalkState);
        //}
    }

    public override void Exit(Robot robot)
    {

    }
}
