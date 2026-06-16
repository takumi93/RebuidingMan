using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected BodyBase body;
    protected TeamObject teamObject;
    protected Robot robot;

    // •Ší‚É“–‚½‚Á‚½”»’è‚ğc‚·‚½‚ß‚Ì•Ï”
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    public virtual void Init()
    {
        body = GetComponentInParent<BodyBase>();
        teamObject = GetComponentInParent<TeamObject>();
        robot = GetComponentInParent<Robot>();
    }

    /// <summary>
    /// “–‚½‚è”»’èON
    /// </summary>
    public virtual void HitOn()
    {
        // UŒ‚2‰ñ–ÚˆÈ~‚ÌÛ‚É‚»‚Ì‘O‚Ég‚Á‚½“à—e‚ğƒNƒŠƒA‚·‚é‚½‚ß
        hitTargets.Clear();
    }


    /// <summary>
    /// “–‚½‚è”»’èOFF
    /// </summary>
    public virtual void HitOff()
    {

    }

    /// <summary>
    /// UŒ‚‚ª“–‚½‚Á‚½‚Ìˆ—
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnHit(Collider other)
    {
        // ©•ª‚Í–³‹
        if (other.transform.IsChildOf(robot.transform)) return;

        // w‰c‚ğæ“¾
        TeamObject target = other.transform.GetComponentInParent<TeamObject>();

        // –¡•û‚Í–³‹
        if (target != null && target.GetTeamType() == teamObject.GetTeamType()) return;

        // ‘½’iHit‚ğ–h~‚·‚é‚½‚ß
        GameObject root = other.transform.root.gameObject;

        if (hitTargets.Contains(root)) return;

        hitTargets.Add(root);

        ApplyDamage(other);
    }

    /// <summary>
    /// UŒ‚‚Ìˆ—
    /// </summary>
    /// <param name="other"></param>
    protected virtual void ApplyDamage(Collider other)
    {
        if(other.GetComponentInParent<PlayerHP>() is PlayerHP playerHP)
        {
            playerHP.Damage(body.Damage, robot.gameObject);
        }
        if(other.GetComponentInParent<RobotHPManager>() is  RobotHPManager robotHP)
        {
            robotHP.ApplyTotalDamage(body.Damage, robot.gameObject);
        }
    }
}
