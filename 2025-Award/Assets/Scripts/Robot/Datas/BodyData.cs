using UnityEngine;

[CreateAssetMenu(fileName = "BodyData", menuName = "Scriptable Objects/BodyData")]
public class BodyData : PartsData
{
    [Header("体の設定項目")]
    [Tooltip("頭のタイプ")]
    [SerializeField]
    private BodyType _bodyType;

    public BodyType BodyType => _bodyType;

    [Tooltip("攻撃Aのダメージ割合")]
    [SerializeField]
    private int _damageA;

    public int DamageA => _damageA;

    [Tooltip("攻撃Bのダメージ割合")]
    [SerializeField]
    private int _damageB;

    public int DamageB => _damageB;

    [Tooltip("攻撃Aのクールタイム")]
    [SerializeField]
    private float _coolTimeA;

    public float CoolTimeA => _coolTimeA;

    [Tooltip("攻撃Bのクールタイム")]
    [SerializeField]
    private float _coolTimeB;

    public float CoolTimeB => _coolTimeB;

    [Tooltip("攻撃Aの攻撃サウンド")]
    [SerializeField]
    private AudioClip _attackSoundA;

    public AudioClip AttackSoundA => _attackSoundA;

    [Tooltip("攻撃Bの攻撃サウンド")]
    [SerializeField]
    private AudioClip _attackSoundB;

    public AudioClip AttackSoundB => _attackSoundB; 

    [Tooltip("攻撃に入る距離")]
    [SerializeField]
    private int _attackRange;

    public int AttackRange => _attackRange;

    [Tooltip("移動先で止まる距離")]
    [SerializeField]
    private float _stoppingDistance;

    public float StoppingDistance => _stoppingDistance;

    public override PartsType GetPartsType() => PartsType.Body;
}

public enum BodyType
{
    Normal, // ノーマル
    Gun,    // 銃
    Axe     // 斧
}
