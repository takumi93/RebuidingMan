using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [SerializeField] private PartsDatabase _partsDatabase; 

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private LayerMask playerLayer;

    RobotStateManager _stateManager { get; set; }

    // 頭のスクリプトの格納場所を指定
    public HeadBase head {  get; private set; }
    // 胴のスクリプトの格納場所を指定
    public BodyBase body { get; private set; }
    // 足のスクリプトの格納場所を指定
    public LegBase leg {  get; private set; }

    // 追尾対象
    public GameObject Target { get; set; } = null;

    LayerMask _searchCategory {  get; set; }

    public float attackDistance { get; private set; }


    private void Start()
    {
        // 体の部位ごとに必要な情報を取得
        head = GetComponentInChildren<HeadBase>(true);
        body = GetComponentInChildren<BodyBase>(true);
        leg = GetComponentInChildren<LegBase>(true);

        head.HeadData = _partsDatabase.GetPartById(head.GetComponent<PartsPickup>().GetPartID()) as HeadData;
        body.BodyData = _partsDatabase.GetPartById(body.GetComponent<PartsPickup>().GetPartID()) as BodyData;
        leg.LegData = _partsDatabase.GetPartById(leg.GetComponent<PartsPickup>().GetPartID()) as LegData;

        head.Init();
        body.Init();
        leg.Init();

        // 敵か味方か識別
        if (1 << gameObject.layer == (int)playerLayer)
        {
            _searchCategory = enemyLayer;
            head.CreateSetup();
            body.CreateSetup();
            leg.CreateSetup();
        }
        else
        {
            _searchCategory = playerLayer;
        }

        attackDistance = body.BodyData.AttackRange;

        _stateManager = new RobotStateManager(this);
    }

    /// <summary>
    /// RobotControllerに送るUpdateの処理
    /// </summary>
    /// <param name="inputInfo"></param>
    public void Tick()
    {
        _stateManager.CurrentState.Tick(this);
    }

    /// <summary>
    /// ラッパー関数として登録
    /// プレイヤーの状態遷移をする処理
    /// </summary>
    /// <param name="newstate"></param>
    public void ChangeState(RobotStateBase newstate)
    {
        _stateManager.ChangeState(newstate);
    }

    /// <summary>
    /// IdleStateの時に実行する
    /// </summary>
    /// <param name="toChase"></param>
    /// <returns></returns>
    public bool HandleIdle()
    {
        head.TrackingTarget();
        if(head.SearchTarget(_searchCategory))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// WalkStateの時に実行する
    /// </summary>
    /// <param name="toChase"></param>
    /// <returns></returns>
    public bool HandleChase(out bool toAttack)
    {
        toAttack = false;

        // ターゲットが見えている場合
        if (head.SearchTarget(_searchCategory))
        {
            head.ChaseTarget();

            // 攻撃距離判定
            if (Target != null)
            {
                float distance = Vector3.Distance(head.transform.position, Target.transform.position);
                if (distance <= attackDistance) // attackDistance は Inspector か定数で指定
                {
                    toAttack = true;
                    return false; // Chase 終了
                }
            }

            return true; // Chase 継続
        }

        // 見失った場合の猶予チェック
        if (head.CheckLose())
        {
            head.ChaseTarget();
            return true; // Chase 継続
        }

        // 完全に見失った
        return false;
    }

    /// <summary>
    /// AttackStateの時に実行する
    /// </summary>
    /// <param name="toChase"></param>
    /// <returns></returns>
    public bool HandleAttack(out bool toChase)
    {
        toChase = false;

        if (Target == null)
        {
            // ターゲット消失 → Chase または Idle へ
            toChase = true;
            return false;
        }

        // 追尾しながら攻撃
        head.ChaseTarget();

        float targetDistance = Vector3.Distance(transform.position, Target.transform.position);

        // 攻撃範囲外になったら Chase に戻す
        if (targetDistance > attackDistance)
        {
            toChase = true;
            return false; // Attack 終了
        }

        return true; // Attack 継続
    }
}
