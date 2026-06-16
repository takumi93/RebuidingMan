/// <summary>
/// Stateを使うための基盤（プレイヤーとロボットで使用中）
/// </summary>
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}
