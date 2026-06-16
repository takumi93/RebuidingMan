using UnityEngine;

public class CreateState: PlayerState
{
    public CreateState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player){ }

    public override void Enter()
    {
        _player.Create.CreateRobot();
    }

    public override void Update()
    {
        if (!_player.PlayerInputInfo.IsMoving)
        {
            _player.ChangeState(_stateManager.IdleState);
        }
        if (_player.PlayerInputInfo.IsMoving)
        {
            _player.ChangeState(_stateManager.WalkState);
        }
    }

    public override void Exit()
    {

    }
}
