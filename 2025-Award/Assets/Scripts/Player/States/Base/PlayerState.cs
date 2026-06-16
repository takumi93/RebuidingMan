/// <summary>
/// プレイヤーの状態ごとの処理の基盤
/// </summary>
public abstract class PlayerState: IState
{
    protected PlayerStateManager _stateManager;
    protected Player _player;

    protected PlayerInputInfo _inputInfo;

    protected PlayerState(PlayerStateManager stateManager, Player player)
    {
        _stateManager = stateManager;
        _player = player;
    }

    /// <summary>
    /// このStateに入った時の処理
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// このStateでのUpdate処理
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// このStateから別のStateになるとき行う処理
    /// </summary>
    public virtual void Exit() { }
}