using UnityEngine;

public class Normal : MonoBehaviour
{
    private BoxCollider[] _hitCollider;
    private NormalBody _body;

    /// <summary>
    /// èâä˙ê›íË
    /// </summary>
    public void Init()
    {
        _hitCollider = GetComponentsInChildren<BoxCollider>();
        _body = GetComponentInParent<NormalBody>();
        foreach (var hit in _hitCollider)
        {
            hit.enabled = false;
        }
    }

    /// <summary>
    /// ìñÇΩÇËîªíËON
    /// </summary>
    public void HitOn()
    {
        foreach (var hit in _hitCollider)
        {
            hit.enabled = true;
        }
    }

    /// <summary>
    /// ìñÇΩÇËîªíËOFF
    /// </summary>
    public void HitOff()
    {
        foreach (var hit in _hitCollider)
        {
            hit.enabled = false;
        }
    }

    public void OnChildTriggerEnter(Collider other)
    {
        var hpManager = other.GetComponentInParent<RobotHPManager>();
        if (hpManager != null)
        {
            hpManager.HitDamage(_body.Damage);
            return;
        }

        var playerHP = other.GetComponentInParent<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.Damage(_body.Damage);
            return;
        }
    }
}
