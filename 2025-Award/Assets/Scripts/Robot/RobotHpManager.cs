using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotHPManager : MonoBehaviour
{  
    [SerializeField]private PartsDatabase _partsDatabase = null;

    [SerializeField] private PlayUI _playUI = null;

    [Header("HP管理")]
    private bool _isDead;

    public bool IsDead => _isDead;

    public List<PartHp> Parts {  get; private set; }

    public int MaxTotalHP { get; private set; }

    public int CurrentTotalHP { get; private set; }

    private const int _endurance = 3;                        // 耐久値からHPを求めるために使用する値

    [Header("爆発に使用する設定")]
    [SerializeField] private GameObject _explosionEffect;     //死んだときのエフェクト

    [SerializeField] private float _explosionRadius = 10f;  //爆発の広さ
    
    [SerializeField] private float _explosionPower = 100f;  //爆発の強さ

    private Robot _robot { get; set; }

    void Start()
    {
        // 新規追加
        Parts = GetComponentsInChildren<PartHp>().ToList();
        _robot = GetComponent<Robot>();

        // HPの初期化
        MaxTotalHP = 0;
        foreach (var p in Parts)
        {
            p.Init(_partsDatabase);
            MaxTotalHP += p.MaxHP;
        }
        MaxTotalHP = MaxTotalHP / _endurance;
        CurrentTotalHP = MaxTotalHP;
    }

    /// <summary>
    /// 指定の部位を破壊
    /// </summary>
    /// <param name="deadPart"></param>
    public void OnPartDead(PartHp deadPart)
    {
        if (_isDead) return;

        DestroyRobot(deadPart);
    }

    /// <summary>
    /// 一番HPが低い部位を破壊しそれ以外をステージに残す
    /// </summary>
    public void DestroyRobotByTotalHP()
    {
        if (_isDead) return;

        var target = Parts
            .Where(p => p.CurrentHP >= 0)    // まだ壊れてない部位
            .OrderBy(p => p.CurrentHP)
            .FirstOrDefault();

        if (target == null)
        {
            return;
        }

        DestroyRobot(target);
    }

    /// <summary>
    /// 指定のオブジェクトだけ削除してそれ以外は親から外し削除
    /// </summary>
    /// <param name="deadPart">破壊するオブジェクト</param>
    public void DestroyRobot(PartHp deadPart)
    {
        if (_isDead) return;

        _isDead = true;

        DetachPart(_robot.Head, deadPart);
        DetachPart(_robot.Body, deadPart);
        DetachPart(_robot.Leg, deadPart);

        // 頭にアニメーションはないため無視
        _robot.Body?.Animation.DestoryAnimation();
        _robot.Leg?.Animation.DestoryAnimation();

        _robot.Body?.Weapon?.HitOff();

        //破壊時のエフェクト
        {
            Instantiate(_explosionEffect, new Vector3(transform.position.x, 1.5f, transform.position.z), transform.rotation);

            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (Collider collider in colliders)
            {
                Rigidbody rb = collider.attachedRigidbody;

                if (rb != null)
                {
                    rb.AddExplosionForce(
                        _explosionPower,
                        new Vector3(transform.position.x, 0, transform.position.z),
                        _explosionRadius);
                }
            }
        }

        _playUI?.EnemyDecrease();

        // 親を削除
        Destroy(gameObject);
    }

    /// <summary>
    /// 部品をロボットから切り離す
    /// </summary>
    /// <param name="part">切り離す部品</param>
    /// <param name="deadPart">破壊される部品</param>
    private void DetachPart(PartBase part, PartHp deadPart)
    {
        if (part == null) return;
        if (part.GetComponent<PartHp>() == deadPart) return;

        part.transform.SetParent(transform.parent, true);

        DropUtility.SetupDroppedPart(part.gameObject);
    }

    /// <summary>
    /// 最後に攻撃してきた敵を保存
    /// </summary>
    /// <param name="damage">与えるダメージ</param>
    /// <param name="attacker">攻撃してきた敵</param>
    public void ApplyTotalDamage(int damage, GameObject attacker)
    {
        CurrentTotalHP = Mathf.Max(CurrentTotalHP - damage, 0);

        if (attacker.TryGetComponent<Robot>(out var robot))
        {
            _robot.LastAttacker = robot;
        }

        if(CurrentTotalHP <= 0)
        {
            DestroyRobotByTotalHP();
        }
    }
}
