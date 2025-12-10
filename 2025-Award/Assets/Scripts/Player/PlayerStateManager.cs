using UnityEngine;

public class PlayerStateManager
{
    private Player player;

    // Œ»İ‚Ìó‘Ô
    public IPlayerState CurrentState { get; private set; }

    public IdleState IdleState { get; private set; }

    public WalkState WalkState { get; private set; }

    public InteractState InteractState { get; private set; }

    public AttackState AttackState { get; private set; }

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    /// <param name="player"></param>
    /// <param name="state"></param>
    public PlayerStateManager(Player player)
    {
        this.player = player;

        IdleState = new IdleState(this, player);
        WalkState = new WalkState(this, player);
        AttackState = new AttackState(this, player);
        InteractState = new InteractState(this, player);

        CurrentState = IdleState;
        CurrentState.Enter(player);
    }

    /// <summary>
    /// ƒvƒŒƒCƒ„[‚Ìó‘Ô‚ğ‘JˆÚ‚·‚é‚Ìˆ—
    /// </summary>
    /// <param name="player"></param>
    /// <param name="newstate"></param>
    public void ChangeState(IPlayerState newstate)
    {
        CurrentState?.Exit(player);
        CurrentState = newstate;
        CurrentState.Enter(player);
    }
}
