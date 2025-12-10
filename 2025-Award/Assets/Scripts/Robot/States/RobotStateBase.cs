using UnityEngine;

public abstract class RobotStateBase
{
    protected RobotStateManager stateManager;
    protected Robot _robot;

    protected RobotStateBase(RobotStateManager stateManager, Robot robot)
    {
        this.stateManager = stateManager;
        this._robot = robot;
    }

    public virtual void Enter(Robot robot) { }

    public virtual void Tick(Robot robot) { }

    public virtual void Exit(Robot robot) { }
}
