using UnityEngine;

public class PlayerStateManager
{
    private Player _player;

    // Œ»İ‚Ìó‘Ô
    public PlayerState CurrentState { get; private set; }

    public IdleState IdleState { get; private set; }

    public WalkState WalkState { get; private set; }

    public InteractState InteractState { get; private set; }

    public AttackState AttackState { get; private set; }

    public CreateState CreateState { get; private set; }

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    /// <param name="player"></param>
    /// <param name="state"></param>
    public PlayerStateManager(Player player)
    {
        _player = player;

        IdleState = new IdleState(this, player);
        WalkState = new WalkState(this, player);
        AttackState = new AttackState(this, player);
        InteractState = new InteractState(this, player);
        CreateState = new CreateState(this, player);

        CurrentState = IdleState;
        CurrentState.Enter();
    }

    /// <summary>
    /// ƒvƒŒƒCƒ„[‚Ìó‘Ô‚ğ‘JˆÚ‚·‚é‚Ìˆ—
    /// </summary>
    /// <param name="player"></param>
    /// <param name="newstate"></param>
    public void ChangeState(PlayerState newstate)
    {
        CurrentState?.Exit();
        CurrentState = newstate;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
