using UnityEngine;

public class GunBody : BodyBase
{
    [Header("gun‚Ìƒpƒ‰ƒ[ƒ^")]

    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private Transform[] _attackAShotPoints;
    [SerializeField] private Transform[] _attackBShotPoints;

    /// <summary>
    /// ‰Šúİ’è
    /// </summary>
    public override void Init(Robot robot)
    {
        base.Init(robot);
    }

    /// <summary>
    /// –¡•û‚Æ‚µ‚Ä¶¬‚·‚é‚Ìˆ—
    /// </summary>
    public override void CreateSetup()
    {
        UpdateMaterial(BodyData);
    }

    /// <summary>
    /// UŒ‚A‚ÌUŒ‚•û–@
    /// </summary>
    public override void AttackA()
    {
        OnAttackStart();

        Animation.SetTrigger("AttackA");

        Damage = BodyData.DamageA;
        currentCoolTime = BodyData.CoolTimeA;
    }

    /// <summary>
    /// UŒ‚B‚ÌUŒ‚•û–@
    /// </summary>
    public override void AttackB()
    {
        OnAttackStart();

        Animation.SetTrigger("AttackB");

        Damage = BodyData.DamageB;
        currentCoolTime = BodyData.CoolTimeB;
    }

    public override void AttackAEvent()
    {
        Fire(_attackAShotPoints, BodyData.AttackSoundA);
    }

    public override void AttackBEvent()
    {
        Fire(_attackBShotPoints, BodyData.AttackSoundB);
    }

    public void Fire(Transform[] shotPoints, AudioClip clip)
    {
        if (!_robot.Target) return;

        Transform target = _robot.Target.transform;

        // ’e‚Ì¶¬‚Æ’e‚Ì‰Šú‰»
        foreach (Transform shotPoint in shotPoints)
        {
            shotPoint.LookAt(target.position);

            GameObject bullet = Instantiate(_bulletPrefab, shotPoint.position, shotPoint.rotation);

            bullet.GetComponent<Bullet>().Init(_robot);
        }

        audioSource?.PlayOneShot(clip);
    }
}
