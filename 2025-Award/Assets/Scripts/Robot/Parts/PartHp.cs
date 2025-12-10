using UnityEngine;

public class PartHp : MonoBehaviour
{
    RobotHPManager _hpManager;  // 親のHP

    PartsData _partData { get; set; }

    [Tooltip("ダメージ倍率")]
    float _damageMultiplier = 1.0f;

    public int PartId {  get; private set; }

    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    private void Awake()
    {
        PartId = GetComponent<PartsPickup>().GetPartID();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(PartsDatabase partsDatabase)
    {
        _hpManager = GetComponentInParent<RobotHPManager>();
        _partData = partsDatabase.GetPartById(PartId);
        MaxHP = _partData.hp;
        CurrentHP = _partData.hp;
    }

    /// <summary>
    /// ロボットが特定の部位に攻撃を受けたときの処理
    /// </summary>
    /// <param name="damage"></param>
    public void PartDamage(int damage)
    {
        float actualDamage = damage * _damageMultiplier;
        CurrentHP = Mathf.Max(CurrentHP - (int)actualDamage, 0);
        _hpManager.HitDamage((int)actualDamage);
        _hpManager.CheckTotalHP();
        // 部位破壊チェック
        if (CurrentHP <= 0)
        {
            _hpManager.OnPartDead(this);
        }
    }
}
