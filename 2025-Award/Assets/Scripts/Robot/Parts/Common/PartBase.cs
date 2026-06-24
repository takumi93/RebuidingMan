using UnityEngine;

public abstract class PartBase : MonoBehaviour
{
    [SerializeField] protected int _id;

    public int Id => _id;
}