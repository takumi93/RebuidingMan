using UnityEngine;

public class Axe : WeaponBase
{
    private BoxCollider[] _hitCollider;

    public override void Init()
    {
        base.Init();

        _hitCollider = GetComponentsInChildren<BoxCollider>();

        HitOff();
    }

    public override void HitOn()
    {
        base.HitOn();

        foreach (var hit in _hitCollider)
        {
            hit.enabled = true;
        }
    }

    public override void HitOff()
    {
        foreach (var hit in _hitCollider)
        {
            hit.enabled = false;
        }
    }
}
