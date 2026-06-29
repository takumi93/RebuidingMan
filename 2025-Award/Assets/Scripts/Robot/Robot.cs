using UnityEngine;

public class Robot : MonoBehaviour
{
    // ステートマシン
    public RobotStateManager StateManager { get; private set; }

    [SerializeField] private PartsDatabase partsDatabase;

    // 頭のスクリプトの格納場所を指定
    public HeadBase Head {  get; private set; }
    // 胴のスクリプトの格納場所を指定
    public BodyBase Body { get; private set; }
    // 足のスクリプトの格納場所を指定
    public LegBase Leg {  get; private set; }

    // 追尾対象
    public Transform Target { get; set; }

    // 最後に攻撃してきた敵
    public Robot LastAttacker { get; set; }

    // 移動先
    public Vector3? MoveTarget { get; set; }

    // 指定された移動先に止まる距離
    public float MoveStoppingDistance { get; set; }

    // 攻撃モーションに入る距離
    public float attackDistance { get; private set; }

    // 陣営
    private TeamObject _teamObject;

    public TeamType TeamType => _teamObject.GetTeamType();


    private void Start()
    {
        _teamObject = GetComponent<TeamObject>();

        // 体の部位ごとに必要な情報を取得
        Head = GetComponentInChildren<HeadBase>(true);
        Body = GetComponentInChildren<BodyBase>(true);
        Leg = GetComponentInChildren<LegBase>(true);

        Head.SetData((HeadData)partsDatabase.GetPartById(Head.Id));
        Body.SetData((BodyData)partsDatabase.GetPartById(Body.Id));
        Leg.SetData((LegData)partsDatabase.GetPartById(Leg.Id));

        Head.Init(this);
        Body.Init(this);
        Leg.Init(this);

        // 敵か味方か識別
        var team = GetComponent<TeamObject>();

        if (team != null && team.GetTeamType() == TeamType.Player)
        {
            Head.CreateSetup();
            Body.CreateSetup();
            Leg.CreateSetup();
        }

        RobotManager.Instance.Register(this);

        attackDistance = Body.BodyData.AttackRange;

        MoveStoppingDistance = Body.BodyData.StoppingDistance;

        StateManager = new RobotStateManager(this);
    }

    /// <summary>
    /// RobotControllerに送るUpdateの処理
    /// </summary>
    /// <param name="inputInfo"></param>
    public void Tick()
    {
        Body.UpdateCoolTime();

        StateManager.CurrentState.Tick(this);
    }

    /// <summary>
    /// ラッパー関数として登録
    /// プレイヤーの状態遷移をする処理
    /// </summary>
    /// <param name="newstate"></param>
    public void ChangeState(RobotStateBase newstate)
    {
        StateManager.ChangeState(newstate);
    }

    /// <summary>
    /// IdleStateの時に実行する
    /// </summary>
    /// <param name="toChase"></param>
    /// <returns></returns>
    public bool HandleIdle()
    {
        Head.TrackingTarget();

        if (MoveTarget.HasValue)
        {
            Leg.Move(MoveTarget.Value);
        }

        return Head.SearchTarget();
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
        if (Head.SearchTarget())
        {
            // 移動先設定
            Head.ChaseTarget();

            if (MoveTarget.HasValue)
            {
                Leg.Move(MoveTarget.Value);
            }

            // 攻撃距離判定
            if (Target != null)
            {
                float distance = Vector3.Distance(transform.position, Target.transform.position);

                if (distance <= attackDistance)
                {
                    toAttack = true;

                    return false; // Chase 終了
                }
            }

            return true; // Chase 継続
        }

        // 見失った場合の猶予チェック
        if (Head.CheckLose())
        {
            if (MoveTarget.HasValue)
            {
                Leg.Move(MoveTarget.Value);
            }

            return true; // Chase 継続
        }

        // 完全に見失った
        return false;
    }

    /// <summary>
    /// AttackStateの時に実行する
    /// </summary>
    /// <param name="toChase"></param>
    /// <returns>攻撃最中はTrue、ターゲットがいないまたは攻撃終了時はFalse</returns>
    public bool HandleAttack()
    {
        // ターゲットがいないならFalse
        if (!Target) return false;

        // 追尾しながら攻撃
        Head.ChaseTarget();

        if (MoveTarget.HasValue)
        {
            Leg.Move(MoveTarget.Value);
        }

        // 攻撃が終了しているならFalse、攻撃最中ならTrue
        return Body.IsAttacking;
    }
}
