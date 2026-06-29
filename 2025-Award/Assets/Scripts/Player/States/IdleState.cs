using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerStateManager stateManager, Player player)
    : base(stateManager, player) { }

    public override void Enter()
    {
        _player.Animation.SetBool("IsMoving", false);
    }

    public override void Update()
    {
        // インタラクト入力があるとき
        if (_player.PlayerInputInfo.Interact)
        {
            _player.ChangeState(_stateManager.InteractState);
        }
        // 生成入力があるとき
        if(_player.PlayerInputInfo.Create)
        {
            _player.ChangeState(_stateManager.CreateState);
        }
        // 攻撃入力かつプレイヤーが攻撃可能な時
        if (_player.PlayerInputInfo.IsAttack && !_player.Attack.IsAttacking)
        {
            _player.ChangeState(_stateManager.AttackState);
        }
        // 移動入力があるとき
        if (_player.PlayerInputInfo.IsMoving)
        {
            _player.ChangeState(_stateManager.WalkState);
        }
    }

    public override void Exit() { }
}
