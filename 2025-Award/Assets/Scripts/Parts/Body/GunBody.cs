using System.Collections;
using UnityEngine;

public class GunBody : MonoBehaviour, BodyController
{
    // 体の情報
    [SerializeField] public BodyData bodyData;

    [Header("頭に連動するRigを指定")]
    [SerializeField] public GameObject BodyToHeadRig = null;
    [Header("足に連動するRigを指定")]
    [SerializeField] public GameObject BodyToLegRig = null;

    public GameObject LegToBodyRig = null;

    [Header("gun時のパラメータ")]

    [SerializeField] GameObject Allybullet;
    [SerializeField] GameObject Enemybullet;

    [SerializeField] Transform shotPointRight;
    [SerializeField] Transform shotPointLeft;

    //SE関連
    [Header("SEを指定します。")]
    [SerializeField]
    public AudioSource audioSource;

    float lapseTime;

    public bool isAttackable;

    //味方かてきかtrueならプレイヤーと味方を攻撃する。
    public bool isEnemy = false;

    PlayerHP playerHp = null;

    public RobotAction robotAction;

    private Animator animator;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        isAttackable = true;

        lapseTime = 0.0f;

        robotAction = this.GetComponentInParent<RobotAction>();
        if (robotAction == null)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (robotAction.IsSet)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().isKinematic = true;


            if (LegToBodyRig)
            {
                BodyToLegRig.transform.position = LegToBodyRig.transform.position;
            }

            animator.SetBool("Walk", true);
        }
        else
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }

        // 攻撃のクールタイム
        // isAttackableがfalseなら、直前のフレームからの経過時間を足す
        if (!isAttackable)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeがクールタイムを越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= bodyData.coolTimeA && lapseTime >= bodyData.coolTimeB)
            {
                isAttackable = true;
                lapseTime = 0.0f;
            }
        }
    }

    public void CreateSetup()
    {
        this.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = bodyData.material;
        audioSource = this.GetComponentInParent<AudioSource>();
    }

    public void PartsDestroy(bool isSet)
    {
        if (isSet)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    public int GetDamageA()
    {
        return bodyData.damageA;
    }

    public int GetDamageB()
    {
        return bodyData.damageB;
    }

    public BodyData OutputData()
    {
        return bodyData;
    }

    public void Attack()
    {
        //攻撃中か、クールタイム中なら返す。
        if (!isAttackable)
        {
            return;
        }
        //違うなら攻撃中をtrueにして処理を実行
        else
        {
            int rand = UnityEngine.Random.Range(1, 11);

            if (rand >= 4)
            {
                animator.SetTrigger("Gun");
                StartCoroutine(GunAttack(bodyData.preparationTimeA, 0));
                robotAction.robotState = RobotAction.RobotState.Chase;
            }
            if (rand <= 3)
            {
                animator.SetTrigger("Missile");
                StartCoroutine(GunAttack(bodyData.preparationTimeB, 1));
                robotAction.robotState = RobotAction.RobotState.Chase;
            }

            isAttackable = false;
        }
    }

    IEnumerator GunAttack(float preparationTime, int AttackType)
    {
        //準備時間まで待つ
        yield return new WaitForSeconds(preparationTime);
        //距離とtransformを宣言
        float minDis = Mathf.Infinity;
        Transform minPos = null;
        if (AttackType == 0)
        {

            if (playerHp != null)
            {
                //プレイヤーがいるならプレイヤーを初期値にする。
                minDis = Vector3.Distance(playerHp.transform.position, transform.position);
                minPos = playerHp.transform;
            }
            if (this.transform.parent.CompareTag("Enemy"))
            {
                if (robotAction.Target)
                {
                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
                    shotPointRight.LookAt(robotAction.Target.transform.position);
                    shotPointLeft.LookAt(robotAction.Target.transform.position);
                    Instantiate(Enemybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
                    Instantiate(Enemybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);

                    if (audioSource)
                    {
                        audioSource.PlayOneShot(bodyData.attackSoundA);
                    }
                }
            }
            else
            {
                if (robotAction.Target)
                {
                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
                    shotPointRight.LookAt(robotAction.Target.transform.position);
                    shotPointLeft.LookAt(robotAction.Target.transform.position);
                    Instantiate(Allybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
                    Instantiate(Allybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
                    if (audioSource)
                    {
                        audioSource.PlayOneShot(bodyData.attackSoundB);
                    }
                }
            }

        }
        else
        {
            if (playerHp != null)
            {
                //プレイヤーがいるならプレイヤーを初期値にする。
                minDis = Vector3.Distance(playerHp.transform.position, transform.position);
                minPos = playerHp.transform;
            }
            if (this.transform.parent.CompareTag("Enemy"))
            {
                if (robotAction.Target)
                {
                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
                    shotPointRight.LookAt(robotAction.Target.transform.position);
                    shotPointLeft.LookAt(robotAction.Target.transform.position);
                    Instantiate(Enemybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
                    Instantiate(Enemybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
                    if (audioSource)
                    {
                        audioSource.PlayOneShot(bodyData.attackSoundA);
                    }
                }
            }
            else
            {
                if (robotAction.Target)
                {
                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
                    shotPointRight.LookAt(robotAction.Target.transform.position);
                    shotPointLeft.LookAt(robotAction.Target.transform.position);
                    Instantiate(Allybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
                    Instantiate(Allybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
                    if (audioSource)
                    {
                        audioSource.PlayOneShot(bodyData.attackSoundB);
                    }
                }
            }
        }
    }
}
