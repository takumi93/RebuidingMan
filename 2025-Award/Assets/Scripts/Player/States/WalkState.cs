using UnityEngine;

public class WalkState : IPlayerState
{
    public WalkState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player){ }

    public override void Enter(Player player)
    {
        player.Animation.SetTrigger("Walk");
    }

    public override void Tick(Player player, InputInfo inputInfo)
    {
        if (inputInfo.Interact)
        {
            player.ChangeState(stateManager.InteractState);
        }
        if (inputInfo.IsAttack && !player.Animation.Attack)
        {
            player.ChangeState(stateManager.AttackState);
        }
        if (!inputInfo.IsWalk)
        {
            player.ChangeState(stateManager.IdleState);
        }
    }

    public override void Exit(Player player) { }
}
