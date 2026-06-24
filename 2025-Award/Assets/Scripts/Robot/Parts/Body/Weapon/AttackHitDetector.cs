using UnityEngine;

public class AttackHitDetector : MonoBehaviour
{
    private WeaponBase _weapon;

    private void Awake()
    {
        _weapon = GetComponentInParent<WeaponBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _weapon?.OnHit(other);
    }
}
