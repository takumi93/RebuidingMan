using UnityEngine;

public class PartHp : MonoBehaviour
{
    private RobotHPManager _robotHP;  // 親のHP

    private PartsData _partData;

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
        _robotHP = GetComponentInParent<RobotHPManager>();
        _partData = partsDatabase.GetPartById(PartId);
        MaxHP = _partData.Hp;
        CurrentHP = _partData.Hp;
    }

    /// <summary>
    /// ロボットの部品のダメージ処理と破壊されていないかのチェック
    /// </summary>
    public void ApplyPartDamage(int damage, GameObject attacker)
    {
        // ダメージ倍率を合わせたダメージ
        float actualDamage = damage * _damageMultiplier;

        // 部品へのダメージ処理
        CurrentHP = Mathf.Max(CurrentHP - (int)actualDamage, 0);
        // ロボット本体へのダメージ処理
        _robotHP.ApplyTotalDamage((int)actualDamage, attacker);

        // 部位破壊チェック
        if (CurrentHP <= 0)
        {
            _robotHP.OnPartDead(this);
        }
    }
}
