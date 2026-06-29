using UnityEngine;
public class PawnHead : HeadBase 
{ 
    [Header("巡回ルート")]
    [SerializeField] private PatrolRoute _patrolRoute; 
    private int _currentPoint = 0; 
    
    public override void Init(Robot robot) 
    { 
        base.Init(robot);
        IsPatrolling = true;
        SetNextDestination();
    } 
    
    /// <summary>
    /// 味方になった時のセットアップ 
    /// </summary> 
    public override void CreateSetup() 
    {
        UpdateMaterial(HeadData);

        // 巡回ルートの設定
        _patrolRoute = StageScene.Instance.AllyRoute;
    } 
    
    /// <summary> 
    /// 敵を見つけたとき移動先を敵にする
    /// </summary> 
    public override void ChaseTarget() 
    {
        if (!_robot.Target) return; 

        _robot.MoveTarget = _robot.Target.position;
    } 
    
    /// <summary> 
    /// Idle状態にすること 
    /// 索敵中にすること 
    /// </summary> 
    public override void TrackingTarget() 
    {
        // ターゲットがいないなら巡回する
        if(!_robot.MoveTarget.HasValue)
        {
            SetNextDestination();
            return;
        }

        float distance = Vector3.Distance(transform.position, _robot.MoveTarget.Value);

        if(distance <= 2.0f)
        {
            SetNextDestination();
        }
    } 
    
    /// <summary> 
    /// 次の目的地を設定 
    /// </summary> 
    private void SetNextDestination() 
    { 
        // 巡回ポイントがないときは無視
        if (_patrolRoute == null || _patrolRoute.GetPointLength() == 0) return;

        _robot.MoveTarget = _patrolRoute.GetPoint(_currentPoint).position;

        _currentPoint = (_currentPoint + 1) % _patrolRoute.GetPointLength(); 
    } 
}