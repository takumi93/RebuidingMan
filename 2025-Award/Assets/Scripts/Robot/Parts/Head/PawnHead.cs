using UnityEngine;
public class PawnHead : HeadBase 
{ 
    [Header("巡回ルート")]
    [SerializeField] private PatrolRoute _patrolRoute; 
    private int _currentPoint = 0; 
    
    public override void Init() 
    { 
        base.Init();
        IsPatrolling = true;
        SetNextDestination(); 
    } 
    
    /// <summary>
    /// 味方になった時のセットアップ 
    /// </summary> 
    public override void CreateSetup() 
    {
        //// 味方用にマテリアルを変更
        //SkinnedMeshRenderer renderer = transform.GetComponentInChildren<SkinnedMeshRenderer>(); 
        //Material[] mats = renderer.materials; 
        //for (int i = 0; i < mats.Length; i++) 
        //{ 
        //    mats[i] = HeadData.AllyMaterial;
        //} 
        //renderer.materials = mats; 
        UpdateMaterial();
        _patrolRoute = StageScene.Instance.AllyRoute;
        _currentPoint = 0; 
        SetNextDestination(); 
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

        if(distance <= _area.stoppingDistance)
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