using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotAttackMethod : MonoBehaviour
{
    [Header("normal時のパラメータ")]
    // 攻撃のダメージ
    [SerializeField] public int normalDamageA = 20;
    [SerializeField] public int normalDamageB = 20;
    // 攻撃のクールタイム
    [SerializeField] private float normalCoolTimeA = 5.0f;
    // 攻撃のクールタイム
    [SerializeField] private float normalCoolTimeB = 5.0f;
    // 攻撃までの準備時間
    [SerializeField] private float normalPreparationTimeA = 1.0f;
    // 攻撃までの準備時間
    [SerializeField] private float normalPreparationTimeB = 1.0f;
    // 攻撃が発生時間（攻撃の当たり判定を残す時間）
    [SerializeField] private float normalOccurrencetimeA = 0.5f;
    [SerializeField] private float normalOccurrenceTimeB = 5.0f;
    // 攻撃モーションが終わる時間
    [SerializeField] private float normalFinishTimeA = 0.5f;
    [SerializeField] private float normalFinishTimeB = 1.0f;

    [SerializeField] private RobotAttackPunch robotAttackPunch;
    [SerializeField] private RobotAttackRariat robotAttackRariat;

    [Header("gun時のパラメータ")]
    // 攻撃のクールタイム
    [SerializeField] private float gunCoolTimeA = 5.0f;
    // 攻撃までの準備時間
    [SerializeField] private float gunPreparationTimeA = 3.0f;
    // 攻撃のクールタイム
    [SerializeField] private float gunCoolTimeB = 5.0f;
    // 攻撃までの準備時間
    [SerializeField] private float gunPreparationTimeB = 7.0f;
    
    [SerializeField] GameObject Allybullet;
    [SerializeField] GameObject Enemybullet;

    [SerializeField] Transform shotPointRight;
    [SerializeField] Transform shotPointLeft;

    [Header("axe時のパラメータ")]
    // 攻撃のダメージ
    [SerializeField] public int axeDamageA = 30;
    [SerializeField] public int axeDamageB = 40;

    // 攻撃のクールタイム
    [SerializeField] private float axeCoolTimeA = 5.0f;
    // 攻撃のクールタイム
    [SerializeField] private float axeCoolTimeB = 5.0f;
    // 攻撃までの準備時間
    [SerializeField] private float axePreparationTimeA = 2.5f;
    // 攻撃までの準備時間
    [SerializeField] private float axePreparationTimeB = 8.0f;
    // 攻撃が発生時間（攻撃の当たり判定を残す時間）
    [SerializeField] private float axeOccurrencetimeA = 1.0f;
    [SerializeField] private float axeOccurrenceTimeB = 1.0f;
    // 攻撃モーションが終わる時間
    [SerializeField] private float axeFinishTimeA = 1.5f;
    [SerializeField] private float axeFinishTimeB = 3.0f;
    [SerializeField] private RobotAttackSlashAxe robotAttackSlashAxe;
    [SerializeField] private RobotAttackChargeAxe robotAttackChargeAxe;

    //SE関連
    [Header("SEを指定します。")]
    [SerializeField]
    public AudioSource audioSource;
    [SerializeField]
    AudioClip punchSound;
    [SerializeField]
    AudioClip spinPunchSound;
    [SerializeField]
    AudioClip shotSound;
    [SerializeField]
    AudioClip missileSound;


    const int LayerNumberAlly = 6;
    const int LayerNumberEnemy = 7;

    const int LayerNumberAllyShot = 11;
    const int LayerNumberEnemyShot = 12;

    float coolTimeA;
    float coolTimeB;
    float lapseTime;

    public bool isAttackable;

    ////攻撃クールタイムの計測用。0になったら攻撃できる
    //float time = 0.0f;
    ////対象が攻撃範囲内にいる場合true
    //bool attackAble = false;

    //味方かてきかtrueならプレイヤーと味方を攻撃する。
    public bool isEnemy = false;

    PlayerHP playerHp = null;
    List<RobotHPManager> robotHpList = new();

    private RobotAction robotAction;

    private Animator animator;

    public enum WeaponState
    {
        normal,

        gun,

        axe,
    }

    public WeaponState weaponState = WeaponState.normal;

    private void Start()
    {
        //ステートを更新。
        if (transform.CompareTag("Normal"))
        {
            weaponState = WeaponState.normal;
            coolTimeA = normalCoolTimeA;
            coolTimeB = normalCoolTimeB;
        }
        if (transform.CompareTag("Gun"))
        {
            weaponState = WeaponState.gun;
            coolTimeA = gunCoolTimeA;
            coolTimeB = gunCoolTimeB;
        }
        if (transform.CompareTag("Axe"))
        {
            weaponState = WeaponState.axe;
            coolTimeA = axeCoolTimeA;
            coolTimeB = axeCoolTimeB;

        }

        //robotAction = transform.parent.parent.GetComponent<RobotAction>();
        robotAction = transform.parent.GetComponent<RobotAction>();
        //animator = transform.parent.transform.GetChild(0).GetComponent<Animator>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        isAttackable = true;

        lapseTime = 0.0f;
    }

    // スクリプトが有効化されたとき呼び出す
    private void OnEnable()
    {
        //このロボットが敵か味方かを判定
        if (transform.parent.gameObject.layer == LayerNumberEnemy)
        {
            isEnemy = true;
            if (this.gameObject.tag == "Axe")
            {
                this.transform.GetChild(1).gameObject.layer = LayerNumberEnemyShot;
            }else if (this.gameObject.tag == "Normal")
            {
                this.transform.GetChild(1).gameObject.layer = LayerNumberEnemyShot;
                this.transform.GetChild(2).gameObject.layer = LayerNumberEnemyShot;
            }
        }
        else
        {
            isEnemy = false;
            if (this.gameObject.tag == "Axe")
            {
                this.transform.GetChild(1).gameObject.layer = LayerNumberAllyShot;
                robotAttackChargeAxe.OnCollider();
                robotAttackSlashAxe.OnCollider();
            }
            else if (this.gameObject.tag == "Normal")
            {
                this.transform.GetChild(1).gameObject.layer = LayerNumberAllyShot;
                this.transform.GetChild(2).gameObject.layer = LayerNumberAllyShot;
                robotAttackPunch.OnCollider();
                robotAttackRariat.OnCollider();
            }
        }
    }

    // スクリプトが無効化されたとき呼び出す
    private void OnDisable()
    {
        if (this.gameObject.tag == "Axe")
        {
            this.transform.GetChild(1).gameObject.layer = LayerNumberEnemyShot;
            robotAttackChargeAxe.OffCollider();
            robotAttackSlashAxe.OffCollider();
        }
        else if (this.gameObject.tag == "Normal")
        {
            this.transform.GetChild(1).gameObject.layer = LayerNumberEnemyShot;
            this.transform.GetChild(2).gameObject.layer = LayerNumberEnemyShot;
            robotAttackPunch.OffCollider();
            robotAttackRariat.OffCollider();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ロボットが味方か敵か
        if (this.transform.parent.gameObject.layer == LayerNumberAlly || this.transform.parent.gameObject.layer == LayerNumberEnemy)
        {


            robotAction = transform.parent.GetComponent<RobotAction>();
            if (robotAction.CategoryHead == 0)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                //if (robotAction.robotState == RobotAction.RobotState.Idle || robotAction.robotState == RobotAction.RobotState.Attack)
                //{
                //    animator.SetBool("Walk", false);
                //}
                //else
                //{
                    animator.SetBool("Walk", true);
                //}
            }
        }
        else
        {

        }

        // 攻撃のクールタイム
        // isAttackableがfalseなら、直前のフレームからの経過時間を足す
        if (!isAttackable)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeがクールタイムを越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= coolTimeA && lapseTime >= coolTimeB)
            {
                isAttackable = true;
                lapseTime = 0.0f;
            }
        }
    }

    // ===============================================================================
    // 変数名: Attack
    // 攻撃をする（体の種類によって変化）
    // ===============================================================================
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

            int rand = Random.Range(1, 11);

            switch (weaponState)
            {
                case WeaponState.normal:
                    //確率で攻撃が変わる。
                    if (rand >= 4)
                    {
                        animator.SetTrigger("Punch");
                        StartCoroutine(NormalAttack(normalPreparationTimeA, normalOccurrencetimeA, normalFinishTimeA, 0));
                        robotAction.robotState = RobotAction.RobotState.Chase;
                    }
                    if (rand <= 3)
                    {
                        animator.SetTrigger("Rariat");
                        StartCoroutine(NormalAttack(normalPreparationTimeB, normalOccurrenceTimeB, normalFinishTimeB, 1));
                        robotAction.robotState = RobotAction.RobotState.Chase;
                    }
                    break;
                case WeaponState.gun:
                    if (rand >= 4)
                    {
                        animator.SetTrigger("Gun");
                        StartCoroutine(GunAttack(gunPreparationTimeA, 0));
                        robotAction.robotState = RobotAction.RobotState.Chase;
                    }
                    if (rand <= 3)
                    {
                        animator.SetTrigger("Missile");
                        StartCoroutine(GunAttack(gunPreparationTimeB, 1));
                        robotAction.robotState = RobotAction.RobotState.Chase;
                    }
                    break;
                case WeaponState.axe:
                    //確率で攻撃が変わる。
                    if (rand >= 4)
                    {
                        animator.SetTrigger("Slash");
                        StartCoroutine(AxeAttack(axePreparationTimeA, axeOccurrencetimeA, axeFinishTimeA, 0));
                        robotAction.robotState = RobotAction.RobotState.Chase;
                    }
                    if (rand <= 3)
                    {
                        animator.SetTrigger("Charge");
                        StartCoroutine(AxeAttack(axePreparationTimeB, axeOccurrenceTimeB, axeFinishTimeB, 1));
                        robotAction.robotState = RobotAction.RobotState.Chase;
                    }

                    break;
            }

            isAttackable = false;
        }
    }

    IEnumerator NormalAttack(float preparationTime,float occurrenceTime, float finishTime, int AttackType)
    {
        if (AttackType == 0)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            robotAttackPunch.OnCollider();
            if (audioSource)
            {
                audioSource.PlayOneShot(punchSound);
            }
            
            yield return new WaitForSeconds(occurrenceTime);
            robotAttackPunch.OffCollider();
            yield return new WaitForSeconds(finishTime);
        }
        else if (AttackType == 1)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            robotAttackRariat.OnCollider();
            if (audioSource)
            {
                audioSource.PlayOneShot(spinPunchSound);
            }
            
            yield return new WaitForSeconds(occurrenceTime);
            robotAttackRariat.OffCollider();
            yield return new WaitForSeconds(finishTime);
        }
        else
        {
            yield break;
        }
        //if (AttackType == 0)
        //{
        //    yield return new WaitForSeconds(preparationTime);
        //    robotAttackPunch.OnCollider();
        //    if (playerHp != null)
        //        playerHp.Damage(damage);
        //    if (robotHpList?.Count > 0)
        //    {
        //        foreach (var robotHp in robotHpList)
        //        {
        //            robotHp.HitDamage(damage);
        //        }
        //    }
        //}
        //if (AttackType == 1)
        //{
        //    yield return new WaitForSeconds(preparationTime);
        //    robotAttackRariat.OnCollider();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        var playerHp = robotAttackRariat.GetPlayerHp();
        //        var robotHpList = robotAttackRariat.GetRobotHpList();
        //        if (playerHp != null)
        //            playerHp.Damage(damage);
        //            Debug.Log("attackA");
        //        if (robotHpList?.Count > 0)
        //        {
        //            foreach (var robotHp in robotHpList)
        //            {
        //                robotHp.HitDamage(damage);
        //                Debug.Log("attackB");
        //            }
        //        }
        //        yield return new WaitForSeconds(1);
        //    }
        //    robotAttackRariat.OffCollider();
        //}
        //else
        //{
        //    yield break;
        //}
    }

    IEnumerator GunAttack(float preparationTime, int AttackType)
    {
        if (AttackType == 0)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            //距離とtransformを宣言
            float minDis = Mathf.Infinity;
            Transform minPos = null;
            if (playerHp != null)
            {
                //プレイヤーがいるならプレイヤーを初期値にする。
                minDis = Vector3.Distance(playerHp.transform.position, transform.position);
                minPos = playerHp.transform;
            }
            if (this.transform.parent.CompareTag("Enemy")){
                if (robotAction.Target)
                {
                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
                    shotPointRight.LookAt(robotAction.Target.transform.position);
                    shotPointLeft.LookAt(robotAction.Target.transform.position);
                    Instantiate(Enemybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
                    Instantiate(Enemybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);

                    if (audioSource)
                    {
                        audioSource.PlayOneShot(shotSound);
                    }
                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
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
                        audioSource.PlayOneShot(shotSound);
                    }
                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
                }
            }

        }
        else
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            //距離とtransformを宣言
            float minDis = Mathf.Infinity;
            Transform minPos = null;
            if (playerHp != null)
            {
                //プレイヤーがいるならプレイヤーを初期値にする。
                minDis = Vector3.Distance(playerHp.transform.position, transform.position);
                minPos = playerHp.transform;
            }
            if (this.transform.parent.CompareTag("Enemy")){
                if (robotAction.Target)
                {
                    //ショットポイントをminposの位置に向けてbulletを飛ばす。
                    shotPointRight.LookAt(robotAction.Target.transform.position);
                    shotPointLeft.LookAt(robotAction.Target.transform.position);
                    Instantiate(Enemybullet, shotPointRight.transform.position, shotPointRight.transform.rotation);
                    Instantiate(Enemybullet, shotPointLeft.transform.position, shotPointRight.transform.rotation);
                    if (audioSource)
                    {
                        audioSource.PlayOneShot(missileSound);
                    }
                    
                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
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
                        audioSource.PlayOneShot(missileSound);
                    }
                    
                    //Instantiate(bullet, shotPointRight.transform.position, shotPointRight.localRotation);
                    //Instantiate(bullet, shotPointLeft.transform.position, shotPointLeft.localRotation);
                }
            }
        }
    }

    IEnumerator AxeAttack(float preparationTime, float occurrenceTime, float finishTime, int AttackType)
    {
        if (AttackType == 0)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            robotAttackSlashAxe.OnCollider();
            yield return new WaitForSeconds(occurrenceTime);
            robotAttackSlashAxe.OffCollider();
            yield return new WaitForSeconds(finishTime);
        }
        else if (AttackType == 1)
        {
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            robotAttackChargeAxe.OnCollider();
            yield return new WaitForSeconds(occurrenceTime);
            robotAttackChargeAxe.OffCollider();
            yield return new WaitForSeconds(finishTime);
        }
        else
        {
            yield break;
        }
    }
}
