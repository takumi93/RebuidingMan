using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    [SerializeField] private PartsData partData;

    public PartsData GetPartData() => partData;
}
