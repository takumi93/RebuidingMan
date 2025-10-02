using System.Collections;
using UnityEngine;

public interface BodyController
{
    void Attack();

    int GetDamageA();

    int GetDamageB();

    BodyData OutputData();

    void PartsDestroy(bool isSet);

    void CreateSetup();
}

//public class BodyController : MonoBehaviour
//{
//    [Header("頭に連動するRigを指定")]
//    [SerializeField] public GameObject BodyToHeadRig = null;
//    [Header("足に連動するRigを指定")]
//    [SerializeField] public GameObject BodyToLegRig = null;
    
//    public GameObject LegToBodyRig = null;

//    // 体の情報
//    public BodyData bodyDatas;

//    const int NormalId = 0;
//    const int GunId = 1;
//    const int AxeId = 2;

//    [SerializeField] private Collider robotAttackRangeA;
//    [SerializeField] private Collider robotAttackRangeB;

//    [Header("gun時のパラメータ")]
    
//    [SerializeField] GameObject Allybullet;
//    [SerializeField] GameObject Enemybullet;

//    [SerializeField] Transform shotPointRight;
//    [SerializeField] Transform shotPointLeft;

//    //SE関連
//    [Header("SEを指定します。")]
//    [SerializeField]
//    public AudioSource audioSource;

//    float lapseTime;

//    public bool isAttackable;

//    //味方かてきかtrueならプレイヤーと味方を攻撃する。
//    public bool isEnemy = false;

//    PlayerHP playerHp = null;

//    public RobotAction robotAction;

//    private Animator animator;

//    public enum WeaponState
//    {
//        normal,

//        gun,

//        axe,
//    }

//    public WeaponState weaponState = WeaponState.normal;

//    private void Start()
//    {
//        //ステートを更新。
//        if (bodyDatas.id == NormalId)
//        {
//            weaponState = WeaponState.normal;
//        }
//        else if (bodyDatas.id == AxeId)
//        {
//            weaponState = WeaponState.axe;
//        }
//        else
//        {
//            weaponState = WeaponState.gun;
//        }

//        animator = transform.GetChild(0).GetComponent<Animator>();

//        isAttackable = true;

//        lapseTime = 0.0f;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (robotAction.IsSet)
//        {
//            this.GetComponent<Rigidbody>().useGravity = false;
//            this.GetComponent<Rigidbody>().isKinematic = true;
            

//            if (LegToBodyRig)
//            {
//                BodyToLegRig.transform.position = LegToBodyRig.transform.position;
//            }

//            animator.SetBool("Walk", true);
//        }
//        else
//        {
//            this.GetComponent<Rigidbody>().useGravity = true;
//            this.GetComponent<Rigidbody>().isKinematic = false;
//        }

//        // 攻撃のクールタイム
//        // isAttackableがfalseなら、直前のフレームからの経過時間を足す
//        if (!isAttackable)
//        {
//            lapseTime += Time.deltaTime;

//            //lapsetimeがクールタイムを越えたら、isAttackableをtrueに戻して
//            //次に備えて、lapseTimeを0で初期化
//            if (lapseTime >= bodyDatas.coolTimeA && lapseTime >= bodyDatas.coolTimeB)
//            {
//                isAttackable = true;
//                lapseTime = 0.0f;
//            }
//        }
//    }

//    // ===============================================================================
//    // 変数名: Attack
//    // 攻撃をする（体の種類によって変化）
//    // ===============================================================================
//    public void Attack()
//    {
//        //攻撃中か、クールタイム中なら返す。
//        if (!isAttackable)
//        {
//            return;
//        }
//        //違うなら攻撃中をtrueにして処理を実行
//        else
//        {

//            int rand = UnityEngine.Random.Range(1, 11);

//            switch (weaponState)
//            {
//                case WeaponState.normal:
//                    //確率で攻撃が変わる。
//                    if (rand >= 4)
//                    {
//                        animator.SetTrigger("Punch");
//                        StartCoroutine(Attack(bodyDatas.preparationTimeA, bodyDatas.occurrenceTimeA, bodyDatas.finishTimeA, 0));
//                    }
//                    if (rand <= 3)
//                    {
//                        animator.SetTrigger("Rariat");
//                        StartCoroutine(Attack(bodyDatas.preparationTimeB, bodyDatas.occurrenceTimeB, bodyDatas.finishTimeB, 1));
//                    }
//                    break;
//                case WeaponState.gun:
//                    if (rand >= 4)
//                    {
//                        animator.SetTrigger("Gun");
//                        StartCoroutine(GunAttack(bodyDatas.preparationTimeA, 0));
//                        robotAction.robotState = RobotAction.RobotState.Chase;
//                    }
//                    if (rand <= 3)
//                    {
//                        animator.SetTrigger("Missile");
//                        StartCoroutine(GunAttack(bodyDatas.preparationTimeB, 1));
//                        robotAction.robotState = RobotAction.RobotState.Chase;
//                    }
//                    break;
//                case WeaponState.axe:
//                    //確率で攻撃が変わる。
//                    if (rand >= 4)
//                    {
//                        animator.SetTrigger("Slash");
//                        StartCoroutine(Attack(bodyDatas.preparationTimeA, bodyDatas.occurrenceTimeA, bodyDatas.finishTimeA, 0));
//                    }
//                    if (rand <= 3)
//                    {
//                        animator.SetTrigger("Charge");
//                        StartCoroutine(Attack(bodyDatas.preparationTimeB, bodyDatas.occurrenceTimeB, bodyDatas.finishTimeB, 1));
//                    }

