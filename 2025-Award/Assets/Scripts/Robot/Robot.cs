using UnityEngine;

public class Robot : MonoBehaviour
{
    // ステートマシン
    public RobotStateManager StateManager { get; private set; }

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

        Head.Init();
        Body.Init();
        Leg.Init();

        // 敵か味方か識別
        var team = GetComponent<TeamObject>();

        if (team != null && team.GetTeamType() == TeamType.Player)
        {
            Head.CreateSetup();
            Body.CreateSetup();
            Leg.CreateSetup();
        }

        attackDistance = Body.BodyData.AttackRange;

        StateManager = new RobotStateManager(this);
    }

    /// <summary>
    /// RobotControllerに送るUpdateの処理
    /// </summary>
    /// <param name="inputInfo"></param>
    public void Tick()
    {
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
        if(Head.SearchTarget())
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
        if (Head.SearchTarget())
        {
            Head.ChaseTarget();

            // 攻撃距離判定
            if (Target != null)
            {
                float distance = Vector3.Distance(Head.transform.position, Target.transform.position);
                if (distance <= attackDistance) // attackDistance は Inspector か定数で指定
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
            Head.ChaseTarget();
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
        Head.ChaseTarget();

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
