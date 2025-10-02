using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PartsData : ScriptableObject
{
    public int id;
    public string partName;
    public Sprite icon;
    public GameObject prefab;
    [Tooltip("‘Ì—Í")]
    public int hp;
    [Tooltip("–hŒä—Í")]
    public int defense;
    [Tooltip("‘¬“x")]
    public int speed;
    [Tooltip("–¡•û‚É‚È‚Á‚½Žž‚ÌMaterial")]
    public Material material;
    public string description;

    public abstract PartsType GetPartsType();
}

public enum PartsType
{
    Head,
    Body,
    Leg
}
