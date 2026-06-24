using UnityEngine;

public class AxeBody : BodyBase
{
    public Axe Axe { get; private set; }

    /// <summary>
    /// ‰Šúİ’è
    /// </summary>
    public override void Init()
    {
        base.Init();

        Axe = GetComponentInChildren<Axe>();
        Axe.Init();
    }

    /// <summary>
    /// –¡•û‚Æ‚µ‚Ä¶¬‚·‚é‚Ìˆ—
    /// </summary>
    public override void CreateSetup()
    {
        transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().material = BodyData.AllyMaterial;
        audioSource = GetComponentInParent<AudioSource>();
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
