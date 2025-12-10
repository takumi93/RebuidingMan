/// <summary>
/// ƒvƒŒƒCƒ„[‚Ìó‘Ô‚²‚Æ‚Ìˆ—‚ÌŠî”Õ
/// </summary>
public abstract class IPlayerState
{
    protected PlayerStateManager stateManager;
    protected Player player;

    protected IPlayerState(PlayerStateManager stateManager, Player player)
    {
        this.stateManager = stateManager;
        this.player = player;
    }

    public virtual void Enter(Player player) { }

    public virtual void Tick(Player player , InputInfo inputInfo) { }

    public virtual void Exit(Player player) { }
}
