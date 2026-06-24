using UnityEngine;
using UnityEngine.AI;

public abstract class LegBase : PartBase
{
    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject LegToBodyRig = null;

    // 足の情報
    public LegData LegData {  get; protected set; }

    protected NavMeshAgent _agent;

    public RobotAnimation Animation { get; protected set; }

    public Rigidbody rb {  get; protected set; }

    public virtual void Init()
    {
        Animation = GetComponent<RobotAnimation>();
        Animation.Init();

        _agent = GetComponentInParent<NavMeshAgent>();
    }

    ///// <summary>
    ///// ロボットができたときに行う初期設定
    ///// </summary>
    ///// <param name="rig"></param>
    ///// <param name="agent"></param>
    //public abstract void SetupRig(GameObject rig);

    public void SetData(LegData data)
    {
        LegData = data;
    }

    public abstract void Move(Vector3 targetPos);

    public virtual void StopMove()
    {
        _agent.isStopped = true;
    }

    /// <summary>
    /// ロボットがプレイヤーによって作成されたときの初期設定
    /// </summary>
    public abstract void CreateSetup();
}
