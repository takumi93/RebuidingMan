using UnityEngine;

public class RobotStateManager
{
    private Robot _robot;

    // Œ»İ‚Ìó‘Ô
    public RobotStateBase CurrentState { get; private set; }

    public RobotIdleState IdleState { get; private set; }

    public RobotWalkState WalkState { get; private set; }

    public RobotAttackState AttackState { get; private set; }

    public RobotStateManager(Robot robot)
    {
        _robot = robot;

        IdleState = new RobotIdleState(this, _robot);
        WalkState = new RobotWalkState(this, _robot);
        AttackState = new RobotAttackState(this, _robot);

        CurrentState = IdleState;

        CurrentState.Enter(_robot);
    }

    /// <summary>
    /// ƒƒ{ƒbƒg‚Ìó‘Ô‚ğ‘JˆÚ‚·‚é‚Ìˆ—
    /// </summary>
    /// <param name="_robot"></param>
    /// <param name="newstate"></param>
    public void ChangeState(RobotStateBase newstate)
    {
        CurrentState?.Exit(_robot);
        CurrentState = newstate;
        CurrentState.Enter(_robot);
    }
}
