using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotCreate : MonoBehaviour
{
    // HPの管理しているスクリプトを指定
    [SerializeField]
    RobotData robotData;
    // ロボットの作成地点を指定
    [SerializeField]
    Transform plantPoint;
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
    // ロボットを配置するオブジェクト
    public GameObject robotAlly = null;

    // 巡回ポジション
    [SerializeField] public Transform[] AllycrawlPositions = null;
    [SerializeField] GameObject FixPoint = null;

    [SerializeField] Material NormalMaterial;
    [SerializeField] Material GunMaterial;
    [SerializeField] Material KnightMaterial;

    [SerializeField] public GameObject clone = null;

    AudioSource audioSource;

    public void Start()
    {
        IsCreate = false;
        IsHeadSet = false;
        IsBodySet = false;
        IsLegSet = false;
    }

    public void Update()
    {
        if (IsHeadSet & IsBodySet & IsLegSet)
        {
            IsCreate = true;
        }
    }

    public void createObject()
    {
        if (IsCreate)
        {
            var parent = Instantiate(clone, robotAlly.transform);
            parent.transform.position = plantPoint.position;
            parent.transform.rotation = plantPoint.rotation;
            var parentPosition = parent.transform;
            //SetPosition(parentPosition, robotAlly.transform, plantPoint);
            var Action = parent.GetComponent<RobotAction>();
            var navmesh = parent.GetComponent<NavMeshAgent>();

            SetGravity(player.grabHead);
            SetGravity(player.grabBody);
            SetGravity(player.grabLeg);

            SetPosition(player.grabHead, parentPosition, plantPoint);
            SetPosition(player.grabBody, parentPosition, plantPoint);
            SetPosition(player.grabLeg, parentPosition, plantPoint);

            if (player.grabHead.CompareTag("Pawn"))
            {
                player.grabHead.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = NormalMaterial;
                player.grabHead.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = NormalMaterial;
                for (int i = 0; AllycrawlPositions.Length < i; i++)
                {

                    Action.crawlPositions[i] = AllycrawlPositions[i];
                }
                Action.crawlPositions = AllycrawlPositions;
            }
            else if (player.grabHead.CompareTag("Rook"))
            {
                player.grabHead.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().material = GunMaterial;
                var Fix = Instantiate(FixPoint, parentPosition.parent);
                Fix.transform.position = plantPoint.transform.position;
                Action.FixedPosition = Fix.transform;
            }
            else if (player.grabHead.CompareTag("Knight"))
            {
                player.grabHead.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().material = KnightMaterial;
            }
            if (player.grabBody.CompareTag("Normal"))
            {
                player.grabBody.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material = NormalMaterial;
                player.grabBody.GetComponent<RobotAttackMethod>().enabled = true;
                player.grabBody.GetComponent<RobotAttackMethod>().audioSource = parent.GetComponent<AudioSource>();
                player.grabBody.GetChild(0).GetComponent<Animator>().enabled = true;
            }
            else if (player.grabBody.CompareTag("Gun"))
            {
                player.grabBody.GetChild(0).GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().material = GunMaterial;
                player.grabBody.GetComponent<RobotAttackMethod>().enabled = true;
                player.grabBody.GetComponent<RobotAttackMethod>().audioSource = parent.GetComponent<AudioSource>();
                player.grabBody.GetChild(0).GetComponent<Animator>().enabled = true;
            }
            else if (player.grabBody.CompareTag("Axe"))
            {
                player.grabBody.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().material = NormalMaterial;
                player.grabBody.GetComponent<RobotAttackMethod>().enabled = true;
                player.grabBody.GetComponent<RobotAttackMethod>().audioSource = parent.GetComponent<AudioSource>();
                player.grabBody.GetChild(0).GetComponent<Animator>().enabled = true;
            }
            if (player.grabLeg.CompareTag("Tire"))
            {
                player.grabLeg.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GunMaterial;
                player.grabLeg.GetComponent<Animator>().enabled = true;
            }
            else if(player.grabLeg.CompareTag("Walk"))
            {
                player.grabLeg.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = NormalMaterial;
                player.grabLeg.GetComponent<Animator>().enabled = true;
            }

            player.grabHead = null;
            player.grabBody = null;
            player.grabLeg = null;

            IsCreate = false;
            IsHeadSet = false;
            IsBodySet = false;
            IsLegSet = false;

            navmesh.enabled = true;
        }
    }

    public void SetPosition(Transform child, Transform parent, Transform point)
    {
        child.parent = parent;
        child.position = point.position;
        child.rotation = point.rotation;
    }

    public void SetGravity(Transform part)
    {
        part.GetComponent<Rigidbody>().useGravity = false;
        part.GetComponent<Rigidbody>().isKinematic = true;
    }
}
