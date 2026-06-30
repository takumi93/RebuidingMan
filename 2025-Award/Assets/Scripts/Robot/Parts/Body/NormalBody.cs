using UnityEngine;

public class NormalBody : BodyBase
{
    /// <summary>
    /// ‰Šúİ’è
    /// </summary>
    public override void Init(Robot robot)
    {
        base.Init(robot);

        Weapon = GetComponentInChildren<Normal>();
        Weapon.Init();
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
        audioSource.PlayOneShot(BodyData.AttackSoundA);

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
        audioSource.PlayOneShot(BodyData.AttackSoundB);

        Damage = BodyData.DamageB;
        currentCoolTime = BodyData.CoolTimeB;
    }
}
