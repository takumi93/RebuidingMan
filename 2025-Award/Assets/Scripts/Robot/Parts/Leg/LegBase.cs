using UnityEngine;
using UnityEngine.AI;

public abstract class LegBase : MonoBehaviour
{
    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject LegToBodyRig = null;

    // 足の情報
    public LegData LegData {  get; set; }

    public RobotAnimation Animation { get; set; }

    public Rigidbody rb {  get; set; }

    public abstract void Init();

    ///// <summary>
    ///// ロボットができたときに行う初期設定
    ///// </summary>
    ///// <param name="rig"></param>
    ///// <param name="agent"></param>
    //public abstract void SetupRig(GameObject rig);

    /// <summary>
    /// ロボットがプレイヤーによって作成されたときの初期設定
    /// </summary>
    public abstract void CreateSetup();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual LegData OutputData()
    {
        return LegData;
    }
}
