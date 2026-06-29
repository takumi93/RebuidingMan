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

    public override void Init(Robot robot)
    {
        base.Init(robot);

        Animation = GetComponentInChildren<RobotAnimation>();
        Animation.Init();

        _agent = robot.GetComponent<NavMeshAgent>();
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
}
