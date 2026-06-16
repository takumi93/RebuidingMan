using UnityEngine;

public class Normal : WeaponBase
{
    // UŒ‚‚Ì“–‚½‚è”»’è
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
