using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    private PartBase part;

    private void Awake()
    {
        part = GetComponent<PartBase>();
    }

    public int Id => part.Id;
}
