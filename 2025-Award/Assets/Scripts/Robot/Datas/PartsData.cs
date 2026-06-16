using UnityEngine;

public abstract class PartsData : ScriptableObject
{
    [Tooltip("ロボットのデータ")]
    [SerializeField] private int _id;
    [SerializeField] private string _partName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private GameObject _prefab;

    [Tooltip("ロボットの個体値")]
    [SerializeField] private int _hp;
    [SerializeField] private int _defense;
    [SerializeField] private int _speed;

    [Tooltip("味方になった時のMaterial")]
    [SerializeField] private Material _allyMaterial;

    // 値を参照させるための変数
    public int Id => _id;
    public string PartName => _partName;
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;
    public int Hp => _hp;
    public int Defense => _defense;
    public int Speed => _speed;
    public Material AllyMaterial => _allyMaterial;

    public abstract PartsType GetPartsType();
}

public enum PartsType
{
    Head,
    Body,
    Leg
}
