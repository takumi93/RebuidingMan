using UnityEngine;

public class AttackHitDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var attack = GetComponentInParent<Normal>();
        if (attack != null)
        {
            attack.OnChildTriggerEnter(other);
        }
    }
}
