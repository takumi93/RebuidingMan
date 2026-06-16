using UnityEngine;

public class AttackHitDetector : MonoBehaviour
{
    private WeaponBase weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<WeaponBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        weapon?.OnHit(other);
    }
}
