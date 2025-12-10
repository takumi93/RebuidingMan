using UnityEngine;

public class AttackState : IPlayerState
{
    public AttackState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player) { }

    public override void Enter(Player player)
    {
        player.Animation.SetTrigger("Attack");
        player.Animation.AttackStart();
        player.Attack();
    }

    public override void Tick(Player player, InputInfo inputInfo)
    {
        if (!player.Animation.Attack)
        {
            if (!inputInfo.IsWalk)
            {
                player.ChangeState(stateManager.IdleState);
            }
            if (inputInfo.IsWalk)
            {
                player.ChangeState(stateManager.WalkState);
            }
        }
    }

    public override void Exit(Player player) { }
}
