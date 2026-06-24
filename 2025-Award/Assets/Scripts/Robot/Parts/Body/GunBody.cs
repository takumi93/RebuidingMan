using UnityEngine;

public class GunBody : BodyBase
{
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
        base.Init();
        
        _robot = GetComponentInParent<Robot>();
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
        OnAttackStart();

        Animation.SetTrigger("AttackA");

        Damage = BodyData.DamageA;
        currentCoolTime = BodyData.CoolTimeA;
    }

    /// <summary>
    /// 攻撃Bの攻撃方法
    /// </summary>
    public override void AttackB()
    {
        OnAttackStart();

        Animation.SetTrigger("AttackB");

        Damage = BodyData.DamageB;
        currentCoolTime = BodyData.CoolTimeB;
    }

    //IEnumerator Attack(float preparationTime, int AttackType)
    //{
    //    // ターゲットがいないなら攻撃しない
    //    if (_robot.Target == null)
    //    {
    //        yield break;
    //    }

    //    //準備時間まで待つ
    //    yield return new WaitForSeconds(preparationTime);

    //    Transform _target = _robot.Target.transform;

    //    Vector3 flatTargetPos = new Vector3(
    //        _target.position.x,
    //        shotPointRight.position.y,
    //        _target.position.z
    //    );

    //    // 発射方向をターゲットへ向ける
    //    shotPointRight.LookAt(_target.position);
    //    shotPointLeft.LookAt(_target.position);

    //    GameObject bulletPrefab;

    //    if (transform.parent.CompareTag("Enemy"))
    //    {
    //        bulletPrefab = Enemybullet;
    //    }
    //    else
    //    {
    //        bulletPrefab = Allybullet;
    //    }

    //    if (AttackType == 0)
    //    {
    //        // 弾発射
    //        Instantiate(bulletPrefab, shotPointRight.position, shotPointRight.rotation);
    //        Instantiate(bulletPrefab, shotPointLeft.position, shotPointLeft.rotation);

    //        audioSource?.PlayOneShot(BodyData.attackSoundA);
    //    }
    //    else
    //    {
    //        // 弾発射
    //        Instantiate(bulletPrefab, shotPointRight.position, shotPointRight.rotation);
    //        Instantiate(bulletPrefab, shotPointLeft.position, shotPointLeft.rotation);

    //        audioSource?.PlayOneShot(BodyData.attackSoundB);
    //    }
    //}
}
