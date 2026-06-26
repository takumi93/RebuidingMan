using UnityEngine;

public class NormalBody : BodyBase
{
    /// <summary>
    /// ‰Šúİ’è
    /// </summary>
    public override void Init()
    {
        base.Init();

        Weapon = GetComponentInChildren<Normal>();
        Weapon.Init();
    }

    /// <summary>
    /// –¡•û‚Æ‚µ‚Ä¶¬‚·‚é‚Ìˆ—
    /// </summary>
    public override void CreateSetup()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = BodyData.AllyMaterial;
        audioSource = this.GetComponentInParent<AudioSource>();
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
}
