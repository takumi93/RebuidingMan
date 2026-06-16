using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player) { }

    public override void Enter()
    {
        // 攻撃アニメーション開始
        _player.Animation.SetTrigger("Attack");
        // 攻撃開始フラグ
        _player.Animation.AttackStart();
    }

    public override void Update()
    {
        // 攻撃終了後
        if (!_player.Animation.Attack)
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
    }

    public override void Exit() { }
}
