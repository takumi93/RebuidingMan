using UnityEngine;

/// <summary>
/// ƒƒ{ƒbƒg‚Ìw‰c‚ğ¯•Ê‚·‚é‚½‚ß
/// </summary>
public class TeamObject : MonoBehaviour
{
    [SerializeField] private TeamType _team = TeamType.Enemy;

    /// <summary>
    /// w‰c‚ÌŠm”F
    /// </summary>
    /// <returns></returns>
    public TeamType GetTeamType()
    {
        return _team;
    }

    /// <summary>
    /// w‰c‚Ì•ÏX
    /// </summary>
    /// <param name="team"></param>
    public void SetTeam(TeamType team)
    {
        _team = team;
    }

    /// <summary>
    /// “G‚©–¡•û‚Ì”»’è
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEnemy(TeamObject other)
    {
        // ‰½‚à‚È‚©‚Á‚½‚çFalse
        if (other == null) return false;

        // “¯‚¶‚È‚çFalseA“G‚È‚çTrue
        if(_team != other._team) return true;
        else return false;
    }
}