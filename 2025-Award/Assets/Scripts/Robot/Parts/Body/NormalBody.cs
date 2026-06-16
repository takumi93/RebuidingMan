using System.Collections;
using UnityEngine;

public class NormalBody : BodyBase
{
    [Header("頭に連動するRigを指定")]
    [SerializeField] public GameObject BodyToHeadRig = null;
    [Header("足に連動するRigを指定")]
    [SerializeField] public GameObject BodyToLegRig = null;

    public Normal Normal {  get; private set; }

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void Init()
    {
        Animation = GetComponentInChildren<RobotAnimation>();
        Normal = GetComponentInChildren<Normal>();
        audioSource = GetComponentInParent<AudioSource>();

        Normal.Init();
        IsAttackable = true;

        lapseTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ConnectRig)
        {
            BodyToLegRig.transform.position = ConnectRig.transform.position;
        }

        // 攻撃のクールタイム
        // isAttackableがfalseなら、直前のフレームからの経過時間を足す
        if (!IsAttackable)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeがクールタイムを越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= currentCoolTime)
            {
                IsAttackable = true;
                lapseTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// 味方として生成する時の処理
    /// </summary>
    public override void CreateSetup()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = BodyData.AllyMaterial;
        audioSource = this.GetComponentInParent<AudioSource>();
    }
    
    /// <summary>
    /// 攻撃Aの攻撃方法
    /// </summary>
    public override void AttackA()
    {
        Animation.SetTrigger("AttackA");
        Damage = BodyData.damageA;
        StartCoroutine(Attack(BodyData.preparationTimeA, BodyData.occurrenceTimeA, BodyData.finishTimeA, 0));

        currentCoolTime = BodyData.coolTimeA;
        IsAttackable = false;
        lapseTime = 0f;
    }

    /// <summary>
    /// 攻撃Bの攻撃方法
    /// </summary>
    public override void AttackB()
    {
        Animation.SetTrigger("AttackB");
        Damage = BodyData.damageB;
        StartCoroutine(Attack(BodyData.preparationTimeB, BodyData.occurrenceTimeB, BodyData.finishTimeB, 1));

        currentCoolTime = BodyData.coolTimeB;
        IsAttackable = false;
        lapseTime = 0f;
    }

    public override int GetDamageA()
    {
        return BodyData.damageA;
    }

    public override int GetDamageB()
    {
        return BodyData.damageB;
    }

    /// <summary>
    /// 攻撃の発生時間の詳細指定
    /// </summary>
    /// <param name="preparationTime">攻撃モーション発生までの時間</param>
    /// <param name="occurrenceTime">当たり判定生成時間</param>
    /// <param name="finishTime">当たり判定が終わった後の待機時間</param>
    /// <param name="AttackType"></param>
    /// <returns></returns>
    IEnumerator Attack(float preparationTime, float occurrenceTime, float finishTime, int AttackType)
    {
        if (AttackType == 0)
        {
            Damage = GetDamageA();
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            Normal.HitOn();
            audioSource?.PlayOneShot(BodyData.attackSoundA);
            yield return new WaitForSeconds(occurrenceTime);
            Normal.HitOff();
            yield return new WaitForSeconds(finishTime);
        }
        else if (AttackType == 1)
        {
            Damage = GetDamageB();
            //準備時間まで待つ
            yield return new WaitForSeconds(preparationTime);
            Normal.HitOn();
            audioSource?.PlayOneShot(BodyData.attackSoundB);
            yield return new WaitForSeconds(occurrenceTime);
            Normal.HitOff();
            yield return new WaitForSeconds(finishTime);
        }
    }
}
