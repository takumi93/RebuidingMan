using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotCreate : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private GameObject robotBasePrefab;    // ベースとなるロボット本体（空の骨組みPrefab）
    [SerializeField] private Transform spawnPoint;          // 出現位置
    [SerializeField]
    Transform plantPoint;
    [SerializeField] public Transform robotAlly;           // ロボットを配置するオブジェクト

    // パーツを管理しているデータベースを指定
    [SerializeField] private PartsDatabase database;
    private HeadData headData;
    private BodyData bodyData;
    private LegData LegData;
    [SerializeField] private Transform headSlot;
    [SerializeField] private Transform bodySlot;
    [SerializeField] private Transform legSlot;

    //// HPの管理しているスクリプトを指定
    //[SerializeField]
    //RobotData robotData;
    // ロボットの作成地点を指定
    // 武器を指定
    [SerializeField]
    GameObject attack;
    // ロボットが生成可能か
    public bool IsCreate;
    // 頭がセットされているか
    public bool IsHeadSet;
    // 胴がセットされているか
    public bool IsBodySet;
    // 足がセットされているか
    public bool IsLegSet;
    // セットされた頭オブジェクトを格納
    GameObject SetHeadOb;
    // セットされた胴オブジェクトを格納
    GameObject SetBodyOb;
    // セットされた足オブジェクトを格納
    GameObject SetLegOb;
    [SerializeField]
    Player player;
    // 頭のレイヤー
    const int LayerNumberHead = 8;
    // 胴のレイヤー
    const int LayerNumberBody = 9;
    // 足のレイヤー
    const int LayerNumberLeg = 10;


    [SerializeField] public GameObject clone = null;

    GameObject headObj;
    GameObject bodyObj;
    GameObject legObj;


    AudioSource audioSource;

    public void Start()
    {
        
    }

    public void CreateRobot()
    {
        if (inventory == null)
        {
            return;
        }

        //inventory.DebugEquippedParts();

        bool hasHead = inventory.HasPartOfType(PartsType.Head);
        bool hasBody = inventory.HasPartOfType(PartsType.Body);
        bool hasLeg = inventory.HasPartOfType(PartsType.Leg);

        if (hasHead && hasBody && hasLeg)
        {
            GameObject clone = Instantiate(robotBasePrefab, spawnPoint.position,spawnPoint.rotation, robotAlly);
            clone.name = "Robot";
            var cloneHp = clone.GetComponent<RobotHPManager>();

            PartsData head = inventory.GetPartOfType(PartsType.Head);
            PartsData body = inventory.GetPartOfType(PartsType.Body);
            PartsData leg = inventory.GetPartOfType(PartsType.Leg);

            // 頭
            if (head != null && head.prefab != null)
            {
                headObj = Instantiate(head.prefab, clone.transform);
                headObj.name = head.partName;
            }

            // 胴
            if (body != null && body.prefab != null)
            {
                bodyObj = Instantiate(body.prefab, clone.transform);
                bodyObj.name = body.partName;
            }

            // 足
            if (leg != null && leg.prefab != null)
            {
                legObj = Instantiate(leg.prefab, clone.transform);
                legObj.name = leg.partName;
            }

            // ロボットを作成したのでパーツをリセット
            inventory.ClearParts();
        }
    }
}
