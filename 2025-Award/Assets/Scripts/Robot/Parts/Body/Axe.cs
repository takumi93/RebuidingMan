using UnityEngine;

public class Axe : MonoBehaviour
{
    private BoxCollider _hitCollider;
    private AxeBody _body;

    /// <summary>
    /// ‰Šúİ’è
    /// </summary>
    public void Init()
    {
        _hitCollider = GetComponent<BoxCollider>();
        _hitCollider.enabled = false;
        _body = GetComponentInParent<AxeBody>();
    }

    /// <summary>
    /// “–‚½‚è”»’èON
    /// </summary>
    public void HitOn()
    {
        _hitCollider.enabled = true;
    }

    /// <summary>
    /// “–‚½‚è”»’èOFF
    /// </summary>
    public void HitOff()
    {
        _hitCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
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
