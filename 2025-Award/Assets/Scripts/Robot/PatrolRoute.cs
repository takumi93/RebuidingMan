using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField]
    private Transform[] _points;

    private void Awake()
    {
        // 自動でポイントを取得する
        _points = new Transform[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            _points[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 指定した巡回ポイントを取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Transform GetPoint(int index)
    {
        return _points[index];
    }

    /// <summary>
    /// 巡回ポイントの数を取得
    /// </summary>
    /// <returns></returns>
    public int GetPointLength()
    {
        return _points.Length;
    }
}
