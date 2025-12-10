using UnityEngine;

public class InteractState : IPlayerState
{
    public InteractState(PlayerStateManager stateManager, Player player) 
        : base(stateManager, player) { }

    public override void Enter(Player player)
    {
        player.Interact();
    }

    public override void Tick(Player player, InputInfo inputInfo)
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

    public override void Exit(Player player) { }
}
