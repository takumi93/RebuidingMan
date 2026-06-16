using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    [SerializeField]public int Id;

    public int GetPartID() => Id;
}
