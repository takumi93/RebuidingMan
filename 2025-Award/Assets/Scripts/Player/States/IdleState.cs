using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerStateManager stateManager, Player player)
    : base(stateManager, player) { }

    public override void Enter()
    {
        _player.Animation.SetTrigger("Idle");
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
        if (_player.PlayerInputInfo.IsMoving)
        {
            _player.ChangeState(_stateManager.WalkState);
        }
    }

    public override void Exit() { }
}
