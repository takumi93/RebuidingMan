using System.Collections;
using UnityEngine;

public class GunBody : BodyBase
{
    [Header("頭に連動するRigを指定")]
    [SerializeField] public GameObject BodyToHeadRig = null;
    [Header("足に連動するRigを指定")]
    [SerializeField] public GameObject BodyToLegRig = null;

    [Header("gun時のパラメータ")]

    [SerializeField] GameObject Allybullet;
    [SerializeField] GameObject Enemybullet;

    [SerializeField] Transform shotPointRight;
    [SerializeField] Transform shotPointLeft;

    Robot _robot { get; set; }

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void Init()
    {
        Animation = GetComponentInChildren<RobotAnimation>();
        audioSource = GetComponentInParent<AudioSource>();
        _robot = GetComponentInParent<Robot>();

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
        GetComponentInChildren<SkinnedMeshRenderer>().material = BodyData.material;
        audioSource = this.GetComponentInParent<AudioSource>();
    }

    /// <summary>
    /// 攻撃Aの攻撃方法
    /// </summary>
    public override void AttackA()
    {
        Animation.SetTrigger("AttackA");
        StartCoroutine(Attack(BodyData.preparationTimeA, 0));

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
        StartCoroutine(Attack(BodyData.preparationTimeB, 1));

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

    IEnumerator Attack(float preparationTime, int AttackType)
    {
        // ターゲットがいないなら攻撃しない
        if (_robot.Target == null)
        {
            yield break;
        }

        //準備時間まで待つ
        yield return new WaitForSeconds(preparationTime);

        Transform _target = _robot.Target.transform;

        Vector3 flatTargetPos = new Vector3(
            _target.position.x,
            shotPointRight.position.y,
            _target.position.z
        );

        // 発射方向をターゲットへ向ける
        shotPointRight.LookAt(_target.position);
        shotPointLeft.LookAt(_target.position);

        GameObject bulletPrefab;

        if (transform.parent.CompareTag("Enemy"))
        {
            bulletPrefab = Enemybullet;
        }
        else
        {
            bulletPrefab = Allybullet;
        }

        if (AttackType == 0)
        {
            // 弾発射
            Instantiate(bulletPrefab, shotPointRight.position, shotPointRight.rotation);
            Instantiate(bulletPrefab, shotPointLeft.position, shotPointLeft.rotation);

            audioSource?.PlayOneShot(BodyData.attackSoundA);
        }
        else
        {
            // 弾発射
            Instantiate(bulletPrefab, shotPointRight.position, shotPointRight.rotation);
            Instantiate(bulletPrefab, shotPointLeft.position, shotPointLeft.rotation);

            audioSource?.PlayOneShot(BodyData.attackSoundB);
        }
    }
}
