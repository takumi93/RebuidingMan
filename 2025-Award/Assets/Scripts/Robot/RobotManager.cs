using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    // インスタンス化
    public static RobotManager Instance { get; private set; }

    // ステージにいるロボットを管理するためのリスト
    private List<Robot> _robots = new List<Robot>();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ロボットのリストに登録する
    /// </summary>
    /// <param name="robot"></param>
    public void Register(Robot robot)
    {
        if (!_robots.Contains(robot))
        {
            _robots.Add(robot);
        }
    }

    /// <summary>
    /// ロボットのリストから除外する
    /// </summary>
    /// <param name="robot"></param>
    public void UnRegister(Robot robot)
    {
        _robots.Remove(robot);
    }

    /// <summary>
    /// 指定した陣営のロボットをリスト化し取得する
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public List<Robot> GetTeamRobots(TeamType team)
    {
        return _robots.Where(r => r.TeamType == team).ToList();
    }

    /// <summary>
    /// 一番近い味方を取得する
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public Robot GetNearestAlly(Robot self)
    {
        return _robots
            // 自分は無視
            .Where(r => r != self)
            // 自分と同じ陣営を検索
            .Where(r => r.TeamType == self.TeamType)
            // 一番近い人を取得
            .OrderBy(r => Vector3.Distance(self.transform.position, r.transform.position))
            .FirstOrDefault();
    }

    /// <summary>
    /// 一番近い敵取得
    /// </summary>
    public Robot GetNearestEnemy(Robot self)
    {
        return _robots
            // 自分と違う陣営を検索
            .Where(r => r.TeamType != self.TeamType)
            // 一番近い物を取得
            .OrderBy(r => Vector3.Distance(self.transform.position, r.transform.position))
            .FirstOrDefault();
    }
}
