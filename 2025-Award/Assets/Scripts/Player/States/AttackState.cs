using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player) { }

    public override void Enter()
    {
        // 攻撃アニメーション開始
        _player.Animation.SetTrigger("Attack");
    }

    public override void Update()
    {
        // 攻撃終了後
        if (!_player.Attack.IsAttacking)
        {
            // 移動入力のあるなしで状態を変更する
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
