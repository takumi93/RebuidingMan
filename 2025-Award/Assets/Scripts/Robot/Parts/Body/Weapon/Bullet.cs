using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TeamType Team {  get; private set; }

    public int Damage {  get; private set; }

    public Robot Robot { get; private set; }

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    /// <param name="team"></param>
    /// <param name="damage"></param>
    public void Init(TeamType team, int damage, Robot robot)
    {
        Team = team;
        Damage = damage;
        Robot = robot;
    }
}
