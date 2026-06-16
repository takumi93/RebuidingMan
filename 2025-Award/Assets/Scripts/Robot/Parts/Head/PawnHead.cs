using UnityEngine;
public class PawnHead : HeadBase 
{ 
    [Header("巡回ルート")]
    [SerializeField] private PatrolRoute _patrolRoute; 
    private int _currentPoint = 0; 
    
    // キャタピラ時の角度
    const float moveStartAngle = 30.0f; 
    const float rotateSpeed = 120f; 
    public override void Init() 
    { 
        base.Init(); 
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
        _patrolRoute = StageScene.Instance.AllyRoute;
        _currentPoint = 0; 
        SetNextDestination(); 
    } 
    
    /// <summary> 
    /// ロボットが敵を追尾する 
    /// </summary> 
    public override void ChaseTarget() 
    { 
        if (!_robot.MoveTarget.HasValue) return; 
        Vector3 targetPos = _robot.MoveTarget.Value; 
        _area.isStopped = false; 
        Vector3 targetDir = targetPos - transform.position; 
        targetDir.y = 0f; 
        
        // 上下方向は無視して水平だけで追尾 
        // 旋回
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up); 
        Transform body = transform.parent; 
        // 回転させたい本体
        body.rotation = Quaternion.RotateTowards( body.rotation, targetRotation, rotateSpeed * Time.deltaTime ); 
        // 自分の向きとターゲット方向の角度差
        float angle = Vector3.Angle(body.forward, targetDir); 
        if (angle < moveStartAngle) 
        { 
            _area.isStopped = false; 
            _area.destination = _robot.MoveTarget.Value; 
        } 
        else 
        { 
            _area.isStopped = true; 
            // 角度差大きい場合はその場で旋回
        } 
    } 
    
    /// <summary> 
    /// Idle状態にすること 
    /// 索敵中にすること 
    /// </summary> 
    public override void TrackingTarget() 
    { 
        // 経路探索の準備ができているか
        if (_area.pathPending) return; 
        if (_area.remainingDistance <= _area.stoppingDistance) 
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

        _area.isStopped = false; 
        _area.SetDestination(_patrolRoute.GetPoint(_currentPoint).position); 
        _currentPoint = (_currentPoint + 1) % _patrolRoute.GetPointLength(); 
    } 
}