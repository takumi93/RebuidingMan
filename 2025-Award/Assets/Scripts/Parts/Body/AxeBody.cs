using System.Collections;
using UnityEngine;

public class AxeBody : MonoBehaviour, BodyController
{
    // 体の情報
    [SerializeField] public BodyData bodyData;

    [Header("頭に連動するRigを指定")]
    [SerializeField] public GameObject BodyToHeadRig = null;
    [Header("足に連動するRigを指定")]
    [SerializeField] public GameObject BodyToLegRig = null;

    public GameObject LegToBodyRig = null;

    [SerializeField] private Collider robotAttackRangeA;
    [SerializeField] private Collider robotAttackRangeB;

    //SE関連
    [Header("SEを指定します。")]
    [SerializeField]
    public AudioSource audioSource;

    float lapseTime;

    public bool isAttackable;

    //味方かてきかtrueならプレイヤーと味方を攻撃する。
    public bool isEnemy = false;

    public RobotAction robotAction;

    private Animator animator;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        isAttackable = true;
        robotAttackRangeA.enabled = false;
        robotAttackRangeB.enabled = false;

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
        this.transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().material = bodyData.material;
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

            //確率で攻撃が変わる。
            if (rand >= 4)
            {
                animator.SetTrigger("Slash");
                StartCoroutine(Attack(bodyData.preparationTimeA, bodyData.occurrenceTimeA, bodyData.finishTimeA, 0));
            }
            if (rand <= 3)
            {
                animator.SetTrigger("Charge");
                StartCoroutine(Attack(bodyData.preparationTimeB, bodyData.occurrenceTimeB, bodyData.finishTimeB, 1));
            }

            isAttackable = false;
        }
    }

    IEnumerator Attack(float preparationTime, float occurrenceTime, float finishTime, int AttackType)
    {
        if (AttackType == 0)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            //robotAttackPunch.OnCollider();
            robotAttackRangeA.enabled = true;
            if (audioSource)
            {
                audioSource.PlayOneShot(bodyData.attackSoundA);
            }

            yield return new WaitForSeconds(occurrenceTime);
            robotAttackRangeA.enabled = false;
            //robotAttackPunch.OffCollider();
            yield return new WaitForSeconds(finishTime);
        }
        else if (AttackType == 1)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            //robotAttackRariat.OnCollider();
            robotAttackRangeB.enabled = true;
            if (audioSource)
            {
                audioSource.PlayOneShot(bodyData.attackSoundB);
            }

            yield return new WaitForSeconds(occurrenceTime);
            //robotAttackRariat.OffCollider();
            robotAttackRangeB.enabled = false;
            yield return new WaitForSeconds(finishTime);
        }
        robotAction.robotState = RobotAction.RobotState.Idle;
    }
}
