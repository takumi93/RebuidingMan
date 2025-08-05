using System;
using System.Collections.Generic;
using UnityEngine;

public enum PartsType
{
    Head,   // “ª
    Body,   // ‘Ì
    Leg     // ‘«
}

[Serializable]
public class Parts
{
    public int id;
    public Sprite icon;
    public PartsType partsType;
    public GameObject partsObject;
    public string partsName;
    public int hp;
    public int defense;
    public int speed;
    public string description;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class PartsData : ScriptableObject
{
    public List<Parts> partsList;
}