//                    break;
//            }

//            isAttackable = false;
//        }
//    }

//    IEnumerator Attack(float preparationTime,float occurrenceTime, float finishTime, int AttackType)
//    {
//        if (AttackType == 0)
//        {
//            //準備時間まで待つ
//            yield return new WaitForSeconds(preparationTime);
//            //robotAttackPunch.OnCollider();
//            robotAttackRangeA.enabled = true;
//            if (audioSource)
//            {
//                audioSource.PlayOneShot(bodyDatas.attackSoundA);
//            }
            
//            yield return new WaitForSeconds(occurrenceTime);
//            robotAttackRangeA.enabled = false;
//            //robotAttackPunch.OffCollider();
//            yield return new WaitForSeconds(finishTime);
//        }
//        else if (AttackType == 1)
//        {
//            //準備時間まで待つ
//            yield return new WaitForSeconds(preparationTime);
//            //robotAttackRariat.OnCollider();
//            robotAttackRangeB.enabled = true;
//            if (audioSource)
//            {
//                audioSource.PlayOneShot(bodyDatas.attackSoundB);
//            }
            
//            yield return new WaitForSeconds(occurrenceTime);
//            //robotAttackRariat.OffCollider();
//            robotAttackRangeB.enabled = false;
//            yield return new WaitForSeconds(finishTime);
//        }
//        robotAction.robotState = RobotAction.RobotState.Idle;
//    }

//    IEnumerator GunAttack(float preparationTime, int AttackType)
//    {
//        if (AttackType == 0)
//        {
//            //準備時間まで待つ
//            yield return new WaitForSeconds(preparationTime);
//            //距離とtransformを宣言
//            float minDis = Mathf.Infinity;
//            Transform minPos = null;
//            if (playerHp != null)
//            {
//                //プレイヤーがいるならプレイヤーを初期値にする。
//                minDis = Vector3.Distance(playerHp.transform.position, transform.position);
//                minPos = playerHp.transform;
//            }
//            if (this.transform.parent.CompareTag("Enemy")){
//                if (robotAction.Target)
//                {
//                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
//                    shotPointRight.LookAt(robotAction.Target.transform.position);
//                    shotPointLeft.LookAt(robotAction.Target.transform.position);
//                    Instantiate(Enemybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
//                    Instantiate(Enemybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);

//                    if (audioSource)
//                    {
//                        audioSource.PlayOneShot(bodyDatas.attackSoundA);
//                    }
//                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
//                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
//                }
//            }
//            else
//            {
//                if (robotAction.Target)
//                {
//                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
//                    shotPointRight.LookAt(robotAction.Target.transform.position);
//                    shotPointLeft.LookAt(robotAction.Target.transform.position);
//                    Instantiate(Allybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
//                    Instantiate(Allybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
//                    if (audioSource)
//                    {
//                        audioSource.PlayOneShot(bodyDatas.attackSoundB);
//                    }
//                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
//                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
//                }
//            }

//        }
//        else
//        {
//            //準備時間まで待つ
//            yield return new WaitForSeconds(preparationTime);
//            //距離とtransformを宣言
//            float minDis = Mathf.Infinity;
//            Transform minPos = null;
//            if (playerHp != null)
//            {
//                //プレイヤーがいるならプレイヤーを初期値にする。
//                minDis = Vector3.Distance(playerHp.transform.position, transform.position);
//                minPos = playerHp.transform;
//            }
//            if (this.transform.parent.CompareTag("Enemy")){
//                if (robotAction.Target)
//                {
//                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
//                    shotPointRight.LookAt(robotAction.Target.transform.position);
//                    shotPointLeft.LookAt(robotAction.Target.transform.position);
//                    Instantiate(Enemybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
//                    Instantiate(Enemybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
//                    if (audioSource)
//                    {
//                        audioSource.PlayOneShot(bodyDatas.attackSoundA);
//                    }
                    
//                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
//                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
//                }
//            }
//            else
//            {
//                if (robotAction.Target)
//                {
//                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
//                    shotPointRight.LookAt(robotAction.Target.transform.position);
//                    shotPointLeft.LookAt(robotAction.Target.transform.position);
//                    Instantiate(Allybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
//                    Instantiate(Allybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
//                    if (audioSource)
//                    {
//                        audioSource.PlayOneShot(bodyDatas.attackSoundB);
//                    }
                    
//                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
//                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
//                }
//            }
//        }
//    }


//}
