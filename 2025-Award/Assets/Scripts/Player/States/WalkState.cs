using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player){ }

    public override void Enter()
    {
        _player.Animation.SetTrigger("Walk");
    }

    public override void Update()
    {
        if (_player.PlayerInputInfo.Interact)
        {
            _player.ChangeState(_stateManager.InteractState);
        }
        if (_player.PlayerInputInfo.IsAttack && !_player.Animation.Attack)
        {
            _player.ChangeState(_stateManager.AttackState);
        }
        if (!_player.PlayerInputInfo.IsMoving)
        {
            _player.ChangeState(_stateManager.IdleState);
        }
    }

    public override void Exit() { }
}
